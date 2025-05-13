"use client";

import { User } from "@/types/type";
import { ColumnDef } from "@tanstack/react-table";
const GenderCell = ({ value }: { value: number }) => (
  <div className="capitalize">{value === 0 ? "Nam" : value === 1 ? "Nữ" : "Không xác định"}</div>
);
export const columns = (): ColumnDef<User>[] => [
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
    accessorKey: "phoneNumber",
    header: () => <div className="text-center">Phone</div>,
    cell: ({ row }) => (
      <div className="text-center">{row.getValue("phoneNumber")}</div>
    ),
  },
  {
    accessorKey: "gender",
    header: () => <div className="text-center">Gender</div>,
    cell: ({ row }) => <GenderCell value={row.getValue("gender")} />,
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
