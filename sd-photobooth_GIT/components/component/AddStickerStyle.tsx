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

const stickerStyleSchema = z.object({
     stickerStyleName: z.string().min(1, "Tên sticker style là bắt buộc"),
     description: z.string().min(1, "Mô tả là bắt buộc"),
});

type StickerStyleFormData = z.infer<typeof stickerStyleSchema>;

const AddStickerStyle = ({ onSuccess }: { onSuccess?: () => void }) => {
     const [isOpen, setIsOpen] = useState(false);
     const [loading, setLoading] = useState(false);

     const {
          register,
          handleSubmit,
          reset,
          formState: { errors },
     } = useForm<StickerStyleFormData>({
          resolver: zodResolver(stickerStyleSchema),
     });

     const onSubmit = async (data: StickerStyleFormData) => {
          try {
               setLoading(true);
               const response = await AxiosAPI.post("/api/StickerStyle", {
                    stickerStyleName: data.stickerStyleName,
                    description: data.description,
               });

               if (response.status >= 200 && response.status < 300) {
                    toast.success("Thêm sticker style thành công");
                    reset();
                    setIsOpen(false);
                    if (onSuccess) onSuccess();
               } else {
                    throw new Error("Thêm sticker style thất bại");
               }
          } catch (error) {
               console.error("Error adding sticker style:", error);
               toast.error("Thêm sticker style thất bại");
          } finally {
               setLoading(false);
          }
     };

     return (
          <Dialog open={isOpen} onOpenChange={setIsOpen}>
               <DialogTrigger asChild>
                    <Button variant="outline" className="gap-2">
                         <PlusCircle className="w-4 h-4" />
                         Thêm Sticker Style
                    </Button>
               </DialogTrigger>
               <DialogContent className="sm:max-w-[425px]">
                    <form onSubmit={handleSubmit(onSubmit)}>
                         <DialogHeader>
                              <DialogTitle>Thêm Sticker Style mới</DialogTitle>
                              <DialogDescription>
                                   Điền thông tin để thêm sticker style mới
                              </DialogDescription>
                         </DialogHeader>
                         <div className="grid gap-4 py-4">
                              <div className="grid grid-cols-4 items-center gap-4">
                                   <Label htmlFor="stickerStyleName" className="text-right">
                                        Tên
                                   </Label>
                                   <Input
                                        id="stickerStyleName"
                                        className="col-span-3"
                                        {...register("stickerStyleName")}
                                   />
                                   {errors.stickerStyleName && (
                                        <p className="col-span-4 text-right text-sm text-red-500">
                                             {errors.stickerStyleName.message}
                                        </p>
                                   )}
                              </div>
                              <div className="grid grid-cols-4 items-center gap-4">
                                   <Label htmlFor="description" className="text-right">
                                        Mô tả
                                   </Label>
                                   <Input
                                        id="description"
                                        className="col-span-3"
                                        {...register("description")}
                                   />
                                   {errors.description && (
                                        <p className="col-span-4 text-right text-sm text-red-500">
                                             {errors.description.message}
                                        </p>
                                   )}
                              </div>
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

export default AddStickerStyle;