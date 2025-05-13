"use client";

import {
     Dialog,
     DialogTrigger,
     DialogContent,
     DialogHeader,
     DialogTitle,
     DialogFooter,
     DialogClose,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { toast } from "react-toastify";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import { IoAddCircleOutline } from "react-icons/io5";

type Field = {
     name: string;
     label: string;
     type?: "text" | "number" | "textarea" | "select" | "checkbox";
     placeholder?: string;
     options?: { label: string; value: string }[]; // select
};

interface SimpleAddItemProps {
     label?: string;
     fields: Field[];
     schema: z.ZodObject<any>;
     queryKey: string;
     createFn: (data: any) => Promise<any>;
}

export const SimpleAddItem = ({
     label = "Thêm mới",
     fields,
     schema,
     queryKey,
     createFn,
}: SimpleAddItemProps) => {
     const [open, setOpen] = useState(false);
     const queryClient = useQueryClient();

     const {
          register,
          handleSubmit,
          reset,
          formState: { errors, isSubmitting },
     } = useForm({ resolver: zodResolver(schema) });

     const mutation = useMutation({
          mutationFn: createFn,
          onSuccess: () => {
               queryClient.invalidateQueries({ queryKey: [queryKey], exact: false });
               queryClient.invalidateQueries({ queryKey: [queryKey, "count"] });
               toast.success("Thêm thành công!");
               setOpen(false);
               reset();
          },
          onError: () => toast.error("Thêm thất bại."),
     });

     const onSubmit = async (data: any) => {
          await mutation.mutateAsync(data);
     };

     return (
          <Dialog open={open} onOpenChange={setOpen}>
               <DialogTrigger asChild>
                    <Button
                         variant="default"
                         className="rounded-2xl h-10 bg-gray-100 text-black shadow hover:shadow-lg hover:scale-105 transition-transform"
                    >
                         <IoAddCircleOutline size={24} />
                    </Button>
               </DialogTrigger>
               <DialogContent className="rounded-2xl p-6 bg-white shadow-lg max-w-lg mx-auto">
                    <DialogHeader>
                         <DialogTitle className="text-xl font-semibold text-gray-800">
                              {label}
                         </DialogTitle>
                    </DialogHeader>
                    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                         {fields.map((field) => (
                              <div key={field.name} className="space-y-1">
                                   <label className="text-sm font-medium text-gray-700 block">
                                        {field.label}
                                   </label>
                                   {field.type === "textarea" ? (
                                        <textarea
                                             {...register(field.name)}
                                             placeholder={field.placeholder}
                                             className="w-full rounded-lg p-3 border border-gray-300 bg-gray-100 focus:ring-2 focus:ring-blue-500 outline-none"
                                        />
                                   ) : field.type === "select" ? (
                                        <select
                                             {...register(field.name)}
                                             className="w-full rounded-lg p-3 border border-gray-300 bg-gray-100 focus:ring-2 focus:ring-blue-500 outline-none"
                                        >
                                             <option value="">-- Chọn --</option>
                                             {field.options?.map((opt) => (
                                                  <option key={opt.value} value={opt.value}>
                                                       {opt.label}
                                                  </option>
                                             ))}
                                        </select>
                                   ) : field.type === "checkbox" ? (
                                        <input
                                             type="checkbox"
                                             {...register(field.name)}
                                             className="rounded border-gray-300 focus:ring-blue-500"
                                        />
                                   ) : (
                                        <input
                                             type={field.type || "text"}
                                             {...register(field.name)}
                                             placeholder={field.placeholder}
                                             className="w-full rounded-lg p-3 border border-gray-300 bg-gray-100 focus:ring-2 focus:ring-blue-500 outline-none"
                                        />
                                   )}
                                   {errors[field.name] && (
                                        <p className="text-red-500 text-sm">
                                             {(errors[field.name]?.message as string) || ""}
                                        </p>
                                   )}
                              </div>
                         ))}

                         <DialogFooter className="mt-6 flex justify-between">
                              <DialogClose asChild>
                                   <Button
                                        type="button"
                                        variant="outline"
                                        className="rounded-lg text-gray-600 border px-6 py-2 hover:bg-gray-100 transition"
                                   >
                                        Hủy
                                   </Button>
                              </DialogClose>
                              <Button
                                   type="submit"
                                   disabled={isSubmitting}
                                   className="rounded-lg bg-blue-500 text-white px-6 py-2 hover:bg-blue-600 transition"
                              >
                                   Lưu
                              </Button>
                         </DialogFooter>
                    </form>
               </DialogContent>
          </Dialog>
     );
};
