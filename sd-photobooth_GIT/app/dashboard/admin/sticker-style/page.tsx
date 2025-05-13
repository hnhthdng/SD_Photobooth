'use client';

import { columns } from "./columns";
import { GenericTable } from "@/components/component/GenericTable";
import { usePaginatedTable } from "@/hooks/useTablePagination";
import {
  createStickerStyle,
  getStickerStyleCount,
  getStickerStyles,

} from "@/services/sticker-style";

import { PaginationControl } from "@/components/component/PaginationControls";
import { StickerStyle } from "@/types/type";
import SkeletonNew from "@/components/component/SkeletonNew";
import { PageSizeSelector } from "@/components/component/PageSizeSelector";

import { SimpleAddItem } from "@/components/component/AddGeneric";
import { StickerStyleSchema } from "@/types/schema/sticker-style";

const StickerStyleTable = () => {
  const {
    data,
    isLoading,
    error,
    pageIndex,
    setPageIndex,
    pageSize,
    setPageSize,
    totalItems,

  } = usePaginatedTable<StickerStyle>({
    queryKey: "stickerStyles",
    fetchDataFn: getStickerStyles,
    fetchCountFn: getStickerStyleCount,
    // searchFn: searchPaymentMethodsByName,
    initialPageSize: 5,

  });


  const handlePageSizeChange = (newSize: number) => {
    setPageSize(newSize);
    setPageIndex(0);
  };

  if (isLoading) return <SkeletonNew columns={5} rows={6} />;
  if (error) return <p>Something went wrong</p>;

  return (
    <div>
      <div className="flex justify-end items-center mb-3 gap-5">
        <SimpleAddItem

          queryKey="stickerStyles"
          schema={StickerStyleSchema}
          createFn={createStickerStyle}
          fields={[
            { name: "stickerStyleName", label: "Tên", placeholder: "Nhập tên" },
            { name: "description", label: "Mô tả", placeholder: "Nhập mô tả" },
          ]}
        />
        <PageSizeSelector
          value={pageSize}
          onChange={handlePageSizeChange}
        />
      </div>
      <GenericTable data={data} columns={columns} />
      <PaginationControl
        pageIndex={pageIndex}
        pageSize={pageSize}
        totalItems={totalItems}
        setPageIndex={setPageIndex}
      />
    </div>
  );
};

export default StickerStyleTable;
