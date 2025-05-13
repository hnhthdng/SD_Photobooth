import { CustomAxiosResponse } from "@/types/axios-response";
import handleAxiosError from "@/utils/handle-axios-error";
import axios, { AxiosRequestConfig } from "axios";
import Cookies from "js-cookie";

const instance = axios.create({
  baseURL: process.env.NEXT_PUBLIC_SERVER_APP_API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

instance.interceptors.request.use((config) => {
  const token = Cookies.get("AccessToken");

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

instance.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response?.status === 401) {
      Cookies.remove("AccessToken");
    }
    return Promise.reject(error);
  }
);

const AxiosAPI = {
  get: async <T>(
    url: string,
    config?: AxiosRequestConfig
  ): Promise<CustomAxiosResponse<T>> => {
    try {
      const response = await instance.get<T>(url, config);
      return response;
    } catch (error) {
      return handleAxiosError<T>(error);
    }
  },
  post: async <T>(
    url: string,
    data: T,
    config?: AxiosRequestConfig
  ): Promise<CustomAxiosResponse<T>> => {
    try {
      const response = await instance.post<T>(url, data, config);
      return response;
    } catch (error) {
      return handleAxiosError<T>(error);
    }
  },
  put: async <T>(
    url: string,
    data: T,
    config?: AxiosRequestConfig
  ): Promise<CustomAxiosResponse<T>> => {
    try {
      const response = await instance.put<T>(url, data, config);
      return response;
    } catch (error) {
      return handleAxiosError<T>(error);
    }
  },
  delete: async <T>(
    url: string,
    config?: AxiosRequestConfig
  ): Promise<CustomAxiosResponse<T>> => {
    try {
      const response = await instance.delete<T>(url, config);
      return response;
    } catch (error) {
      return handleAxiosError<T>(error);
    }
  },
  patch: async <T>(
    url: string,
    data: T,
    config?: AxiosRequestConfig
  ): Promise<CustomAxiosResponse<T>> => {
    try {
      const response = await instance.patch<T>(url, data, config);
      return response;
    } catch (error) {
      return handleAxiosError<T>(error);
    }
  },
};

export default AxiosAPI;
