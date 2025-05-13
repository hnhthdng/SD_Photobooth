import { ColumnDef } from "@tanstack/react-table";
import { DepositProduct } from "@/types/type";
import ViewDetailDepositProduct from "@/components/component/IDDepositProduct";
// import DeleteDepositProduct from "@/components/component/DeleteDepositProduct";
import { Group } from "@mantine/core";
import DeletePayment from "@/components/component/DeletePayment";

const DateCell = ({ value }: { value: string }) => {
  const date = new Date(value);
  return <div>{date.toLocaleDateString()}</div>;
};

export const columns = (
  onDelete: (id: number) => Promise<void>
): ColumnDef<DepositProduct>[] => [
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
    accessorKey: "description",
    header: () => <div className="text-center">Description</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("description")}</div>,
  },
  {
    accessorKey: "productId",
    header: () => <div className="text-center">Product ID</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("productId")}</div>,
  },
  {
    accessorKey: "amountAdd",
    header: () => <div className="text-center">Amount Add</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("amountAdd")}</div>,
  },
  {
    accessorKey: "price",
    header: () => <div className="text-center">Price</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("price")}</div>,
  },
  {
    accessorKey: "createdAt",
    header: () => <div className="text-center">Created At</div>,
    cell: ({ row }) => <DateCell value={row.getValue("createdAt")} />,
  },
  
  {
    id: "actions",
    header: () => <div className="text-center">Actions</div>,
    cell: ({ row }) => (
      <Group justify="center">
        <ViewDetailDepositProduct id={row.original.id} />
        <DeletePayment id={row.original.id} onDelete={onDelete} />
      </Group>
    ),
  },
];
