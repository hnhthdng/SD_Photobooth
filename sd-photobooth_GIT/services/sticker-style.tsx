import AxiosAPI from "@/configs/axios";
import { PaginatedResponse } from "@/types/paginated-response";
import { StickerStyle } from "@/types/type";

export const getStickerStyles = async ({
  page,
  pageSize,
}: {
  page: number;
  pageSize: number;
}): Promise<PaginatedResponse<StickerStyle>> => {
  const res = await AxiosAPI.get<StickerStyle[]>(
    "/api/StickerStyle/all",
    {
      params: {
        PageNumber: page,
        PageSize: pageSize,
      },
    }
  );

  return {
    items: res.data  || [],
    totalItems:  res.data ? res.data.length : 0,
  };
 
};

export const getStickerStyleCount = async (): Promise<number> => {
  const res = await AxiosAPI.get<number>("/api/StickerStyle/count");
  return res.data || 0;
};


export const createStickerStyle = async (data: StickerStyle) => {
  const res = await AxiosAPI.post("/api/StickerStyle", data);
  return res.data;
};

export const updateStickerStyle = async ({
  id,
  data,
}: {
  id: number;
  data: StickerStyle;
}) => {
  const res = await AxiosAPI.put(`/api/StickerStyle/${id}`, data);
  return res.data;
};

export const deleteStickerStyle = async (id: number) => {
  const res = await AxiosAPI.delete(`/api/StickerStyle/${id}`);
  return res.data;
};

export const getStickerStyleById = async (id: number) => {
  const res = await AxiosAPI.get(`/api/StickerStyle/${id}`);
  return res.data;
};
