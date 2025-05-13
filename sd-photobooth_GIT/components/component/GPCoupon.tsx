"use client";

import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
  DialogFooter,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Switch } from "@/components/ui/switch";
import AxiosAPI from "@/configs/axios";
import { toast } from "react-toastify";
import { Coupon } from "@/types/type";
import { FaPen } from "react-icons/fa";

const couponSchema = z
  .object({
    name: z.string().min(1, "Tên không được để trống").max(100, "Tên quá dài"),
    description: z.string().max(500, "Mô tả quá dài").optional(),
    code: z.string().min(1, "Mã không được để trống").max(50, "Mã quá dài"),
    discount: z.coerce.number().min(0, "Giảm giá không được âm").nullable(),
    discountPercent: z.coerce
      .number()
      .min(0, "Phần trăm giảm giá không được âm")
      .max(100, "Tối đa 100%")
      .nullable(),
    startDate: z.string().min(1, "Ngày bắt đầu không được để trống"),
    endDate: z.string().min(1, "Ngày kết thúc không được để trống"),
    maxUse: z.coerce.number().min(1, "Tối thiểu 1 lần sử dụng").optional(),
    maxDiscount: z.coerce
      .number()
      .min(0, "Giảm giá tối đa không được âm")
      .optional(),
    minOrder: z.coerce
      .number()
      .min(0, "Đơn hàng tối thiểu không được âm")
      .optional(),
    isActive: z.boolean().default(true),
  })
  .superRefine((data, ctx) => {
    const hasDiscount = (data.discount ?? 0) > 0;
    const hasDiscountPercent = (data.discountPercent ?? 0) > 0;

    if (!hasDiscount && !hasDiscountPercent) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: "Yêu cầu nhập giảm giá hoặc phần trăm giảm giá",
        path: ["discount"],
      });
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: "Yêu cầu nhập giảm giá hoặc phần trăm giảm giá",
        path: ["discountPercent"],
      });
    }

    if (hasDiscount && hasDiscountPercent) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: "Chỉ được nhập một trong hai",
        path: ["discount"],
      });
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: "Chỉ được nhập một trong hai",
        path: ["discountPercent"],
      });
    }
  });

type CouponFormValues = z.infer<typeof couponSchema>;

interface UpdateCouponProps {
  couponId: number;
  onUpdateSuccess: () => void;
}

const UpdateCoupon = ({ couponId, onUpdateSuccess }: UpdateCouponProps) => {
  const [isOpen, setIsOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const [fetching, setFetching] = useState(false);

  const {
    register,
    handleSubmit,
    watch,
    reset,
    setValue,
    formState: { errors },
  } = useForm<CouponFormValues>({
    resolver: zodResolver(couponSchema),
    defaultValues: {
      name: "",
      description: "",
      code: "",
      discount: null,
      discountPercent: null,
      startDate: "",
      endDate: "",
      maxUse: undefined,
      maxDiscount: undefined,
      minOrder: undefined,
      isActive: true,
    },
  });

  const fetchCoupon = async () => {
    setFetching(true);
    try {
      const response = await AxiosAPI.get<Coupon>(`/api/Coupon/${couponId}`);
      if (response.status === 200) {
        const data = response.data;
        reset({
          ...data,
          startDate: data?.startDate?.slice(0, 10) || "",
          endDate: data?.endDate?.slice(0, 10) || "",
          discount: data?.discount ?? null,

          discountPercent: data?.discountPercent != null ? data.discountPercent : null,

        });
      } else {
        toast.error("Không lấy được thông tin coupon");
        setIsOpen(false);
      }
    } catch (error) {
      toast.error("Lỗi khi lấy thông tin coupon");
      setIsOpen(false);
    } finally {
      setFetching(false);
    }
  };

  useEffect(() => {
    if (isOpen) {
      fetchCoupon();
    } else {
      reset(); // Reset form when dialog closes
    }
  }, [isOpen]);

  const watchDiscount = watch("discount");
  const watchDiscountPercent = watch("discountPercent");

  const handleDiscountChange = (
    e: React.ChangeEvent<HTMLInputElement>,
    field: "discount" | "discountPercent"
  ) => {
    const value = e.target.value;


    if (value === '') {

      setValue(field, null, { shouldValidate: true });
      return;
    }

    const numValue = Number(value);
    if (isNaN(numValue)) return;

    setValue(field, numValue, { shouldValidate: true });

    // Clear the other field when one is set
    if (field === "discount" && numValue > 0) {
      setValue("discountPercent", null, { shouldValidate: true });
    }
    if (field === "discountPercent" && numValue > 0) {
      setValue("discount", null, { shouldValidate: true });
    }
  };

  const onSubmit = async (values: CouponFormValues) => {
    setLoading(true);
    try {
      const payload = {
        ...values,
        discount: values.discount,

        discountPercent: values.discountPercent !== null ? values.discountPercent : null,

      };

      await AxiosAPI.put(`/api/Coupon/${couponId}`, payload);
      toast.success("Cập nhật coupon thành công!");
      setIsOpen(false);
      onUpdateSuccess();
    } catch (err: any) {
      const errorMessage =
        err.response?.data?.message ||
        err.message ||
        "Đã xảy ra lỗi khi cập nhật";
      toast.error(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={isOpen} onOpenChange={setIsOpen}>
      <DialogTrigger asChild>
        <Button variant="outline"><FaPen /></Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px] max-h-[80vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>Cập nhật Coupon</DialogTitle>
          <DialogDescription>Chỉnh sửa thông tin coupon</DialogDescription>
        </DialogHeader>
        {fetching ? (
          <p>Đang tải dữ liệu...</p>
        ) : (
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="name">Tên coupon</Label>
              <Input id="name" {...register("name")} />
              {errors.name && (
                <p className="text-red-500 text-sm">{errors.name.message}</p>
              )}
            </div>

            <div className="space-y-2">
              <Label htmlFor="description">Mô tả</Label>
              <Input id="description" {...register("description")} />
              {errors.description && (
                <p className="text-red-500 text-sm">
                  {errors.description.message}
                </p>
              )}
            </div>

            <div className="space-y-2">
              <Label htmlFor="code">Mã coupon</Label>
              <Input id="code" {...register("code")} />
              {errors.code && (
                <p className="text-red-500 text-sm">{errors.code.message}</p>
              )}
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="discount">Số tiền giảm</Label>
                <Input
                  id="discount"
                  type="number"
                  value={watchDiscount ?? ""}
                  onChange={(e) => handleDiscountChange(e, "discount")}
                  disabled={!!watchDiscountPercent}
                />
                {errors.discount && (
                  <p className="text-red-500 text-sm">
                    {errors.discount.message}
                  </p>
                )}
              </div>
              <div className="space-y-2">
                <Label htmlFor="discountPercent">Phần trăm giảm (%)</Label>
                <Input
                  id="discountPercent"
                  type="number"
                  value={watchDiscountPercent ?? ""}
                  onChange={(e) => handleDiscountChange(e, "discountPercent")}
                  disabled={!!watchDiscount}
                />
                {errors.discountPercent && (
                  <p className="text-red-500 text-sm">
                    {errors.discountPercent.message}
                  </p>
                )}
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="startDate">Ngày bắt đầu</Label>
                <Input type="date" id="startDate" {...register("startDate")} />
                {errors.startDate && (
                  <p className="text-red-500 text-sm">
                    {errors.startDate.message}
                  </p>
                )}
              </div>
              <div className="space-y-2">
                <Label htmlFor="endDate">Ngày kết thúc</Label>
                <Input type="date" id="endDate" {...register("endDate")} />
                {errors.endDate && (
                  <p className="text-red-500 text-sm">
                    {errors.endDate.message}
                  </p>
                )}
              </div>
            </div>

            <div className="grid grid-cols-3 gap-4">
              <div className="space-y-2">
                <Label htmlFor="maxUse">Số lần sử dụng tối đa</Label>
                <Input type="number" id="maxUse" {...register("maxUse")} />
                {errors.maxUse && (
                  <p className="text-red-500 text-sm">
                    {errors.maxUse.message}
                  </p>
                )}
              </div>
              <div className="space-y-2">
                <Label htmlFor="maxDiscount">Giảm giá tối đa</Label>
                <Input
                  type="number"
                  id="maxDiscount"
                  {...register("maxDiscount")}
                />
                {errors.maxDiscount && (
                  <p className="text-red-500 text-sm">
                    {errors.maxDiscount.message}
                  </p>
                )}
              </div>
              <div className="space-y-2">
                <Label htmlFor="minOrder">Đơn hàng tối thiểu</Label>
                <Input type="number" id="minOrder" {...register("minOrder")} />
                {errors.minOrder && (
                  <p className="text-red-500 text-sm">
                    {errors.minOrder.message}
                  </p>
                )}
              </div>
            </div>

            <div className="flex items-center space-x-2">
              <Switch
                id="isActive"
                checked={watch("isActive")}
                onCheckedChange={(checked) => setValue("isActive", checked)}
              />
              <Label htmlFor="isActive">Kích hoạt</Label>
            </div>

            <DialogFooter>
              <Button type="submit" disabled={loading}>
                {loading ? "Đang cập nhật..." : "Cập nhật"}
              </Button>
            </DialogFooter>
          </form>
        )}
      </DialogContent>
    </Dialog>
  );
};

export default UpdateCoupon;
