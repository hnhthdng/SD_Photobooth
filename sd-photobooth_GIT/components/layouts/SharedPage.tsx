import React, { JSX, ReactNode } from "react";
import { SharedTable } from "@/components/layouts/SharedTable";
import { SearchInput } from "./SearchInput";
import { PageSizeSelector } from "./PageSizeSelector";


export interface CrudPageWrapperProps<T> {
  title?: string;
  isSearchable?: boolean;
  search?: string;
  onSearchChange?: (value: string) => void;
  createButton?: ReactNode;
  data: T[];
  columns: any;
  isLoading: boolean;
  pageCount: number;
  pageIndex: number;
  pageSize: number;
  rightSlot?: ReactNode;
  onPageChange: (index: number) => void;
  onPageSizeChange: (size: number) => void;
}

function CrudPageWrapper<T>({
  title,
  isSearchable = true,
  search,
  onSearchChange,
  createButton,
  data,
  columns,
  isLoading,
  pageCount,
  pageIndex,
  pageSize,
  rightSlot,
  onPageChange,
  onPageSizeChange,
}: CrudPageWrapperProps<T>) {
  return (
    <div className="space-y-4">
      {title && <h2 className="text-2xl font-semibold">{title}</h2>}
      <div className="flex items-center justify-between gap-2">
        {isSearchable && (
          <SearchInput
            value={search ?? ""}
            onChange={onSearchChange ?? (() => {})}
            placeholder="Search..."
          />
        )}

        <div className="flex items-center  gap-2">
          {rightSlot}
          {createButton}
        </div>
      </div>

      <PageSizeSelector value={pageSize} onChange={onPageSizeChange} />
      <SharedTable
        data={data}
        columns={columns}
        isLoading={isLoading}
        pageCount={pageCount}
        pageIndex={pageIndex}
        pageSize={pageSize}
        onPageChange={onPageChange}
        onPageSizeChange={onPageSizeChange}
        
      />
    </div>
  );
}

export const MemoizedCrudPage = React.memo(CrudPageWrapper) as <T>(
  props: CrudPageWrapperProps<T>
) => JSX.Element;
