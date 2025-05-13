'use client';

import { useEffect, useState } from "react";
import { useDebounce } from "@/hooks/useDebounce";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { PaginatedResponse } from "@/types/paginated-response";

interface PaginatedFetchParams {
  page: number;
  pageSize: number;
}



interface UsePaginatedTableProps<T> {
  queryKey: string;
  fetchDataFn: (params: PaginatedFetchParams) => Promise<PaginatedResponse<T>>;
  fetchCountFn?: () => Promise<number>;
  searchFn?: (searchTerm: string) => Promise<T[]>;
  initialPageSize?: number;
  withSearch?: boolean;
  debounceTime?: number;
}

export const usePaginatedTable = <T,>({
  queryKey,
  fetchDataFn,
  fetchCountFn,
  searchFn,
  initialPageSize = 5,
  withSearch = true,
  debounceTime = 300,
}: UsePaginatedTableProps<T>) => {
  const [searchTerm, setSearchTerm] = useState("");
  const debouncedSearchTerm = useDebounce(searchTerm, debounceTime);
  const [pageIndex, setPageIndex] = useState(0);
  const [pageSize, setPageSize] = useState(initialPageSize);

  const isSearching = withSearch && debouncedSearchTerm.trim() !== "" && searchFn;

  // Main data query
  const itemsQuery = useQuery({
    queryKey: [queryKey, "items", pageIndex, pageSize, debouncedSearchTerm],
    queryFn: async () => {
      // console.log('Fetching data...');
      if (isSearching) {
        const items = await searchFn(debouncedSearchTerm);
        return { items, totalItems: items.length };
      }
      return await fetchDataFn({
        page: pageIndex + 1, 
        pageSize,
      });
    },
    placeholderData:undefined,
    staleTime: 60_000,
  });

  
  const countQuery = useQuery({
    queryKey: [queryKey, "count"],
    queryFn: fetchCountFn!,
    enabled: !!fetchCountFn && !isSearching,
    staleTime: 60_000,
  });

  
  const totalItems = isSearching 
  ? itemsQuery.data?.totalItems ?? 0
  : countQuery.data ?? itemsQuery.data?.totalItems ?? 0;

  useEffect(() => {
    const totalPages = Math.ceil(totalItems / pageSize);
    if (pageIndex > 0 && pageIndex >= totalPages) {
      setPageIndex(totalPages - 1);
    }
  }, [totalItems, pageIndex, pageSize]);

  return {
    
    searchTerm,
    setSearchTerm,
    debouncedSearchTerm,
    isSearching,
    
   
    pageIndex,
    setPageIndex,
    pageSize,
    setPageSize,
    
   
    data: itemsQuery.data?.items ?? [],
    totalItems,
    
    
    isLoading: itemsQuery.isLoading || (!!fetchCountFn && countQuery.isLoading),
    error: itemsQuery.error || countQuery.error,
    
    
    refetch: () => {
      itemsQuery.refetch();
      if (fetchCountFn) countQuery.refetch();
    },
  };
};