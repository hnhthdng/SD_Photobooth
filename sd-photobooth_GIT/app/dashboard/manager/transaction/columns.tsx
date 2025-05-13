import { ColumnDef } from "@tanstack/react-table";
import { Transaction } from "@/types/type";

const DateCell = ({ value }: { value: string }) => {
  const date = new Date(value);
  return <div>{date.toLocaleDateString()}</div>;
};

export const columns = (): ColumnDef<Transaction>[] => [
  {
    accessorKey: "id",
    header: () => <div className="text-center">ID</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("id")}</div>,
  },
  {
    accessorKey: "paymentId",
    header: () => <div className="text-center">Payment ID</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("paymentId")}</div>,
  },
  {
    accessorKey: "paymentMethodName",
    header: () => <div className="text-center">Payment Method Name</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("paymentMethodName")}</div>,
  },
  {
    accessorKey: "type",
    header: () => <div className="text-center">Type</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("type")}</div>,
  },
  {
    accessorKey: "amount",
    header: () => <div className="text-center">Amount</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("amount")}</div>,
  },
  {
    accessorKey: "orderId",
    header: () => <div className="text-center">Order ID</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("orderId")}</div>,
  },
  {
    accessorKey: "depositId",
    header: () => <div className="text-center">Deposit ID</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("depositId")}</div>,
  },
  {
    accessorKey: "createdAt",
    header: () => <div className="text-center">Created At</div>,
    cell: ({ row }) => <div className="text-center"><DateCell value={row.getValue("createdAt")} /></div>,
  },
  
];
