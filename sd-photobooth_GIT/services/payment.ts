import AxiosAPI from "@/configs/axios";
import { PaginatedResponse } from "@/types/paginated-response";
import { Payment } from "@/types/type";

export const getPayment = async (
  page: number,
  size: number,
  search?: string
): Promise<PaginatedResponse<Payment>> => {
  const [res, countRes] = await Promise.all([
    AxiosAPI.get<Payment[]>("/api/Payment", {
      params: {
        PageNumber: page,
        PageSize: size,
      },
    }),
    AxiosAPI.get<number>("/api/Payment/count"),
  ]);
  return {
    items: res.data ?? [],
    totalItems: countRes.data ?? 0,
  };
};

export const deletePayment = async (id: number) => {
  const res = await AxiosAPI.delete(`/api/Payment/${id}`);
  if (res.status !== 200) throw new Error("Delete failed");
};
