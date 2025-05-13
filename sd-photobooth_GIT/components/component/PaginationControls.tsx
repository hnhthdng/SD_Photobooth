"use client";

import { Button } from "@/components/ui/button";
import { MdNavigateBefore, MdNavigateNext } from "react-icons/md";
interface PaginationControlProps {
  pageIndex: number;
  pageSize: number;
  totalItems: number;
  setPageIndex: (page: number) => void;
}

export const PaginationControl = ({
  pageIndex,
  pageSize,
  totalItems,
  setPageIndex,
}: PaginationControlProps) => {
  const totalPages = Math.max(1, Math.ceil(totalItems / pageSize));

  const handlePrev = () => {
    if (pageIndex > 0) {
      setPageIndex(pageIndex - 1);
    }
  };

  const handleNext = () => {
    if (pageIndex + 1 < totalPages) {
      setPageIndex(pageIndex + 1);
    }
  };

  return (
    <div className="flex justify-center items-center mt-4 space-x-4">
      <Button
        variant="outline"
        onClick={handlePrev}
        disabled={pageIndex === 0}
        className="h-10 w-10 rounded-full p-0 flex items-center justify-center dark:border-gray-600"
      >
        <MdNavigateBefore size={20} className="text-gray-800 dark:text-gray-200" />
      </Button>

      <span className="px-4 py-2 text-sm rounded-full border border-gray-300 dark:border-gray-600 bg-white dark:bg-[#2a2a2a] text-gray-800 dark:text-gray-200 shadow-sm dark:shadow-none">
        {Math.min(pageIndex + 1, totalPages)} of {totalPages}
      </span>

      <Button
        variant="outline"
        onClick={handleNext}
        disabled={pageIndex + 1 >= totalPages}
        className="h-10 w-10 rounded-full p-0 flex items-center justify-center dark:border-gray-600"
      >
        <MdNavigateNext size={20} className="text-gray-800 dark:text-gray-200" />
      </Button>
    </div>


  );
};
