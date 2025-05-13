"use client";

import { User } from "@/types/type";
import { ColumnDef } from "@tanstack/react-table";

const StatusCell = ({ value }: { value: boolean }) => (
  <div className="capitalize">{value ? "Active" : "Inactive"}</div>
);
const GenderCell = ({ value }: { value: number }) => (
  <div className="capitalize">{value === 0 ? "Nam" : value === 1 ? "Nữ" : "Không xác định"}</div>
);
const DateCell = ({ value }: { value: string | null }) => {
  if (!value) return <div>-</div>;

  const date = new Date(value);
  return <div>{date.toLocaleDateString()}</div>;
};
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
    accessorKey: "gender",
    header: () => <div className="text-center">Gender</div>,
    cell: ({ row }) => <GenderCell value={row.getValue("gender")} />,
  },
  {
    accessorKey: "birthDate",
    header: () => <div className="text-center">Birth Date</div>,
    cell: ({ row }) => <DateCell value={row.getValue("birthDate")} />,
  },
  {
    id: "membershipLevel", 
    accessorFn: (row) => row.membershipCard?.currentLevel ?? "N/A",  
    header: () => <div className="text-center">Membership Level</div>,
    cell: ({ getValue }) => (
      <div className="text-center">{String(getValue())}</div>  
    ),
  },
  {
    accessorKey: "isBanned",
    header: () => <div className="text-center">Banned</div>,
    cell: ({ row }) => <StatusCell value={row.getValue("isBanned")} />,
  },
];
