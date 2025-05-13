"use client";

import { deleteBooth, getBoothList } from "@/services/booth";
import { columns } from "./columns";
import { toast } from "react-toastify";
import { useEffect, useState } from "react";
import { useDebounce } from "@/hooks/useDebounce";
import { z } from "zod";
import AxiosAPI from "@/configs/axios";
import { usePaginatedQuery } from "@/hooks/usePaginatedQuery";
import { Booth } from "@/types/booth";
import { Location } from "@/types/type";
import dynamic from "next/dynamic";
import { LoadingSkeleton } from "@/components/layouts/LoadingSkeleton";
import { getAllLocations } from "@/services/location";
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

const boothSchema = z.object({
  boothName: z.string().min(1, "Required"),
  locationId: z.string().min(1, "Required"),
  description: z.string().optional(),
  status: z.boolean().default(true),
});

export default function BoothPage() {
  const [search, setSearch] = useState("");
  const debouncedSearch = useDebounce(search, 500);

  const [pageSize, setPageSize] = useState(5);
  const [pageIndex, setPageIndex] = useState(0);
  const [locations, setLocations] = useState<Location[]>([]);

  useEffect(() => {
    const fetchLocations = async () => {
      try {
        const res = await getAllLocations();
        const validLocations = res.filter(
          (l): l is Location => !!l?.locationName && !!l?.id
        );
        setLocations(validLocations);
      } catch (error) {
        console.error("Failed to fetch locations", error);
        toast.error("Failed to load locations");
      }
    };

    fetchLocations();
  }, []);

  const { data, totalItems, isLoading, refetch } = usePaginatedQuery<Booth>({
    queryKey: "booths",
    pageIndex,
    pageSize,
    search: debouncedSearch,
    queryFn: async ({ page, size, search }) => {
      return getBoothList(page, size, search);
    },
  });

  const handleDelete = async (id: number) => {
    try {
      await deleteBooth(id);
      toast.success("Xóa thành công");
      if (data?.length === 1 && pageIndex > 0) setPageIndex((prev) => prev - 1);
      else refetch();
    } catch {
      toast.error("Failed to delete booth");
    }
  };

  return (
    <CrudPageWrapper
      title="Booth Management"
      search={search}
      onSearchChange={setSearch}
      createButton={
        <CreateDialogForm
          title="Add Booth"
          description="Create new booth entry"
          triggerText=""
          triggerIcon={<PlusCircleIcon className="w-10 h-10" />}
          schema={boothSchema}
          onSubmit={async (values) => {
            await AxiosAPI.post("/api/Booth", {
              ...values,
              locationId: Number(values.locationId),
            });
            // toast.success("Thêm thành công! ");
            refetch();
          }}
          fields={[
            { type: "text", name: "boothName", label: "Booth Name" },
            {
              type: "select",
              name: "locationId",
              label: "Location",
              options: locations
                .filter((l) => l && l.locationName)
                .map((l) => ({
                  label: l.locationName,
                  value: l.id,
                })),
            },
            { type: "text", name: "description", label: "Description" },
            {
              type: "switch",
              name: "status",
              label: "Status",
              description: "Enable or disable this booth",
            },
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
