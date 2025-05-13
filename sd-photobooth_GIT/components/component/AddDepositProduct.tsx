"use client";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { PlusCircle } from "lucide-react";
import { toast } from "react-toastify";
import AxiosAPI from "@/configs/axios";

const DepositProductSchema = z.object({
  name: z.string().min(1, "Tên là bắt buộc"),
  description: z.string().min(1, "Mô tả là bắt buộc"),
  price: z.coerce.number().min(0, "Giá phải >= 0"),
  amountAdd: z.coerce.number().min(0, "Số lượng phải >= 0"),
  productId: z.string().min(1, "Product ID là bắt buộc"),
});

type DepositProductFormData = z.infer<typeof DepositProductSchema>;

const AddDepositProduct = ({ onSuccess }: { onSuccess?: () => void }) => {
  const [isOpen, setIsOpen] = useState(false);
  const [loading, setLoading] = useState(false);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<DepositProductFormData>({
    resolver: zodResolver(DepositProductSchema),
  });

  const onSubmit = async (data: DepositProductFormData) => {
    try {
      setLoading(true);
      const response = await AxiosAPI.post("/api/DepositProduct", data);
      if (response.status >= 200 && response.status < 300) {
        toast.success("Thêm Deposit Product thành công");
        reset();
        setIsOpen(false);
        if (onSuccess) onSuccess();
      } else {
        throw new Error("Thêm thất bại");
      }
    } catch (error) {
      console.error("Error adding deposit product:", error);
      toast.error("Thêm thất bại");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={isOpen} onOpenChange={setIsOpen}>
      <DialogTrigger asChild>
        <Button variant="outline" className="gap-2">
          <PlusCircle className="w-4 h-4" />
          Thêm Deposit Product
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <form onSubmit={handleSubmit(onSubmit)}>
          <DialogHeader>
            <DialogTitle>Thêm Deposit Product</DialogTitle>
            <DialogDescription>Điền thông tin để thêm mới</DialogDescription>
          </DialogHeader>
          <div className="grid gap-4 py-4">
            {[
              { id: "name", label: "Tên" },
              { id: "description", label: "Mô tả" },
              { id: "price", label: "Giá" },
              { id: "amountAdd", label: "Số lượng thêm" },
              { id: "productId", label: "Product ID" },
            ].map(({ id, label }) => (
              <div key={id} className="grid grid-cols-4 items-center gap-4">
                <Label htmlFor={id} className="text-right">
                  {label}
                </Label>
                <Input
                  id={id}
                  className="col-span-3"
                  {...register(id as keyof DepositProductFormData)}
                />
                {errors[id as keyof DepositProductFormData] && (
                  <p className="col-span-4 text-right text-sm text-red-500">
                    {errors[id as keyof DepositProductFormData]?.message}
                  </p>
                )}
              </div>
            ))}
          </div>
          <DialogFooter>
            <Button type="submit" disabled={loading}>
              {loading ? "Đang xử lý..." : "Lưu"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
};

export default AddDepositProduct;
