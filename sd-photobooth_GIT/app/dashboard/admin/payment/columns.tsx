import DeletePayment from "@/components/component/DeletePayment";
import EditPaymentMethod from "@/components/component/GPPayment";
import ViewDetail from "@/components/component/IDPayment";
import { Button } from "@/components/ui/button";

import { PaymentMethod } from "@/types/type";
import { ColumnDef } from "@tanstack/react-table";


const DateCell = ({ value }: { value: string }) => {
     const date = new Date(value);
     return <div>{date.toLocaleDateString()}</div>;
};



export const columns: ColumnDef<PaymentMethod>[] = [
     {
          accessorKey: "id",
          header: () => <div className="text-center">ID</div>,
          cell: ({ row }) => <div className="text-center">{row.getValue("id")}</div>,
     },
     {
          accessorKey: "methodName",
          header: () => <div className="text-center">Method Name</div>,
          cell: ({ row }) => (
               <div className="text-center">{row.getValue("methodName")}</div>
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
          accessorKey: "isActive",
          header: () => <div className="text-center">Is Active</div>,
          cell: ({ row }) => (
               <div className="text-center">
                    {row.getValue("isActive") ? "Yes" : "No"}
               </div>
          ),
     },
     {
          accessorKey: "isOnline",
          header: () => <div className="text-center">Is Online</div>,
          cell: ({ row }) => (
               <div className="text-center">
                    {row.getValue("isOnline") ? "Yes" : "No"}
               </div>
          ),
     },
     {
          accessorKey: "forMobile",
          header: () => <div className="text-center">For Mobile</div>,
          cell: ({ row }) => (
               <div className="text-center">
                    {row.getValue("forMobile") ? "Yes" : "No"}
               </div>
          ),
     },
     {
          accessorKey: "createdAt",
          header: () => <div className="text-center">Created At</div>,
          cell: ({ row }) => (
               <div className="text-center">
                    <DateCell value={row.getValue("createdAt")} />
               </div>
          ),
     },
     {
          id: "edit",
          header: () => <div className="text-center">Edit</div>,
          cell: ({ row }) => {
               const id = row.original.id;
               return (
                    <>
                         {/* <ViewDetail id={id} /> */}
                    </>
               );
          },
     },
];




