import { Skeleton } from "@/components/ui/skeleton";

interface SkeletonTableProps {
  columns: number;
  rows: number;
}

const SkeletonTable = ({ columns, rows }: SkeletonTableProps) => {
  return (
    <div className="overflow-x-auto">
      <table className="w-full table-auto border-collapse border border-gray-300 rounded-md shadow-sm">
        <thead className="bg-gray-100">
          <tr>
            {Array.from({ length: columns }).map((_, index) => (
              <th
                key={index}
                className="border border-gray-300 px-4 py-3 text-left"
              >
                <Skeleton className="h-4 w-24" />
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {Array.from({ length: rows }).map((_, rowIndex) => (
            <tr key={rowIndex}>
              {Array.from({ length: columns }).map((_, colIndex) => (
                <td
                  key={colIndex}
                  className="border border-gray-300 px-4 py-3"
                >
                  <Skeleton className="h-4 w-full" />
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default SkeletonTable;
