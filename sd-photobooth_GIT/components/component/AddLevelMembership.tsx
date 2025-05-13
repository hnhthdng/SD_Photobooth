"use client";

import { useState } from "react";
import { useForm, Controller } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { NumericFormat } from "react-number-format";
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
import { Switch } from "@/components/ui/switch";
import AxiosAPI from "@/configs/axios";
import { toast } from "react-toastify";

const levelMembershipSchema = z.object({
  name: z.string().min(1, "Tên là bắt buộc"),
  description: z.string().optional(),
  point: z.coerce.number().min(0, "Điểm phải là số không âm"),
  isActive: z.boolean(),
  discountPercent: z.coerce
    .number()
    .min(0)
    .max(100, "Phần trăm giảm giá từ 0 đến 100"),
  maxDiscount: z.coerce.number().min(0),
  minOrder: z.coerce.number().min(0),
});

type LevelMembershipForm = z.infer<typeof levelMembershipSchema>;

const AddLevelMembership = ({ onSuccess }: { onSuccess?: () => void }) => {
  const [isOpen, setIsOpen] = useState(false);
  const [loading, setLoading] = useState(false);

  const {
    control,
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<LevelMembershipForm>({
    resolver: zodResolver(levelMembershipSchema),
    defaultValues: {
      name: "",
      description: "",
      point: 0,
      isActive: true,
      discountPercent: 0,
      maxDiscount: 0,
      minOrder: 0,
    },
  });

  const onSubmit = async (data: LevelMembershipForm) => {
    if (loading) return;
    setLoading(true);
    try {
      await AxiosAPI.post("/api/LevelMembership", data);
      toast.success("Thêm cấp độ thành viên thành công!");
      reset();
      setIsOpen(false);
      onSuccess?.();
    } catch (error: any) {
      toast.error(error?.response?.data?.message || "Đã có lỗi xảy ra");
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={isOpen} onOpenChange={setIsOpen}>
      <DialogTrigger asChild>
        <Button variant="outline">Thêm cấp độ thành viên</Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[500px] max-h-[80vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>Thêm cấp độ thành viên</DialogTitle>
          <DialogDescription>
            Nhập thông tin cấp độ thành viên và nhấn lưu.
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="grid gap-4">
          <div>
            <Label htmlFor="name">Tên</Label>
            <Input id="name" {...register("name")} />
            {errors.name && (
              <p className="text-red-500 text-sm">{errors.name.message}</p>
            )}
          </div>

          <div>
            <Label htmlFor="description">Mô tả</Label>
            <Input id="description" {...register("description")} />
          </div>

          <div>
            <Label htmlFor="point">Điểm</Label>
            <Input
              type="number"
              min="0"
              id="point"
              {...register("point", { valueAsNumber: true })}
            />
            {errors.point && (
              <p className="text-red-500 text-sm">{errors.point.message}</p>
            )}
          </div>

          <div>
            <Label htmlFor="discountPercent">Phần trăm giảm giá (%)</Label>
            <Controller
              name="discountPercent"
              control={control}
              render={({ field }) => (
                <NumericFormat
                  id="discountPercent"
                  allowNegative={false}
                  decimalScale={2}
                  fixedDecimalScale
                  customInput={Input}
                  value={field.value}
                  onValueChange={(values) => field.onChange(values.floatValue)}
                />
              )}
            />
            {errors.discountPercent && (
              <p className="text-red-500 text-sm">
                {errors.discountPercent.message}
              </p>
            )}
          </div>

          <div>
            <Label htmlFor="maxDiscount">Giảm giá tối đa</Label>
            <Input
              type="number"
              min="0"
              id="maxDiscount"
              {...register("maxDiscount", { valueAsNumber: true })}
            />
            {errors.maxDiscount && (
              <p className="text-red-500 text-sm">
                {errors.maxDiscount.message}
              </p>
            )}
          </div>

          <div>
            <Label htmlFor="minOrder">Đơn hàng tối thiểu</Label>
            <Input
              type="number"
              min="0"
              id="minOrder"
              {...register("minOrder", { valueAsNumber: true })}
            />
            {errors.minOrder && (
              <p className="text-red-500 text-sm">{errors.minOrder.message}</p>
            )}
          </div>

          <div className="flex items-center justify-between rounded-md border p-4">
            <span className="text-sm font-medium">Kích hoạt</span>
            <Controller
              name="isActive"
              control={control}
              render={({ field }) => (
                <Switch
                  checked={field.value}
                  onCheckedChange={field.onChange}
                />
              )}
            />
          </div>

          <DialogFooter>
            <Button type="submit" disabled={loading}>
              {loading ? "Đang lưu..." : "Lưu"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
};

export default AddLevelMembership;
