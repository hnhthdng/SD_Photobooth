"use client";

import { columns } from "./columns";
import { useState } from "react";
import { z } from "zod";
import AxiosAPI from "@/configs/axios";
import { usePaginatedQuery } from "@/hooks/usePaginatedQuery";
import { Booth } from "@/types/booth";
import dynamic from "next/dynamic";
import { LoadingSkeleton } from "@/components/layouts/LoadingSkeleton";
import { getLevelMembershipCards } from "@/services/level-membership-card";
import { IconPlus } from "@tabler/icons-react";

const GPUpgradeLevel = dynamic(
  () => import("@/components/component/GPUpgrade"),
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

export default function MembershipCardPage() {
  const [pageSize, setPageSize] = useState(5);
  const [pageIndex, setPageIndex] = useState(0);

  const { data, totalItems, isLoading, refetch } = usePaginatedQuery<Booth>({
    queryKey: "Membership Cards",
    pageIndex,
    pageSize,
    queryFn: async ({ page, size, search }) => {
      return getLevelMembershipCards(page, size, search);
    },
  });

  return (

    <>
    <GPUpgradeLevel onUpdateSuccess={refetch} />
    <CrudPageWrapper
      title="Membership Card Management"
      isSearchable={false}
      
      data={data}
      columns={columns(refetch)}
      isLoading={isLoading}
      pageCount={Math.ceil(totalItems / pageSize)}
      pageIndex={pageIndex}
      pageSize={pageSize}
      onPageChange={setPageIndex}
      onPageSizeChange={setPageSize}
    />
    </>
  );
}
