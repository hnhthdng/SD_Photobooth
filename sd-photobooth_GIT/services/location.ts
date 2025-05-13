import AxiosAPI from "@/configs/axios";
import { PaginatedResponse } from "@/types/paginated-response";
import { Location } from "@/types/type";

export const getLocationList = async (
  page: number,
  size: number,
  search?: string
): Promise<PaginatedResponse<Location>> => {
  const url = search?.trim()
    ? `/api/Location/by-location/${search}`
    : `/api/Location`;
  const [res, countRes] = await Promise.all([
    AxiosAPI.get<Location[]>(url, {
      params: { PageNumber: page, PageSize: size },
    }),
    AxiosAPI.get<number>("/api/Location/count"),
  ]);

  return {
    items: res.data ?? [],
    totalItems: countRes.data ?? 0,
  };
};

export const getCountLocation = async (): Promise<number> => {
  const res = await AxiosAPI.get<number>("/api/Location/count");
  return res.data ?? 0;
};

export const getAllLocations = async (): Promise<Location[]> => {
  const res = await AxiosAPI.get<Location[]>("/api/Location");
  return res.data ?? [];
};

export const deleteLocation = async (id: number) => {
  const res = await AxiosAPI.delete(`/api/Location/${id}`);
  if (res.status !== 200) throw new Error("Delete failed");
};
