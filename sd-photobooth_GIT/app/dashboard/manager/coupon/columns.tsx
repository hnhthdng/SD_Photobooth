import { ColumnDef } from "@tanstack/react-table";
import { Coupon } from "@/types/type";
import { MoreHorizontal } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import DeletePayment from "@/components/component/DeletePayment";
import ViewDetailCoupon from "@/components/component/IDCoupon";
import UpdateCoupon from "@/components/component/GPCoupon";
import { Group } from "@mantine/core";
const StatusCell = ({ value }: { value: boolean }) => {
  const text = value ? "Active" : "Inactive";
  const colorClass = value ? "text-green-600" : "text-gray-400";

  return <div className={`capitalize font-medium ${colorClass}`}>{text}</div>;
};

const DateCell = ({ value }: { value: string }) => {
  const date = new Date(value);
  return <div>{date.toLocaleDateString()}</div>;
};

export const columns = (
  onDelete: (id: number) => Promise<void>,
  refetchData: () => void
): ColumnDef<Coupon>[] => [
  {
    accessorKey: "id",
    header: () => <div className="text-center">ID</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("id")}</div>,
  },
  {
    accessorKey: "name",
    header: () => <div className="text-center">Name</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("name")}</div>,
  },
  {
    accessorKey: "code",
    header: () => <div className="text-center">Code</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("code")}</div>,
  },
  {
    accessorKey: "discount",
    header: () => <div className="text-center">Discount</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("discount")}</div>,
  },
  {
    accessorKey: "startDate",
    header: () => <div className="text-center">Start Date</div>,
    cell: ({ row }) => <div className="text-center"><DateCell value={row.getValue("startDate")} /></div> ,
  },
  {
    accessorKey: "endDate",
    header: () => <div className="text-center">End Date</div>,
    cell: ({ row }) => <div className="text-center"><DateCell value={row.getValue("endDate")} /></div>,
  },
  {
    accessorKey: "isActive",
    header: () => <div className="text-center">Is Active</div>,
    cell: ({ row }) => <div className="text-center"><StatusCell value={row.getValue("isActive")} /></div>,
  },
  {
    accessorKey: "createdAt",
    header: () => <div className="text-center">Created At</div>,
    cell: ({ row }) => <div className="text-center"><DateCell value={row.getValue("createdAt")} /></div> ,
  },
  
  

  {
        id: "actions",
        header: () => <div className="text-center">Actions</div>,
        cell: ({ row }) => (
           
          <Group justify="center">
            <UpdateCoupon couponId={row.original.id} onUpdateSuccess={refetchData} />
            <ViewDetailCoupon id={row.original.id} />
            <DeletePayment id={row.original.id} onDelete={onDelete} />
          </Group>
        ),
      },
  
];
