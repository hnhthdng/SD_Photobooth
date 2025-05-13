import AxiosAPI from "@/configs/axios";
import { PaginatedResponse } from "@/types/paginated-response";
import { DepositProduct, Session } from "@/types/type";

export const getSession = async (
  page: number,
  size: number,
  search: string | undefined
): Promise<PaginatedResponse<Session>> => {
  const url = search?.trim()
    ? `/api/Session/by-code/${search}`
    : `/api/Session`;
  const [res, countRes] = await Promise.all([
    AxiosAPI.get<Session[]>(url, {
      params: { PageNumber: page, PageSize: size },
    }),
    AxiosAPI.get<number>("/api/Session/count"),
  ]);
  return {
    items: res.data ?? [],
    totalItems: countRes.data ?? 0,
  };
};
