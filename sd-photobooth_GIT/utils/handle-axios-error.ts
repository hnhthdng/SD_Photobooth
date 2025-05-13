import { CustomAxiosResponse } from "@/types/axios-response";
import axios from "axios";

const handleAxiosError = <T>(error: unknown): CustomAxiosResponse<T> => {
  console.error("❌ Axios Error:", error);
  if (axios.isAxiosError(error)) {
    console.error(
      "❌ Axios Error:",
      error.response?.status,
      error.response?.data
    );
    return {
      data: error.response?.data || null,
      status: error.response?.status || 500,
      statusText: error.response?.statusText || "Internal Server Error",
    };
  }
  console.error("❌ Unknown Error:", error);
  return {
    data: null,
    status: 500,
    statusText: "Internal Server Error",
  };
};

export default handleAxiosError;