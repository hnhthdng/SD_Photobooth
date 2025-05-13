import React, { useState } from "react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { toast } from "react-toastify";
import AxiosAPI from "@/configs/axios";

interface User {
  fullName: string | null;
  userName: string;
  phoneNumber: string;
  role: string;
  location: {
    locationName: string;
  };
}

const GPUser: React.FC = () => {
  const [email, setEmail] = useState("");
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(false);
  const [notFound, setNotFound] = useState(false);

  const fetchUserDetails = async () => {
    if (!email) return;
    setLoading(true);
    setNotFound(false);
    setUser(null); // Đảm bảo user bị xóa trước khi tải lại

    try {
      const response = await AxiosAPI.get<User>(`api/User/detail?email=${encodeURIComponent(email)}`);
      setUser(response.data);
      toast.success("Tải thông tin thành công!");
    } catch (error: any) {
      if (error.response) {
        if (error.response.status === 404) {
          setNotFound(true);
          toast.error("Không tìm thấy người dùng.");
        } else if (error.response.status === 500) {
          toast.error("Lỗi máy chủ nội bộ.");
        } else {
          toast.error("Đã xảy ra lỗi. Vui lòng thử lại sau.");
        }
      } else {
        toast.error("Lỗi kết nối. Vui lòng kiểm tra mạng.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <Card className="max-w-md mx-auto p-4">
      <CardHeader>
        <CardTitle>Thông tin người dùng</CardTitle>
      </CardHeader>
      <CardContent>
        <div className="space-y-4">
          <div className="flex flex-col space-y-1.5">
            <Label htmlFor="email">Email</Label>
            <Input
              id="email"
              type="email"
              placeholder="Nhập email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>
          <Button onClick={fetchUserDetails} disabled={loading} className="w-full">
            {loading ? "Đang tải..." : "Tìm kiếm"}
          </Button>
          {notFound && <p className="text-red-500 text-center">Không có kết quả.</p>}
          {user && !notFound && (
            <div className="mt-4 border p-4 rounded-lg">
              <p><strong>Họ và tên:</strong> {user.fullName || "Chưa có thông tin"}</p>
              <p><strong>Tên đăng nhập:</strong> {user.userName}</p>
              <p><strong>Số điện thoại:</strong> {user.phoneNumber}</p>
              <p><strong>Vai trò:</strong> {user.role}</p>
              <p><strong>Địa điểm:</strong> {user.location?.locationName || "Không có thông tin"}</p>
            </div>
          )}
        </div>
      </CardContent>
    </Card>
  );
};

export default GPUser;
