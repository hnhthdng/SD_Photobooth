"use client";
import { useCallback, useEffect, useMemo, useState } from "react";
import { columns } from "./columns";
import { User } from "@/types/type";
import AxiosAPI from "@/configs/axios";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  flexRender,
  getCoreRowModel,
  useReactTable,
} from "@tanstack/react-table";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";

import AddManager from "@/components/component/AddManager";

interface ApiResponse {
  data: User[];
  totalCount: number;
}

const useCustomerData = () => {
  const [data, setData] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [totalItems, setTotalItems] = useState(0);

  const fetchData = useCallback(async (pageIndex: number, pageSize: number) => {
    try {
      setLoading(true);
      const response = await AxiosAPI.get<ApiResponse>("/api/User/manager", {
        params: {
          PageNumber: pageIndex + 1,
          PageSize: pageSize,
        },
      });
      setData(response.data?.data || []);
      setTotalItems(response.data?.totalCount || 0);
      setError(null);
    } catch (err: any) {
      setError(err.message || "Failed to fetch data");
      setData([]);
      setTotalItems(0);
    } finally {
      setLoading(false);
    }
  }, []);

  return {
    data,
    loading,
    error,
    totalItems,
    fetchData,
  };
};

const ManagerPage = () => {
  const [pageSize, setPageSize] = useState(5);
  const [pageIndex, setPageIndex] = useState(0);

  const { data, loading, error, totalItems, fetchData } = useCustomerData();

  const totalPages = Math.max(1, Math.ceil(totalItems / pageSize));

  const memoizedColumns = useMemo(() => columns(), []);

  const table = useReactTable({
    data,
    columns: memoizedColumns,
    pageCount: totalPages,
    state: {
      pagination: {
        pageIndex,
        pageSize,
      },
    },
    manualPagination: true,
    getCoreRowModel: getCoreRowModel(),
    onPaginationChange: (updater) => {
      const newPagination =
        typeof updater === "function"
          ? updater({ pageIndex, pageSize })
          : updater;
      setPageIndex(
        Math.min(Math.max(newPagination.pageIndex, 0), totalPages - 1)
      );
      setPageSize(newPagination.pageSize);
    },
  });

  useEffect(() => {
    fetchData(pageIndex, pageSize);
  }, [fetchData, pageIndex, pageSize]);

  const handlePageSizeChange = useCallback(
    (e: React.ChangeEvent<HTMLSelectElement>) => {
      const newSize = Number(e.target.value);
      setPageSize(newSize);

      setPageIndex(0);
    },
    []
  );

  const handlePrevious = useCallback(() => {
    setPageIndex((prev) => Math.max(prev - 1, 0));
  }, []);

  const handleNext = useCallback(() => {
    setPageIndex((prev) => Math.min(prev + 1, totalPages - 1));
  }, [totalPages]);

  return (
    <div className="w-full space-y-4">
      <div className="flex items-center justify-between py-4">
        <div className="flex items-center space-x-2">
          <AddManager
            onSuccess={() => {
              setPageIndex(0);
            }}
          />
          <Label htmlFor="pageSize" className="text-sm">
            Số hàng/trang:
          </Label>
          <select
            id="pageSize"
            value={pageSize}
            onChange={handlePageSizeChange}
            className="border border-gray-300 rounded px-2 py-1 text-sm"
          >
            {[5, 10, 15, 20].map((size) => (
              <option key={size} value={size}>
                {size}
              </option>
            ))}
          </select>
        </div>
      </div>

      {loading ? (
        <div className="text-sm text-muted-foreground">Đang tải dữ liệu...</div>
      ) : error ? (
        <div className="text-red-500 p-4">Error: {error}</div>
      ) : (
        <>
          <div className="rounded-md border">
            <Table>
              <TableHeader>
                {table.getHeaderGroups().map((headerGroup) => (
                  <TableRow key={headerGroup.id}>
                    {headerGroup.headers.map((header) => (
                      <TableHead key={header.id}>
                        {flexRender(
                          header.column.columnDef.header,
                          header.getContext()
                        )}
                      </TableHead>
                    ))}
                  </TableRow>
                ))}
              </TableHeader>
              <TableBody>
                {table.getRowModel().rows?.length ? (
                  table.getRowModel().rows.map((row) => (
                    <TableRow key={row.id}>
                      {row.getVisibleCells().map((cell) => (
                        <TableCell key={cell.id}>
                          {flexRender(
                            cell.column.columnDef.cell,
                            cell.getContext()
                          )}
                        </TableCell>
                      ))}
                    </TableRow>
                  ))
                ) : (
                  <TableRow>
                    <TableCell
                      colSpan={memoizedColumns.length}
                      className="text-center"
                    >
                      No results
                    </TableCell>
                  </TableRow>
                )}
              </TableBody>
            </Table>
          </div>

          <div className="flex items-center justify-end space-x-2 py-4">
            <Button
              variant="outline"
              size="sm"
              onClick={handlePrevious}
              disabled={pageIndex === 0 || loading}
            >
              Previous
            </Button>

            <span className="text-sm text-muted-foreground">
              Page {pageIndex + 1} of {totalPages}
            </span>

            <Button
              variant="outline"
              size="sm"
              onClick={handleNext}
              disabled={pageIndex >= totalPages - 1 || loading || !totalItems}
            >
              Next
            </Button>
          </div>
        </>
      )}
    </div>
  );
};

export default ManagerPage;
