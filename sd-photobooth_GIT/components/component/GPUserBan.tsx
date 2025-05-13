"use client"
import React, { useState } from "react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Switch } from "@/components/ui/switch";
import { toast } from "react-toastify";
import AxiosAPI from "@/configs/axios";

const GPUserBan = () => {
  const [email, setEmail] = useState("");
  const [isBanned, setIsBanned] = useState(false);
  const [loading, setLoading] = useState(false);

  const updateBanStatus = async () => {
    if (!email) {
      toast.error("Vui lòng nhập email.");
      return;
    }
    setLoading(true);
    try {
      await AxiosAPI.post(
        `api/User/update-ban-status?email=${encodeURIComponent(email)}&isBanned=${isBanned}`,{}
      );
      toast.success(`Cập nhật trạng thái thành công: ${isBanned ? "Bị cấm" : "Không bị cấm"}`);
    } catch (error: any) {
      toast.error("Lỗi khi cập nhật trạng thái. Vui lòng thử lại.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Card className="max-w-md mx-auto p-4">
      <CardHeader>
        <CardTitle>Cập nhật trạng thái cấm</CardTitle>
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
          <div className="flex items-center justify-between">
            <Label htmlFor="ban-switch">Cấm người dùng</Label>
            <Switch id="ban-switch" checked={isBanned} onCheckedChange={setIsBanned} />
          </div>
          <Button onClick={updateBanStatus} disabled={loading} className="w-full">
            {loading ? "Đang cập nhật..." : "Cập nhật trạng thái"}
          </Button>
        </div>
      </CardContent>
    </Card>
  );
};

export default GPUserBan;
