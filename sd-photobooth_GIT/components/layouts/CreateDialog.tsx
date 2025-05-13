"use client";

import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Switch } from "@/components/ui/switch";
import { DefaultValues, FieldValues, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { memo, ReactNode, useState } from "react";
import { ZodType } from "zod";
import { toast } from "react-toastify";
import { PlusCircleIcon } from "lucide-react";

type Field =
  | {
    type: "text";
    name: string;
    label: string;
    placeholder?: string;
    description?: string;
  }
  | {
    type: "select";
    name: string;
    label: string;
    options: { label: string; value: string | number }[];
    loading?: boolean;
  }
  | {
    type: "switch";
    name: string;
    label: string;
    description?: string;
  }
  | {
    type: "number";
    name: string;
    label: string;
    description?: string;
  }
  | {
    type: "date";
    name: string;
    label: string;
    description?: string;
  };

interface CreateDialogFormProps<T> {
  title: string;
  description?: string;
  triggerText: string;
  triggerIcon?: ReactNode;
  schema: ZodType<T>;
  defaultValues?: T;
  fields: Field[];
  onSubmit: (values: T) => Promise<void>;
  onSuccess?: () => void;
}

const CreateDialogForm = <T extends FieldValues>({
  title,
  description,
  triggerText,
  triggerIcon,
  schema,
  defaultValues,
  fields,
  onSubmit,
  onSuccess,
}: CreateDialogFormProps<T>) => {
  const [isOpen, setIsOpen] = useState(false);
  const [loading, setLoading] = useState(false);

  const {
    register,
    handleSubmit,
    setValue,
    watch,
    reset,
    formState: { errors },
  } = useForm<T>({
    resolver: zodResolver(schema),
    defaultValues: defaultValues as DefaultValues<T>,
  });

  const onInternalSubmit = async (values: T) => {
    try {
      setLoading(true);
      await onSubmit(values);
      toast.success("Thêm thành công!");
      reset();
      setIsOpen(false);
      onSuccess?.();
    } catch (error: any) {
      console.error(error);
      toast.error(error?.response?.data?.message || "Error occurred");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={isOpen} onOpenChange={setIsOpen}>
      <DialogTrigger asChild>
        <Button className="rounded-xl px-4 py-2 text-sm font-medium bg-gray-100 hover:bg-gray-200 dark:bg-[#2c2c2e] dark:hover:bg-[#3a3a3c] text-gray-800 dark:text-gray-100">
          {triggerIcon}

          {triggerText && <span className="ml-2">{triggerText}</span>}
        </Button>
      </DialogTrigger>

      <DialogContent className="sm:max-w-[500px] rounded-2xl p-8 bg-white/80 dark:bg-[#1c1c1e]/80 border border-gray-200 dark:border-gray-600 shadow-xl backdrop-blur-md">
        <DialogHeader className="mb-6">
          <DialogTitle className="text-xl font-semibold text-gray-900 dark:text-white flex items-center gap-2">
            {title}
          </DialogTitle>
          {description && (
            <DialogDescription className="text-sm text-gray-600 dark:text-gray-400">
              {description}
            </DialogDescription>
          )}
        </DialogHeader>

        <form
          onSubmit={handleSubmit(onInternalSubmit)}
          className="space-y-6 w-full px-6 py-4 max-h-[70vh] overflow-y-auto"
        >
          {fields.map((field) => {
            const errorMsg = errors?.[field.name as keyof T]?.message;

            if (field.type === "text" || field.type === "number" || field.type === "date") {
              return (
                <div key={field.name} className="space-y-1 w-full">
                  <Label htmlFor={field.name} className="text-sm text-gray-800 dark:text-gray-200 font-medium">
                    {field.label}
                  </Label>
                  <Input
                    id={field.name}
                    type={field.type === "number" ? "number" : field.type === "date" ? "date" : "text"}
                    {...register(field.name as any, field.type === "number" ? { valueAsNumber: true } : {})}
                    className="w-full rounded-2xl border border-gray-200 dark:border-gray-600 bg-white dark:bg-[#1c1c1e] text-gray-900 dark:text-gray-100 px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
                  />
                  {field.description && (
                    <p className="text-xs text-gray-500 dark:text-gray-400">{field.description}</p>
                  )}
                  {errorMsg && (
                    <p className="text-xs text-red-500">{`${errorMsg}`}</p>
                  )}
                </div>
              );
            }

            if (field.type === "select") {
              return (
                <div key={field.name} className="space-y-1 w-full">
                  <Label htmlFor={field.name} className="text-sm text-gray-800 dark:text-gray-200 font-medium">
                    {field.label}
                  </Label>
                  <select
                    id={field.name}
                    {...register(field.name as any)}
                    className="w-full appearance-none rounded-2xl border border-gray-200 dark:border-gray-600 bg-white dark:bg-[#1c1c1e] text-gray-900 dark:text-gray-100 px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
                    defaultValue=""
                  >
                    <option value="" disabled>
                      {field.loading ? "Loading..." : "Select"}
                    </option>
                    {field.options.map((opt) => (
                      <option key={opt.value} value={opt.value}>
                        {opt.label}
                      </option>
                    ))}
                  </select>
                  {errorMsg && (
                    <p className="text-xs text-red-500">{`${errorMsg}`}</p>
                  )}
                </div>
              );
            }

            if (field.type === "switch") {
              const checked = watch(field.name as any);
              return (
                <div
                  key={field.name}
                  className="flex items-center justify-between rounded-2xl border border-gray-200 dark:border-gray-600 bg-white dark:bg-[#1c1c1e] px-4 py-3"
                >
                  <div className="flex-1 space-y-0.5">
                    <p className="text-sm font-medium text-gray-800 dark:text-gray-100">
                      {field.label}
                    </p>
                    {field.description && (
                      <p className="text-xs text-gray-500 dark:text-gray-400">
                        {field.description}
                      </p>
                    )}
                  </div>
                  <Switch
                    id={field.name}
                    checked={checked as boolean}
                    onCheckedChange={(checked) => setValue(field.name as any, checked as any)}
                  />
                </div>
              );
            }

            return null;
          })}

          <DialogFooter className="pt-6">
            <Button
              type="submit"
              disabled={loading}
              className="w-full bg-blue-600 hover:bg-blue-700 text-white text-sm font-medium rounded-xl py-2 transition"
            >
              {loading ? "Saving..." : "Save"}
            </Button>
          </DialogFooter>
        </form>

      </DialogContent>
    </Dialog>

  );
};

export const MemoizedCreateDialogForm = memo(CreateDialogForm);
