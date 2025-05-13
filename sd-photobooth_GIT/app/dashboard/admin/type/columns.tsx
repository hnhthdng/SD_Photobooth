"use client";

import DeletePayment from "@/components/component/DeletePayment";

import EditTypeSession from "@/components/component/GPTypeSession";
import ViewDetailTypeSession from "@/components/component/IDTypeSession";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { TypeSession } from "@/types/type";
import { ColumnDef } from "@tanstack/react-table";
import { MoreHorizontal } from "lucide-react";

const DateCell = ({ value }: { value: string }) => {
  const date = new Date(value);
  return <div>{date.toLocaleDateString()}</div>;
};

const PriceCell = ({ value }: { value: number }) => {
  const formattedValue = new Intl.NumberFormat("vi-VN", {
    style: "currency",
    currency: "VND",
    minimumFractionDigits: 0,
  }).format(value || 0);

  return <div>{formattedValue}</div>;
};

export const columns = (
  onDelete: (id: number) => Promise<void>,
  refetchData: () => void
): ColumnDef<TypeSession>[] => [
    {
      accessorKey: "id",
      header: () => <div className="text-center">ID</div>,
      cell: ({ row }) => <div className="text-center">{row.getValue("id")}</div>,
    },
    {
      accessorKey: "name",
      header: () => <div className="text-center">Name</div>,
      cell: ({ row }) => (
        <div className="text-center">{row.getValue("name")}</div>
      ),
    },
    {
      accessorKey: "description",
      header: () => <div className="text-center">Description</div>,
      cell: ({ row }) => (
        <div className="text-center">{row.getValue("description")}</div>
      ),
    },
    {
      accessorKey: "duration",
      header: () => <div className="text-center">Duration</div>,
      cell: ({ row }) => (
        <div className="text-center">{row.getValue("duration")}</div>
      ),
    },
    {
      accessorKey: "price",
      header: () => <div className="text-center">Price</div>,
      cell: ({ row }) => (
        <div className="text-center">
          <PriceCell value={row.getValue("price")} />
        </div>
      ),
    },
    {
      accessorKey: "ableTakenNumber",
      header: () => <div className="text-center">Able Taken Number</div>,
      cell: ({ row }) => (
        <div className="text-center">{row.getValue("ableTakenNumber")}</div>
      ),
    },
    {
      accessorKey: "createdAt",
      header: () => <div className="text-center">Created At</div>,
      cell: ({ row }) => <DateCell value={row.getValue("createdAt")} />,
    },

    {
      id: "edit",
      header: () => <div className="text-center">Edit</div>,
      cell: ({ row }) => {
        const id = row.original.id;
        return <EditTypeSession id={id} onUpdateSuccess={refetchData} />;
      },
    },
    {
      id: "actions",
      enableHiding: false,
      cell: ({ row }) => {
        const id = row.original.id;

        return (
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button variant="ghost" className="h-8 w-8 p-0">
                <MoreHorizontal />
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align="center">
              <DropdownMenuLabel>Actions</DropdownMenuLabel>

              <DropdownMenuSeparator />
              <DropdownMenuItem asChild>
                <DeletePayment id={id} onDelete={onDelete} />
              </DropdownMenuItem>

              <DropdownMenuItem asChild>
                <ViewDetailTypeSession id={id} />
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
        );
      },
    },
  ];
