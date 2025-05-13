import { z } from "zod";

export const couponSchema = z
  .object({
    name: z.string().min(1, "Name is required").max(100),
    description: z.string().max(500).optional(),
    code: z.string().min(1, "Code is required").max(50),
    discount: z.coerce.number().min(0).optional(),
    discountPercent: z.coerce.number().min(0).max(100).optional(),
    startDate: z.string().min(1, "Start date is required"),
    endDate: z.string().min(1, "End date is required"),
    maxUse: z.coerce.number().min(1).optional(),
    maxDiscount: z.coerce.number().min(0).optional(),
    minOrder: z.coerce.number().min(0).optional(),
    isActive: z.boolean().default(true),
  })
  .superRefine((data, ctx) => {
    const hasDiscount = (data.discount ?? 0) > 0;
    const hasDiscountPercent = (data.discountPercent ?? 0) > 0;

    if (!hasDiscount && !hasDiscountPercent) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message:
          "Phải nhập một trong hai: 'Discount' hoặc 'DiscountPercent' với giá trị lớn hơn 0",
        path: ["discount"],
      });
    }

    if (hasDiscount && hasDiscountPercent) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message:
          "Chỉ được nhập một trong hai: 'Discount' hoặc 'DiscountPercent'",
        path: ["discount"],
      });
    }

    if (
      hasDiscountPercent &&
      data.discountPercent &&
      (data.discountPercent < 0 || data.discountPercent > 100)
    ) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: "DiscountPercent phải từ 0 đến 100",
        path: ["discountPercent"],
      });
    }
  });
