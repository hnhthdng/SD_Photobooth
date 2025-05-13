import { z } from "zod";

export const DepositProductSchema = z.object({
  name: z.string().min(1, "Tên là bắt buộc"),
  description: z.string().min(1, "Mô tả là bắt buộc"),
  price: z.coerce.number().min(0, "Giá phải >= 0"),
  amountAdd: z.preprocess((val) => {
    if (val === "" || val === null || val === undefined) return undefined;
    const parsed = Number(val);
    return isNaN(parsed) ? undefined : parsed;
  }, z.number().min(0, "Số lượng phải >= 0").optional()),

  // productId: z.string().min(1, "Product ID là bắt buộc"),
});
