"use client";

import {
     Select,
     SelectContent,
     SelectItem,
     SelectTrigger,
     SelectValue,
} from "@/components/ui/select";

interface PageSizeSelectorProps {
     value: number;
     onChange: (value: number) => void;
     options?: number[];
     placeholder?: string;
}

export const PageSizeSelector = ({
     value,
     onChange,
     options = [5, 10, 15, 20],
     placeholder = "Rows per page",
}: PageSizeSelectorProps) => {
     return (
          <div className="mb-3 flex justify-end">
               <Select
                    value={value.toString()}
                    onValueChange={(val) => onChange(Number(val))}
               >
                    <SelectTrigger className="w-[100px] h-[36px] rounded-md border border-gray-200 bg-white px-3 py-1.5 text-sm text-gray-900 font-normal transition-colors duration-150 hover:border-gray-300 focus:border-gray-400 focus:outline-none focus:ring-1 focus:ring-gray-300">
                         <SelectValue placeholder={placeholder} />
                    </SelectTrigger>
                    <SelectContent className="rounded-md border border-gray-200 bg-white shadow-sm">
                         {options.map((option) => (
                              <SelectItem
                                   key={option}
                                   value={option.toString()}
                                   className="cursor-pointer px-3 py-1.5 text-sm text-gray-800 hover:bg-gray-100 focus:bg-gray-100"
                              >
                                   {option}
                              </SelectItem>
                         ))}
                    </SelectContent>
               </Select>
          </div>

     );
};