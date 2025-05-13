"use client";


import { useState } from "react";
import { usePaginatedQuery } from "@/hooks/usePaginatedQuery";
import { Transaction } from "@/types/type";
import { columns } from "./columns";
import dynamic from "next/dynamic";
import { LoadingSkeleton } from "@/components/layouts/LoadingSkeleton";
import { getTransaction } from "@/services/transaction";

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

export default function TransactionPage() {


  const [pageSize, setPageSize] = useState(5);
  const [pageIndex, setPageIndex] = useState(0);

  const { data, totalItems, isLoading } = usePaginatedQuery<Transaction>({
    queryKey: "transactions",
    pageIndex,
    pageSize,

    queryFn: async ({ page, size }) => {
      return await getTransaction(page, size);
    },
  });

  return (
    <CrudPageWrapper
      isSearchable={false}
      title="Transaction Management"

      createButton={null}
      data={data}
      columns={columns()}
      isLoading={isLoading}
      pageCount={Math.ceil(totalItems / pageSize)}
      pageIndex={pageIndex}
      pageSize={pageSize}
      onPageChange={setPageIndex}
      onPageSizeChange={setPageSize}
    />
  );
}
