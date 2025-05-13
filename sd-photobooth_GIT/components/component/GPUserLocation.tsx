import React, { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { toast } from "react-toastify";
import AxiosAPI from "@/configs/axios";

interface Location {
  id: number;
  locationName: string;
}

const GPUserLocation: React.FC = () => {
  const [email, setEmail] = useState("");
  const [locations, setLocations] = useState<Location[]>([]);
  const [selectedLocation, setSelectedLocation] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    fetchLocations();
  }, []);

  const fetchLocations = async () => {
    try {
      const response = await AxiosAPI.get("api/Location");
      setLocations(response.data as Location[]);
    } catch (error) {
      console.error("Fetch locations error:", error);
      toast.error("Không thể tải danh sách địa điểm.");
    }
  };

  //   const handleSubmit = async (e: React.FormEvent) => {
  //     e.preventDefault();
  // //     if (!email || !selectedLocation || loading) return;
  // if (loading) return;
  //     setLoading(true);

  //     try {
  //       await AxiosAPI.post("api/User/staff/move-location", { email, locationId: selectedLocation });
  //       toast.success("Chuyển địa điểm thành công!");
  //     } catch (error) {
  //       console.error("Update Error:", error);
  //       toast.error("Chuyển địa điểm thất bại.");
  //     } finally {
  //       setLoading(false);
  //     }
  //   };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (loading) return;
    setLoading(true);

    try {
      // const url = `api/User/staff/move-location?email=${encodeURIComponent(email)}&locationId=${selectedLocation}`;
      await AxiosAPI.post(
        `/api/User/staff/move-location?email=${encodeURIComponent(
          email
        )}&locationId=${selectedLocation}`,
        {}
      );
      toast.success("Chuyển địa điểm thành công!");
    } catch (error) {
      console.error("Update Error:", error);
      toast.error("Chuyển địa điểm thất bại.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Card className="max-w-md mx-auto p-4">
      <CardHeader>
        <CardTitle>Chuyển địa điểm</CardTitle>
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
            <Label htmlFor="location">Chọn địa điểm</Label>
            <Select
              onValueChange={(value) => setSelectedLocation(Number(value))}
            >
              <SelectTrigger>
                <SelectValue placeholder="-- Chọn địa điểm --" />
              </SelectTrigger>
              <SelectContent>
                {locations.map((loc) => (
                  <SelectItem key={loc.id} value={String(loc.id)}>
                    {loc.locationName}
                  </SelectItem>
                ))}
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

export default GPUserLocation;
