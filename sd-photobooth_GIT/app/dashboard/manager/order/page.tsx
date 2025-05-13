"use client";

import { columns } from "./columns";
import { toast } from "react-toastify";
import { useEffect, useState } from "react";
import { z } from "zod";
import AxiosAPI from "@/configs/axios";
import { usePaginatedQuery } from "@/hooks/usePaginatedQuery";
import { Order, PaymentMethod, TypeSession } from "@/types/type";
import { deleteOrder, getOrderList } from "@/services/order";
import PaymentDialog from "@/components/layouts/PaymentDialog";
import dynamic from "next/dynamic";
import { LoadingSkeleton } from "@/components/layouts/LoadingSkeleton";
import { Button } from "@/components/ui/button";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";
import { PlusCircleIcon, Terminal } from "lucide-react";
import CashOrderDialog from "@/components/component/CashOrderDialog";

const CreateDialogForm = dynamic(
  () =>
    import("@/components/layouts/CreateDialog").then(
      (mod) => mod.MemoizedCreateDialogForm
    ),
  {
    loading: () => <LoadingSkeleton />,
    ssr: false,
  }
);

const CrudPageWrapper = dynamic(
  () =>
    import("@/components/layouts/SharedPage").then(
      (mod) => mod.MemoizedCrudPage
    ),
  {
    loading: () => <LoadingSkeleton />,
    ssr: false,
  }
);

const orderSchema = z.object({
  email: z.string().email("Invalid email address").min(1, "Email is required"),
  phone: z.string().min(1, "Phone number is required"),
  typeSessionId: z.coerce.number().min(0, "Session type is required"),
  paymentMethodId: z.coerce.number().min(1, "Payment method is required"),
  couponCode: z.string().optional(),
});

export default function OrderPage() {
  const [pageSize, setPageSize] = useState(5);
  const [pageIndex, setPageIndex] = useState(0);
  const [typeSessions, setTypeSessions] = useState<TypeSession[]>([]);
  const [paymentMethods, setPaymentMethods] = useState<PaymentMethod[]>([]);
  const [paymentLink, setPaymentLink] = useState<string | null>(null);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [cashOrderInfo, setCashOrderInfo] = useState<any | null>(null);
  const [isCashDialogOpen, setIsCashDialogOpen] = useState(false);
  useEffect(() => {
    const fetchData = async () => {
      try {
        const [typeSessionRes, paymentMethodRes] = await Promise.all([
          AxiosAPI.get("/api/TypeSession"),
          AxiosAPI.get("/api/PaymentMethod/all/web"),
        ]);
        setTypeSessions(typeSessionRes.data as TypeSession[]);
        setPaymentMethods(paymentMethodRes.data as PaymentMethod[]);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };
    fetchData();
  }, []);

  const { data, totalItems, isLoading, refetch } = usePaginatedQuery<Order>({
    queryKey: "orders",
    pageIndex,
    pageSize,
    queryFn: async ({ page, size, search }) => {
      return await getOrderList(page, size, search);
    },
  });

  const handleDelete = async (id: number) => {
    try {
      await deleteOrder(id);
      toast.success("Xóa thành công");
      if (data?.length === 1 && pageIndex > 0) setPageIndex((prev) => prev - 1);
      else refetch();
    } catch {
      toast.error("Failed to delete order");
    }
  };

  return (
    <>
      <CrudPageWrapper
        title="Order Management"
        isSearchable={false}
        createButton={


          <CreateDialogForm

            title="Add Order"
            description="Create a new order entry"
            triggerText=""
            triggerIcon={<PlusCircleIcon className="w-10 h-10" />}
            schema={orderSchema}
            onSubmit={async (values) => {
              try {
                const response = await AxiosAPI.post("api/Order/dashboard", {
                  ...values,
                });

                const data = response.data as OrderResponse;

                const selectedPaymentMethod = paymentMethods.find(
                  (method) => method.id === values.paymentMethodId
                );

                const isBanking = selectedPaymentMethod?.methodName.toLowerCase() === "banking";
                const isCash = selectedPaymentMethod?.methodName.toLowerCase() === "cash";

                if (isBanking && data.paymentLink) {
                  setPaymentLink(data.paymentLink);
                  setIsDialogOpen(true);
                 
                  refetch();
                } else if (isCash && data.code) {
                  const sessionResponse = await AxiosAPI.post(
                    `/api/Session/${data.code}`, {}
                  );
                  const sessionData = sessionResponse.data;


                  setCashOrderInfo({
                    orderCode: data.code,
                    sessionInfo: sessionData,
                  });
                  setIsCashDialogOpen(true);
                
                  refetch();
                } else {
                  console.error("Unexpected response:", data);
                  toast.error("Failed to create order: No payment link or order code.");
                }
               
              } catch (error) {
                toast.error(
                  error instanceof Error
                    ? `Error creating order: ${error.message}`
                    : "Unknown error"
                );
              }
            }}

            fields={[
              {
                type: "text",
                name: "email",
                label: "Email",
                placeholder: "Enter your email",
              },
              {
                type: "text",
                name: "phone",
                label: "Phone",
                placeholder: "Enter your phone number",
              },
              {
                type: "select",
                name: "typeSessionId",
                label: "Session Type",
                options: typeSessions.map((session) => ({
                  value: session.id,
                  label: session.name,
                })),
              },
              {
                type: "text",
                name: "couponCode",
                label: "Coupon Code",
                placeholder: "Enter coupon code (optional)",
              },
              {
                type: "select",
                name: "paymentMethodId",
                label: "Payment Method",
                options: paymentMethods.map((method) => ({
                  value: method.id,
                  label: method.methodName,
                })),
              },
            ]}
          />

        }
        data={data}
        columns={columns(handleDelete, refetch)}
        isLoading={isLoading}
        pageCount={Math.ceil(totalItems / pageSize)}
        pageIndex={pageIndex}
        pageSize={pageSize}
        onPageChange={setPageIndex}
        onPageSizeChange={setPageSize}
      />

      <PaymentDialog
        open={isDialogOpen}
        onClose={() => setIsDialogOpen(false)}
        paymentLink={paymentLink}
      />
      {cashOrderInfo && (
        <CashOrderDialog
          open={isCashDialogOpen}
          onClose={() => setIsCashDialogOpen(false)}
          orderCode={cashOrderInfo.orderCode}
          sessionInfo={cashOrderInfo.sessionInfo}
        />
      )}
    </>
  );
}
