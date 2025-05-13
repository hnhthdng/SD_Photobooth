import React, { useState } from "react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { toast } from "react-toastify";
import AxiosAPI from "@/configs/axios";

const GPRoleUser: React.FC = () => {
  const [email, setEmail] = useState("");
  const [selectedRole, setSelectedRole] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!email || selectedRole === null || loading) return;

    setLoading(true);
    try {
      
      await AxiosAPI.post(`api/User/change-role?email=${encodeURIComponent(email)}&role=${selectedRole}`, {});
      toast.success("Cập nhật vai trò thành công!");
    } catch (error) {
      console.error("Update Error:", error);
      toast.error("Cập nhật vai trò thất bại.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Card className="max-w-md mx-auto p-4">
      <CardHeader>
        <CardTitle>Thay đổi vai trò</CardTitle>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit} className="space-y-4">
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
          <div className="flex flex-col space-y-1.5">
            <Label htmlFor="role">Chọn vai trò</Label>
            <Select onValueChange={(value) => setSelectedRole(Number(value))}>
              <SelectTrigger>
                <SelectValue placeholder="-- Chọn vai trò --" />
              </SelectTrigger>
              <SelectContent>
                {/* <SelectItem value="0">Admin</SelectItem> */}
                <SelectItem value="1">Manager</SelectItem>
                <SelectItem value="2">Staff</SelectItem>
                
              </SelectContent>
            </Select>
          </div>
          <Button type="submit" disabled={loading} className="w-full">
            {loading ? "Đang xử lý..." : "Xác nhận"}
          </Button>
        </form>
      </CardContent>
    </Card>
  );
};

export default GPRoleUser;