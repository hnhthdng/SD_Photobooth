"use client";

import EditMembershipCard from "@/components/component/GPMembershipCard";
import ViewDetailMembershipCard from "@/components/component/IDMemberShipCard";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { MembershipCard } from "@/types/type";
import { ColumnDef } from "@tanstack/react-table";
import { MoreHorizontal } from "lucide-react";

export const columns = (
  refetchData: () => void
): ColumnDef<MembershipCard>[] => [
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
    accessorKey: "customer.fullName",
    header: () => <div className="text-center">Customer Name</div>,
    cell: ({ row }) => {
      const name = row.original.customer.fullName;
      return <div className="text-center">{name || "No Name"}</div>;
    },
  },
  {
    accessorKey: "customer.email",
    header: () => <div className="text-center">Customer Email</div>,
    cell: ({ row }) => {
      const email = row.original.customer.email;
      return <div className="text-center">{email || "No Email"}</div>;
    },
  },
  {
    accessorKey: "points",
    header: () => <div className="text-center">Points</div>,
    cell: ({ row }) => (
      <div className="text-center">{row.getValue("points")}</div>
    ),
  },

  {
    accessorKey: "levelMemberShip",
    header: () => <div className="text-center">Level Membership</div>,
    cell: ({ row }) => {
      const name = row.original.levelMemberShip.name;
      return <div className="text-center">{name || "No Name"}</div>;
    },
  },

  {
    accessorKey: "isActive",
    header: () => <div className="text-center">Is Active</div>,
    cell: ({ row }) => (
      <div className="text-center">
        {row.getValue("isActive") ? "Yes" : "No"}
      </div>
    ),
  },
  {
    id: "edit",
    header: () => <div className="text-center">Edit</div>,
    cell: ({ row }) => {
      const id = row.original.id;
      return <EditMembershipCard id={id} onUpdated={refetchData} />;
    },
  },

  {
    id: "actions",
    enableHiding: false,
    cell: ({ row }) => {
      const id = row.original.id;
      const sticker = row.original;
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
            {/* <DropdownMenuItem asChild>
                                        <DeletePayment id={id} onDelete={onDelete} />
                                   </DropdownMenuItem>

                                   */}

            <DropdownMenuItem asChild>
              <ViewDetailMembershipCard id={id} />
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      );
    },
  },
];
