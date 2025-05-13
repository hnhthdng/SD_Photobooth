"use client";

import { toast } from "react-toastify";
import { useState } from "react";
import { useDebounce } from "@/hooks/useDebounce";
import { z } from "zod";
import AxiosAPI from "@/configs/axios";
import { usePaginatedQuery } from "@/hooks/usePaginatedQuery";
import { columns } from "./columns";
import { Location } from "@/types/type";
import dynamic from "next/dynamic";
import { LoadingSkeleton } from "@/components/layouts/LoadingSkeleton";
import { deleteLocation, getLocationList } from "@/services/location";
import { PlusCircleIcon } from "lucide-react";

const CreateDialogForm = dynamic(
  () =>
    import("@/components/layouts/CreateDialog").then(
      (mod) => mod.MemoizedCreateDialogForm
    ),
  {
    loading: () => <LoadingSkeleton />,
    ssr: false,
  }
);

const CrudPageWrapper = dynamic(
  () =>
    import("@/components/layouts/SharedPage").then(
      (mod) => mod.MemoizedCrudPage
    ),
  {
    loading: () => <LoadingSkeleton />,
    ssr: false,
  }
);

const locationSchema = z.object({
  locationName: z.string().min(1, "Required"),
  address: z.string().min(1, "Required"),
});

export default function LocationPage() {
  const [search, setSearch] = useState("");
  const debouncedSearch = useDebounce(search, 500);

  const [pageSize, setPageSize] = useState(5);
  const [pageIndex, setPageIndex] = useState(0);

  const { data, totalItems, isLoading, refetch } = usePaginatedQuery<Location>({
    queryKey: "locations",
    pageIndex,
    pageSize,
    search: debouncedSearch,
    queryFn: async ({ page, size, search }) => {
      return await getLocationList(page, size, search);
    },
  });

  const handleDelete = async (id: number) => {
    try {
      await deleteLocation(id);
      toast.success("Xóa thành công");
      if (data?.length === 1 && pageIndex > 0) setPageIndex((prev) => prev - 1);
      else refetch();
    } catch {
      toast.error("Failed to delete location");
    }
  };

  return (
    <CrudPageWrapper
      title="Location Management"
      search={search}
      onSearchChange={setSearch}
      createButton={
        <CreateDialogForm
          title="Add Location "
          description="Create a new location"
          triggerText=""
          triggerIcon={<PlusCircleIcon className="w-10 h-10" />}
          schema={locationSchema}
          onSubmit={async (values) => {
            await AxiosAPI.post("/api/Location", values);
            refetch();
          }}
          fields={[
            { type: "text", name: "locationName", label: "Location Name" },
            { type: "text", name: "address", label: "Address" },
          ]}
        />
      }
      data={data}
      columns={columns(handleDelete, refetch)}
      isLoading={isLoading}
      pageCount={Math.ceil(totalItems / pageSize)}
      pageIndex={pageIndex}
      pageSize={pageSize}
      onPageChange={setPageIndex}
      onPageSizeChange={setPageSize}
    />
  );
}
