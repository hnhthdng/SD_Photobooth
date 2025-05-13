import AxiosAPI from "@/configs/axios";
import { PaginatedResponse } from "@/types/paginated-response";
import { Transaction } from "@/types/type";

export const getTransaction = async (
  page: number,
  size: number
): Promise<PaginatedResponse<Transaction>> => {
  const [res, countRes] = await Promise.all([
    AxiosAPI.get<Transaction[]>(`/api/Transaction`, {
      params: { PageNumber: page, PageSize: size },
    }),
    AxiosAPI.get<number>("/api/Transaction/count"),
  ]);

  return {
    items: res.data ?? [],
    totalItems: countRes.data ?? 0,
  };
};
