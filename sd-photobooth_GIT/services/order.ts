import AxiosAPI from "@/configs/axios";
import axios from "@/configs/axios";
import { PaginatedResponse } from "@/types/paginated-response";
import { Order } from "@/types/type";

export const getOrderList = async (
  page: number,
  size: number,
  search?: string
): Promise<PaginatedResponse<Order>> => {
  const url = search?.trim() ? `/api/Order/by-name/${search}` : `/api/Order`;
  const [res, countRes] = await Promise.all([
    AxiosAPI.get<Order[]>(url, {
      params: { PageNumber: page, PageSize: size },
    }),
    AxiosAPI.get<number>("/api/Order/count"),
  ]);

  return {
    items: res.data ?? [],
    totalItems: countRes.data ?? 0,
  };
};

export const deleteOrder = async (id: number) => {
  const res = await axios.delete(`/api/Order/${id}`);
  if (res.status !== 200) throw new Error("Delete failed");
};
