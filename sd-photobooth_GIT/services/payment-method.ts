import AxiosAPI from "@/configs/axios";
import { PaginatedResponse } from "@/types/paginated-response";
import { PaymentMethod } from "@/types/type";

export const getPaymentMethods = async ({
  page,
  pageSize,
}: {
  page: number;
  pageSize: number;
}): Promise<PaginatedResponse<PaymentMethod>> => {
  const res = await AxiosAPI.get<PaymentMethod[]>(
    "/api/PaymentMethod",
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

export const getPaymentMethodCount = async (): Promise<number> => {
  const res = await AxiosAPI.get<number>("/api/PaymentMethod/count");
  return res.data || 0;
};

export const searchPaymentMethodsByName = async (
  name: string
): Promise<PaymentMethod[]> => {
  const res = await AxiosAPI.get<PaymentMethod[]>(
    `/api/PaymentMethod/by-name/${name}`
  );
  return res.data || [];
};

export const createPaymentMethod = async (data: PaymentMethod) => {
  const res = await AxiosAPI.post("/api/PaymentMethod", data);
  return res.data;
};

export const updatePaymentMethod = async ({
  id,
  data,
}: {
  id: number;
  data: PaymentMethod;
}) => {
  const res = await AxiosAPI.put(`/api/PaymentMethod/${id}`, data);
  return res.data;
};

export const deletePaymentMethod = async (id: number) => {
  const res = await AxiosAPI.delete(`/api/PaymentMethod/${id}`);
  return res.data;
};

export const getPaymentMethodById = async (id: number) => {
  const res = await AxiosAPI.get(`/api/PaymentMethod/${id}`);
  return res.data;
};
