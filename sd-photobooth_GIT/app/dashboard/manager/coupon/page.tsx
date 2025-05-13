"use client";

import { columns } from "./columns";
import { toast } from "react-toastify";
import { useState } from "react";
import { useDebounce } from "@/hooks/useDebounce";
import { z, ZodType } from "zod";
import AxiosAPI from "@/configs/axios";
import { usePaginatedQuery } from "@/hooks/usePaginatedQuery";
import { Coupon } from "@/types/type";
import dynamic from "next/dynamic";
import { LoadingSkeleton } from "@/components/layouts/LoadingSkeleton";
import { couponSchema } from "@/types/schema/coupon";
import { deleteCoupon, getCouponList } from "@/services/coupon";
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

type CouponFormType = z.infer<typeof couponSchema>;

export default function CouponPage() {
  const [search, setSearch] = useState("");
  const debouncedSearch = useDebounce(search, 500);

  const [pageSize, setPageSize] = useState(5);
  const [pageIndex, setPageIndex] = useState(0);

  const { data, totalItems, isLoading, refetch } = usePaginatedQuery<Coupon>({
    queryKey: "coupons",
    pageIndex,
    pageSize,
    search: debouncedSearch,
    queryFn: async ({ page, size, search }) => {
      return await getCouponList(page, size, search);
    },
  });

  const handleDelete = async (id: number) => {
    try {
      await deleteCoupon(id);
      toast.success("Xóa thành côngO");
      if (data?.length === 1 && pageIndex > 0) setPageIndex((prev) => prev - 1);
      else refetch();
    } catch (error) {
      toast.error("Failed to delete coupon");
    }
  };

  return (
    <CrudPageWrapper
      title="Coupon Management"
      search={search}
      onSearchChange={(val) => {
        setSearch(val);
        setPageIndex(0);
      }}
      createButton={
        <CreateDialogForm
          title="Add Coupon"
          description="Create new coupon entry"
          triggerText=""
          triggerIcon={<PlusCircleIcon className="w-10 h-10" />}
          schema={couponSchema as ZodType<CouponFormType>}
          fields={[
            { name: "name", label: "Name", type: "text" },
            { name: "code", label: "Code", type: "text" },
            { name: "description", label: "Description", type: "text" },
            { name: "discount", label: "Discount", type: "number" },
            {
              name: "discountPercent",
              label: "Discount Percent(%)",
              type: "number",
            },
            { name: "maxUse", label: "Max Use", type: "number" },
            { name: "maxDiscount", label: "Max Discount", type: "number" },
            { name: "minOrder", label: "Min Order", type: "number" },
            { name: "startDate", label: "From", type: "date" },
            { name: "endDate", label: "To", type: "date" },
            { name: "isActive", label: "Is Active", type: "switch" },
          ]}
          onSubmit={async (values) => {
            await AxiosAPI.post("/api/Coupon", values);
            refetch();
          }}
        />
      }
      data={data ?? []}
      columns={columns(handleDelete, refetch)}
      isLoading={isLoading}
      pageCount={Math.ceil(totalItems / pageSize)}
      onPageChange={setPageIndex}
      pageIndex={pageIndex}
      pageSize={pageSize}
      onPageSizeChange={setPageSize}
    />
  );
}
