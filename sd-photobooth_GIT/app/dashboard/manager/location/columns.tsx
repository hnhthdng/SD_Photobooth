import { ColumnDef } from "@tanstack/react-table";
import { Location } from "@/types/type";
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
import ViewDetailLocation from "@/components/component/IDLocation";
import UpdateLocation from "@/components/component/GPLocation";
import { Group } from "@mantine/core";

const StatusCell = ({ value }: { value: boolean }) => (
  <div className="capitalize">{value ? "Active" : "Inactive"}</div>
);

const DateCell = ({ value }: { value: string }) => {
  const date = new Date(value);
  return <div>{date.toLocaleDateString()}</div>;
};

export const columns = (
  onDelete: (id: number) => Promise<void>,
  refetchData: () => void
): ColumnDef<Location>[] => [
  {
    accessorKey: "id",
    header: () => <div className="text-center">ID</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("id")}</div>,
  },
  {
    accessorKey: "locationName",
    header: () => <div className="text-center">Location Name</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("locationName")}</div>,
  },
  {
    accessorKey: "address",
    header: () => <div className="text-center">Address</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("address")}</div>,
  },
  {
    accessorKey: "createdAt",
    header: () => <div className="text-center">Created At</div>,
    cell: ({ row }) => <div className="text-center"><DateCell value={row.getValue("createdAt")} /></div>,
  },
  
  
  {
      id: "actions",
      header: () => <div className="text-center">Actions</div>,
      cell: ({ row }) => (
         
        <Group justify="center">
          <UpdateLocation id={row.original.id} onUpdateSuccess={refetchData} />
          <ViewDetailLocation id={row.original.id} />
          <DeletePayment id={row.original.id} onDelete={onDelete} />
        </Group>
      ),
    },

 
];
