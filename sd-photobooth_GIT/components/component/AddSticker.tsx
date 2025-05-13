"use client";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
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
import AxiosAPI from "@/configs/axios";
import { toast } from "react-toastify";

const formSchema = z.object({
  name: z.string().min(1, "Sticker name is required"),
  StickerFile: z.any().refine((file) => file?.[0], "Sticker file is required"),
  stickerStyleName: z.string().min(1, "Sticker style is required"),
});

interface AddStickerProps {
  onAddSuccess: () => void;
}

const AddSticker: React.FC<AddStickerProps> = ({ onAddSuccess }) => {
  const [isOpen, setIsOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const [styles, setStyles] = useState<{ id: number; stickerStyleName: string }[]>([]);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm({
    resolver: zodResolver(formSchema),
    defaultValues: {
      name: "",
      stickerStyleName: "",
      StickerFile: undefined,
    },
  });

  useEffect(() => {
    const fetchStyles = async () => {
      try {
        const response = await AxiosAPI.get("/api/StickerStyle/all");
        setStyles(response.data as any);
      } catch (error) {
        console.error("Failed to load sticker styles", error);
      }
    };
    fetchStyles();
  }, []);

  const onSubmit = async (values: any) => {
    if (loading) return;
    setLoading(true);

    const formData = new FormData();
    formData.append("name", values.name);
    formData.append("stickerStyleId", values.stickerStyleName);
    formData.append("StickerFile", values.StickerFile?.[0]); // Binary file for StickerFile

    try {
      const response = await AxiosAPI.post(`/api/Sticker`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });

      if (response.status === 200) {
        toast.success("Thêm thành công! ");
        // console.log(response.data);
        reset();
        setIsOpen(false);
        onAddSuccess();
      }
    } catch (err: any) {
      console.error("Save Error:", err);
      toast.error(err.response?.data?.message || "Something went wrong");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={isOpen} onOpenChange={setIsOpen}>
      <DialogTrigger asChild>
        <Button variant="outline">Add Sticker</Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle>Add Sticker</DialogTitle>
          <DialogDescription>
            Enter sticker information and upload the image.
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="grid w-full items-center gap-4">
            <div className="flex flex-col space-y-1.5">
              <Label htmlFor="name">Sticker Name</Label>
              <Input id="name" placeholder="Enter sticker name" {...register("name")} />
              {errors.name && <p className="text-red-500 text-sm">{errors.name.message}</p>}
            </div>

            <div className="flex flex-col space-y-1.5">
              <Label htmlFor="stickerStyleName">Sticker Style</Label>
              <select
                id="stickerStyleName"
                {...register("stickerStyleName")}
                className="border rounded px-3 py-2"
              >
                <option value="">Select a style</option>
                {styles.map((style) => (
                  <option key={style.id} value={style.id}>
                    {style.stickerStyleName}
                  </option>
                ))}
              </select>
              {errors.stickerStyleName && (
                <p className="text-red-500 text-sm">{errors.stickerStyleName.message}</p>
              )}
            </div>

            <div className="flex flex-col space-y-1.5">
              <Label htmlFor="StickerFile">Upload Sticker</Label>
              <Input id="StickerFile" type="file" {...register("StickerFile")} />
              {errors.StickerFile && (
                <p className="text-red-500 text-sm">{errors.StickerFile.message}</p>
              )}
            </div>
          </div>
          <DialogFooter>
            <Button type="submit" disabled={loading}>
              {loading ? "Saving..." : "Save Sticker"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
};

export default AddSticker;
