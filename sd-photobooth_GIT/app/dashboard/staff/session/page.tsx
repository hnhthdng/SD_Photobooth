"use client";

import { LoadingSkeleton } from "@/components/layouts/LoadingSkeleton";
import { columns } from "./columns";
import { useMemo, useState } from "react";
import dynamic from "next/dynamic";
import { Session } from "@/types/type";
import { usePaginatedQuery } from "@/hooks/usePaginatedQuery";
import { useDebounce } from "@/hooks/useDebounce";
import { getSession } from "@/services/session";

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

const SessionPage = () => {
  const [search, setSearch] = useState("");
  const debouncedSearch = useDebounce(search, 500);

  const [pageSize, setPageSize] = useState(5);
  const [pageIndex, setPageIndex] = useState(0);

  const { data, totalItems, isLoading, refetch } = usePaginatedQuery<Session>({
    queryKey: "sessions",
    pageIndex,
    pageSize,
    search: debouncedSearch,
    queryFn: async ({ page, size, search }) => {
      return await getSession(page, size, search);
    },
  });

  const memoizedColumns = useMemo(() => columns(), []);

  return (
    <CrudPageWrapper
      title="Session Management"
      isSearchable
      search={search}
      onSearchChange={setSearch}
      data={data}
      columns={memoizedColumns}
      isLoading={isLoading}
      pageCount={Math.ceil(totalItems / pageSize)}
      pageIndex={pageIndex}
      pageSize={pageSize}
      onPageChange={setPageIndex}
      onPageSizeChange={setPageSize}
    />
  );
};

export default SessionPage;
