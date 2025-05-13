import { PaginatedResponse } from "@/types/paginated-response";
import { useQuery } from "@tanstack/react-query";

export interface UsePaginatedQueryOptions<T> {
  queryKey: string;
  pageIndex: number;
  pageSize: number;
  search?: string;
  queryFn: (params: {
    page: number;
    size: number;
    search?: string;
  }) => Promise<PaginatedResponse<T>>;
}

export function usePaginatedQuery<T>({
  queryKey,
  pageIndex,
  pageSize,
  search,
  queryFn,
}: UsePaginatedQueryOptions<T>) {
  const query = useQuery({
    queryKey: [queryKey, pageIndex, pageSize, search],
    queryFn: () =>
      queryFn({
        page: pageIndex + 1,
        size: pageSize,
        search,
      }),
  });

  return {
    data: query.data?.items ?? [],
    totalItems: query.data?.totalItems ?? 0,
    isLoading: query.isLoading,
    error: query.error,
    refetch: query.refetch,
  };
}
