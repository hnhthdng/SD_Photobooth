"use client";
import { useEffect, useState } from "react";
import { useForm, useFieldArray } from "react-hook-form";
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
import { Switch } from "@/components/ui/switch";
import AxiosAPI from "@/configs/axios";
import { toast } from "react-toastify";
import { Smartphone, PlusCircle } from "lucide-react";
import { FrameStyle } from "@/types/type";

const frameSchema = z.object({
  name: z.string().min(1, "Name is required"),
  frameStyleId: z.string().min(1, "FrameStyleId is required"),
  slotCount: z.string().min(1, "SlotCount is required"),
  forMobile: z.boolean().optional(),
  frameFile: z
    .any()
    .refine((file) => file instanceof File, "Frame file is required"),
  coordinateDTOs: z
    .array(
      z.object({
        x: z.number(),
        y: z.number(),
        width: z.number(),
        height: z.number(),
      })
    )
    .min(1, "At least one coordinate is required"),
});

type FrameFormData = z.infer<typeof frameSchema>;

const AddFrame = ({ onSuccess }: { onSuccess: () => void }) => {
  const [isOpen, setIsOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const [frameStyles, setFrameStyles] = useState<FrameStyle[]>([]);
  const [loadingStyles, setLoadingStyles] = useState(true);

  useEffect(() => {
    const fetchFrameStyles = async () => {
      try {
        const res = await AxiosAPI.get("/api/FrameStyle");
        setFrameStyles(res.data as any);
      } catch (error) {
        console.error("Failed to fetch frame styles", error);
      } finally {
        setLoadingStyles(false);
      }
    };

    fetchFrameStyles();
  }, []);

  const {
    register,
    handleSubmit,
    setValue,
    watch,
    control,
    formState: { errors },
    reset,
  } = useForm<FrameFormData>({
    resolver: zodResolver(frameSchema),
    mode: "onChange",
    defaultValues: {
      name: "",
      frameStyleId: "",
      slotCount: "",
      forMobile: false,
      coordinateDTOs: [{ x: 0, y: 0, width: 0, height: 0 }],
    },
  });

  const { fields, append, remove } = useFieldArray({
    control,
    name: "coordinateDTOs",
  });

  const forMobile = watch("forMobile");

  const onSubmit = async (data: FrameFormData) => {
    const formData = new FormData();
    formData.append("Name", data.name);
    formData.append("FrameStyleId", data.frameStyleId);
    formData.append("SlotCount", data.slotCount);
    formData.append("ForMobile", String(data.forMobile || false));
    formData.append("FrameFile", data.frameFile);

    data.coordinateDTOs.forEach((c, i) => {
      formData.append(`CoordinateDTOs[${i}].x`, c.x.toString());
      formData.append(`CoordinateDTOs[${i}].y`, c.y.toString());
      formData.append(`CoordinateDTOs[${i}].width`, c.width.toString());
      formData.append(`CoordinateDTOs[${i}].height`, c.height.toString());
    });

    try {
      setLoading(true);
      await AxiosAPI.post("/api/Frame", formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });
      toast.success("Thêm thành công!");
      reset();
      setIsOpen(false);
      onSuccess();
    } catch (err: any) {
      console.error(err);
      toast.error(err?.response?.data?.message || "Error while adding frame");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={isOpen} onOpenChange={setIsOpen}>
      <DialogTrigger asChild>
        <Button variant="outline">
          <PlusCircle className="mr-2 h-4 w-4" /> Add Frame
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[600px] max-h-[80vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>Add Frame</DialogTitle>
          <DialogDescription>Upload frame info and image</DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="grid gap-4">
            <div className="flex flex-col space-y-1.5">
              <Label htmlFor="name">Name</Label>
              <Input id="name" {...register("name" as const)} />
              {errors.name && (
                <p className="text-sm text-red-500">{errors.name.message}</p>
              )}
            </div>

            <div className="flex flex-col space-y-1.5">
              <Label htmlFor="frameStyleId">Frame Style</Label>
              <select
                id="frameStyleId"
                {...register("frameStyleId" as const)}
                className="border border-input bg-background px-3 py-2 rounded-md text-sm"
                defaultValue=""
              >
                <option value="" disabled>
                  {loadingStyles ? "Loading..." : "Select frame style"}
                </option>
                {frameStyles.map((style) => (
                  <option key={style.id} value={style.id}>
                    {style.name}
                  </option>
                ))}
              </select>
              {errors.frameStyleId && (
                <p className="text-sm text-red-500">
                  {errors.frameStyleId.message}
                </p>
              )}
            </div>

            <div className="flex flex-col space-y-1.5">
              <Label htmlFor="slotCount">Slot Count</Label>
              <Input
                id="slotCount"
                type="number"
                {...register("slotCount" as const)}
              />
              {errors.slotCount && (
                <p className="text-sm text-red-500">
                  {errors.slotCount.message}
                </p>
              )}
            </div>

            <div className="flex flex-col space-y-1.5">
              <Label htmlFor="frameFile">Frame File</Label>
              <Input
                id="frameFile"
                type="file"
                accept="image/*"
                onChange={(e) => {
                  const file = e.target.files?.[0];
                  if (file) setValue("frameFile", file);
                }}
              />
              {errors.frameFile && (
                <p className="text-sm text-red-500">
                  {errors.frameFile.message}
                </p>
              )}
            </div>

            <div className="flex items-center space-x-4 rounded-md border p-4">
              <Smartphone />
              <div className="flex-1 space-y-1">
                <p className="text-sm font-medium">Mobile Support</p>
                <p className="text-sm text-muted-foreground">
                  Is this frame for mobile?
                </p>
              </div>
              <Switch
                id="forMobile"
                checked={forMobile}
                onCheckedChange={(checked) => setValue("forMobile", checked)}
              />
            </div>

            <div className="grid gap-2">
              <Label>Coordinates</Label>
              {fields.map((field, index) => (
                <div
                  key={field.id}
                  className="flex items-start gap-4 border p-3 rounded-md"
                >
                  <div className="space-y-1">
                    <Label htmlFor={`coordinate-${index}-x`}>X</Label>
                    <Input
                      id={`coordinate-${index}-x`}
                      type="number"
                      placeholder="x"
                      {...register(`coordinateDTOs.${index}.x`, {
                        valueAsNumber: true,
                      })}
                    />
                  </div>
                  <div className="space-y-1">
                    <Label htmlFor={`coordinate-${index}-y`}>Y</Label>
                    <Input
                      id={`coordinate-${index}-y`}
                      type="number"
                      placeholder="y"
                      {...register(`coordinateDTOs.${index}.y`, {
                        valueAsNumber: true,
                      })}
                    />
                  </div>
                  <div className="space-y-1">
                    <Label htmlFor={`coordinate-${index}-width`}>Width</Label>
                    <Input
                      id={`coordinate-${index}-width`}
                      type="number"
                      placeholder="width"
                      {...register(`coordinateDTOs.${index}.width`, {
                        valueAsNumber: true,
                      })}
                    />
                  </div>
                  <div className="space-y-1">
                    <Label htmlFor={`coordinate-${index}-height`}>Height</Label>
                    <Input
                      id={`coordinate-${index}-height`}
                      type="number"
                      placeholder="height"
                      {...register(`coordinateDTOs.${index}.height`, {
                        valueAsNumber: true,
                      })}
                    />
                  </div>
                  <Button
                    type="button"
                    variant="destructive"
                    onClick={() => remove(index)}
                    className="mt-6"
                  >
                    -
                  </Button>
                </div>
              ))}

              <Button
                type="button"
                variant="outline"
                onClick={() => append({ x: 0, y: 0, width: 0, height: 0 })}
              >
                Add Coordinate
              </Button>
              {errors.coordinateDTOs && (
                <p className="text-sm text-red-500">
                  {(errors.coordinateDTOs as any)?.message}
                </p>
              )}
            </div>
          </div>

          <DialogFooter>
            <Button type="submit" disabled={loading}>
              {loading ? "Saving..." : "Save Frame"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
};

export default AddFrame;
