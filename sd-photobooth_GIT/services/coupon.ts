import AxiosAPI from "@/configs/axios";
import { PaginatedResponse } from "@/types/paginated-response";
import { Coupon } from "@/types/type";

export const getCouponList = async (
  page: number,
  size: number,
  search?: string
): Promise<PaginatedResponse<Coupon>> => {
  const url = search?.trim() ? `/api/Coupon/search/${search}` : "/api/Coupon";
  const [res, countRes] = await Promise.all([
    AxiosAPI.get<Coupon[]>(url, {
      params: { PageNumber: page, PageSize: size },
    }),
    AxiosAPI.get<number>("/api/Coupon/count"),
  ]);

  return {
    items: res.data ?? [],
    totalItems: countRes.data ?? 0,
  };
};

export const deleteCoupon = async (id: number) => {
  const res = await AxiosAPI.delete(`/api/Coupon/${id}`);
  if (res.status !== 200) throw new Error("Delete failed");
};
