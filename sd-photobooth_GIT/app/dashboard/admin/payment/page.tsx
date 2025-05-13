'use client';

import { columns } from "./columns";
import { GenericTable } from "@/components/component/GenericTable";
import { usePaginatedTable } from "@/hooks/useTablePagination";
import {
  getPaymentMethodCount,
  getPaymentMethods,
  searchPaymentMethodsByName,
} from "@/services/payment-method";
import { SearchInput } from "@/components/component/SearchInput";
import { PaginationControl } from "@/components/component/PaginationControls";
import SkeletonNew from "@/components/component/SkeletonNew";
import { PaymentMethod } from "@/types/type";
import { PageSizeSelector } from "@/components/component/PageSizeSelector";

const PaymentMethodTable = () => {
  const {
    data,
    isLoading,
    error,
    pageIndex,
    setPageIndex,
    pageSize,
    setPageSize,
    totalItems,
    searchTerm,
    setSearchTerm,
  } = usePaginatedTable<PaymentMethod>({
    queryKey: "paymentMethods",
    fetchDataFn: getPaymentMethods,
    fetchCountFn: getPaymentMethodCount,
    searchFn: searchPaymentMethodsByName,
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
      <SearchInput searchTerm={searchTerm} setSearchTerm={setSearchTerm} />
      <PageSizeSelector
        value={pageSize}
        onChange={handlePageSizeChange}
      />
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

export default PaymentMethodTable;
