import AxiosAPI from "@/configs/axios";

import { Booth } from "@/types/booth";
import { PaginatedResponse } from "@/types/paginated-response";

export const getBoothList = async (
  page: number,
  size: number,
  search?: string
): Promise<PaginatedResponse<Booth>> => {
  const url = search?.trim() ? `/api/Booth/by-name/${search}` : `/api/Booth`;
  const [res, countRes] = await Promise.all([
    AxiosAPI.get<Booth[]>(url, {
      params: { PageNumber: page, PageSize: size },
    }),
    AxiosAPI.get<number>("/api/Booth/count"),
  ]);

  return {
    items: res.data ?? [],
    totalItems: countRes.data ?? 0,
  };
};

export const deleteBooth = async (id: number) => {
  const res = await AxiosAPI.delete(`/api/Booth/${id}`);
  if (res.status !== 200) throw new Error("Delete failed");
};
