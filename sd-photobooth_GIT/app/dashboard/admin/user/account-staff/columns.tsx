"use client";

import { User } from "@/types/type";
import { ColumnDef } from "@tanstack/react-table";

export const columns = (): // onDelete: (id: number) => Promise<void>,
// refetchData: () => void,
ColumnDef<User>[] => [
  {
    accessorKey: "id",
    header: () => <div className="text-center">ID</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("id")}</div>,
  },
  {
    accessorKey: "fullName",
    header: () => <div className="text-center">Full Name</div>,
    cell: ({ row }) => (
      <div className="text-center">{row.getValue("fullName")}</div>
    ),
  },
  {
    accessorKey: "userName",
    header: () => <div className="text-center">User Name</div>,
    cell: ({ row }) => (
      <div className="text-center">{row.getValue("userName")}</div>
    ),
  },
  {
    accessorKey: "email",
    header: () => <div className="text-center">Email</div>,
    cell: ({ row }) => (
      <div className="text-center">{row.getValue("email")}</div>
    ),
  },
  {
    accessorFn: (row) => row.location?.locationName,
    id: "locationName",
    header: () => <div className="text-center">Location</div>,
    cell: ({ row }) => (
      <div className="text-center">{row.getValue("locationName")}</div>
    ),
  },
];
