import AxiosAPI from "@/configs/axios";
import axios from "@/configs/axios";
import { Booth } from "@/types/booth";
import { PaginatedResponse } from "@/types/paginated-response";
import { User } from "@/types/type";
import { CustomerResponse } from "@/types/user-info";

export const getCustomerList = async (
  page: number,
  size: number
): Promise<PaginatedResponse<User>> => {
  const res = await AxiosAPI.get<CustomerResponse>("/api/User/customer", {
    params: { PageNumber: page, PageSize: size },
  });
  return {
    items: res.data?.data || [],
    totalItems: res.data?.totalCount || 0,
  };
};

export const getStaffList = async (
  page: number,
  size: number
): Promise<PaginatedResponse<User>> => {
  const res = await AxiosAPI.get<CustomerResponse>("/api/User/staff", {
    params: { PageNumber: page, PageSize: size },
  });
  return {
    items: res.data?.data || [],
    totalItems: res.data?.totalCount || 0,
  };
};
