import React, { useEffect, useState } from "react";
import AxiosAPI from "@/configs/axios";
import { Loader, Button } from "@mantine/core";
import { Label } from "@/components/ui/label";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogDescription,
  DialogTrigger,
  DialogFooter,
} from "@/components/ui/dialog";
import { DepositProduct } from "@/types/type";
import { FaEye } from "react-icons/fa";
const ViewDetailDepositProduct = ({ id }: { id: number }) => {
  const [data, setData] = useState<DepositProduct | null>(null);
  const [loading, setLoading] = useState(true);
  const [open, setOpen] = useState(false);

  useEffect(() => {
    if (open && id) {
      const fetchData = async () => {
        try {
          const res = await AxiosAPI.get(`/api/DepositProduct/${id}`);
          setData(res.data as any);
        } catch (error) {
          console.error("Failed to fetch detail:", error);
        } finally {
          setLoading(false);
        }
      };
      fetchData();
    }
  }, [open, id]);

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button variant="outline"><FaEye /></Button>
      </DialogTrigger>
      <DialogContent className="max-w-xl">
        <DialogHeader>
          <DialogTitle>Chi tiết Deposit Product</DialogTitle>
          <DialogDescription>
            Xem thông tin chi tiết sản phẩm
          </DialogDescription>
        </DialogHeader>

        {loading ? (
          <Loader />
        ) : data ? (
          <div className="grid gap-4 py-4 overflow-y-auto max-h-80">
            <div>
              <Label>ID:</Label>
              <p>{data.id}</p>
            </div>
            <div>
              <Label>Tên:</Label>
              <p>{data.name}</p>
            </div>
            <div>
              <Label>Mô tả:</Label>
              <p>{data.description}</p>
            </div>
            <div>
              <Label>Giá:</Label>
              <p>{data.price.toLocaleString()} VND</p>
            </div>
            <div>
              <Label>Amount Add:</Label>
              <p>{data.amountAdd}</p>
            </div>
            <div>
              <Label>Product ID:</Label>
              <p>{data.productId}</p>
            </div>
            <div>
              <Label>Ngày tạo:</Label>
              <p>{new Date(data.createdAt).toLocaleString()}</p>
            </div>
            <div>
              <Label>Ngày cập nhật:</Label>
              <p>{new Date(data.lastModified).toLocaleString()}</p>
            </div>
          </div>
        ) : (
          <p>Không tìm thấy thông tin sản phẩm.</p>
        )}

        <DialogFooter>
          <Button onClick={() => setOpen(false)}>Đóng</Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

export default ViewDetailDepositProduct
