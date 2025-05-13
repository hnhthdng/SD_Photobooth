import { ColumnDef } from "@tanstack/react-table";
import { TypeSessionProduct } from "@/types/type";
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
import ViewDetailTypeSessionProduct from "@/components/component/IDTypeSessionProduct";
import UpdateTypeSessionProduct from "@/components/component/GPTypeSessionProduct";
import { Group } from "@mantine/core";

const DateCell = ({ value }: { value: string }) => {
  const date = new Date(value);
  return <div>{date.toLocaleDateString()}</div>;
};

export const columns = (
  onDelete: (id: number) => Promise<void>,
  refetchData: () => void
): ColumnDef<TypeSessionProduct>[] => [
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
      accessorKey: "productId",
      header: () => <div className="text-center">Product</div>,
      cell: ({ row }) => <div className="text-center">{row.getValue("productId")}</div>,
    },
    {
      accessorKey: "levelMembershipId",
      header: () => <div className="text-center">Level Membership ID</div>,
      cell: ({ row }) => <div className="text-center">{row.getValue("levelMembershipId")}</div>,
    },
    {
      accessorKey: "typeSessionId",
      header: () => <div className="text-center">TypeSession ID</div>,
      cell: ({ row }) => <div className="text-center">{row.getValue("typeSessionId")}</div>,
    },
    {
      accessorKey: "couponId",
      header: () => <div className="text-center">Coupon ID</div>,
      cell: ({ row }) => <div className="text-center">{row.getValue("couponId")}</div>,
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

        <Group justify="center" gap={"xs"}>
          <UpdateTypeSessionProduct id={row.original.id} onUpdateSuccess={refetchData} />;
          <ViewDetailTypeSessionProduct id={row.original.id} />
          <DeletePayment id={row.original.id} onDelete={onDelete} />
        </Group>
      ),
    },

  ];
