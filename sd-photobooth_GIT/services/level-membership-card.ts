import AxiosAPI from "@/configs/axios";
import axios from "@/configs/axios";
import { Booth } from "@/types/booth";
import { PaginatedResponse } from "@/types/paginated-response";

export const getLevelMembershipCards = async (
  page: number,
  size: number,
  search?: string
): Promise<PaginatedResponse<Booth>> => {
  const [res, countRes] = await Promise.all([
    AxiosAPI.get<Booth[]>("/api/MembershipCard", {
      params: { PageNumber: page, PageSize: size },
    }),
    AxiosAPI.get<number>("/api/MembershipCard/count"),
  ]);

  return {
    items: res.data ?? [],
    totalItems: countRes.data ?? 0,
  };
};

export const deleteLevelMembershipCard = async (id: number) => {
  const res = await AxiosAPI.delete(`/api/MembershipCard/${id}`);
  if (res.status !== 200) throw new Error("Delete failed");
};
