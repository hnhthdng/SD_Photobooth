"use client";
import React, { useState } from "react";

import AxiosAPI from "@/configs/axios";
import { User } from "@/types/type";

const ViewDetailCustomer = () => {
  const [email, setEmail] = useState("");
  const [customer, setCustomer] = useState<User | null>(null);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleFetchCustomer = async () => {
    setLoading(true);
    setError("");
    setCustomer(null);

    try {
      const response = await AxiosAPI.get<User>(
        `/api/User/detail?email=${encodeURIComponent(email)}`
      );

     
      if (!response.data || !response.data.id) {
        throw new Error("NOT_FOUND"); 
      }

      setCustomer(response.data);
    } catch (err: any) {
      if (err.message === "NOT_FOUND" || err.response?.status === 404) {
        setError("Không tìm thấy thông tin khách hàng.");
      } else if (err.response?.status === 403) {
        setError("Bạn không có quyền truy cập thông tin này.");
      } else if (err.response?.status === 400) {
        setError("Email không hợp lệ.");
      } else if (err.response?.data?.message) {
        setError(err.response.data.message);
      } else {
        setError("Có lỗi xảy ra. Vui lòng thử lại sau.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="p-4 max-w-xl mx-auto bg-white rounded shadow">
      <h2 className="text-xl font-semibold mb-4">Xem thông tin khách hàng</h2>
      <div className="flex gap-2 mb-4">
        <input
          type="email"
          className="border p-2 flex-1 rounded"
          placeholder="Nhập email khách hàng"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          disabled={loading}
        />
        <button
          onClick={handleFetchCustomer}
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 disabled:bg-gray-400"
          disabled={loading || !email}
        >
          {loading ? "Đang tải..." : "Tìm"}
        </button>
      </div>

      {loading && <div className="p-4 text-center">Đang tải dữ liệu...</div>}

     
      {error && (
        <div className="p-4 border border-red-200 rounded bg-red-50 text-red-600">
          {error}
        </div>
      )}

     
      {!error && customer && (
        <div className="border-t pt-4 space-y-2">
          <p>
            <strong>ID:</strong> {customer.id}
          </p>
          <p>
            <strong>Họ tên:</strong> {customer.fullName || "Chưa có"}
          </p>
          <p>
            <strong>Tên người dùng:</strong> {customer.userName}
          </p>
          <p>
            <strong>Email:</strong> {customer.email}
          </p>
          <p>
            <strong>Số điện thoại:</strong> {customer.phoneNumber || "Chưa có"}
          </p>
          <p>
            <strong>Giới tính:</strong>{" "}
            {customer.gender === 0
              ? "Nam"
              : customer.gender === 1
              ? "Nữ"
              : "Khác"}
          </p>
          <p>
            <strong>Ngày sinh:</strong> {customer.birthDate || "Chưa có"}
          </p>
          <p>
            <strong>Vai trò:</strong> {customer.role}
          </p>
          <p>
            <strong>Bị cấm:</strong> {customer.isBanned ? "Có" : "Không"}
          </p>
        </div>
      )}
    </div>
  );
};

export default ViewDetailCustomer;
