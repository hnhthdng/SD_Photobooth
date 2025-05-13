import { ColumnDef } from "@tanstack/react-table";
import { Payment } from "@/types/type";
import { PaymentStatus, PaymentStatusMeta } from "@/types/enum/payment-status";

const DateCell = ({ value }: { value: string }) => {
  const date = new Date(value);
  return <div>{date.toLocaleDateString()}</div>;
};

export const columns = (): ColumnDef<Payment>[] => [
  {
    accessorKey: "id",
    header: () => <div className="text-center">ID</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("id")}</div>,
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
    accessorKey: "paymentMethodName",
    header: () => <div className="text-center">Payment Method</div>,
    cell: ({ row }) => <div className="text-center">{row.getValue("paymentMethodName")}</div>,
  },
  {
    accessorKey: "status",
    header: () => <div className="text-center">Status</div>,
    cell: ({ row }) => {
      const statusValue = row.getValue("status") as PaymentStatus;
      const meta = PaymentStatusMeta[statusValue];

      if (!meta) return <div className="text-center text-gray-500">Unknown</div>;

      const Icon = meta.icon;

      return (
        <div className="flex justify-center items-center">
          <div className={`flex items-center gap-2 text-center ${meta.colorClass}`}>
            <Icon />
            <span className="">{meta.label}</span>
          </div>
        </div>
      );
    },
  },
  {
    accessorKey: "createdAt",
    header: () => <div className="text-center">Created At</div>,
    cell: ({ row }) => <div className="text-center"><DateCell value={row.getValue("createdAt")} /></div>,
  },

];
