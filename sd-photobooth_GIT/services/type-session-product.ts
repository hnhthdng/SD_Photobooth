import AxiosAPI from "@/configs/axios";
import axios from "@/configs/axios";
import { PaginatedResponse } from "@/types/paginated-response";
import { TypeSessionProduct } from "@/types/type";

export const getTypeSessionProductList = async (
  page: number,
  size: number
): Promise<PaginatedResponse<TypeSessionProduct>> => {
  const [res, countRes] = await Promise.all([
    AxiosAPI.get<TypeSessionProduct[]>("/api/TypeSessionProduct", {
      params: { PageNumber: page, PageSize: size },
    }),
    AxiosAPI.get<number>("/api/TypeSessionProduct/count"),
  ]);

  return {
    items: res.data ?? [],
    totalItems: countRes.data ?? 0,
  };
};

export const deleteTypeSessionProduct = async (id: number) => {
  const res = await AxiosAPI.delete(`/api/TypeSessionProduct/${id}`);
  if (res.status !== 200 && res.status !== 204) throw new Error("Delete failed");
};
