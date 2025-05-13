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
import { Pencil, Smartphone } from "lucide-react";
import { Coordinate, Frame, FrameStyle } from "@/types/type";

const frameSchema = z.object({
     name: z.string().min(1, "Name is required"),
     frameStyleId: z.string(),
     slotCount: z.coerce.number().min(1, "SlotCount must be at least 1"),
     forMobile: z.boolean().default(false),
     frameFile: z.any().optional(),
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

const UpdateFrame = ({ id, onUpdated }: { id: number; onUpdated?: () => void }) => {
     const [isOpen, setIsOpen] = useState(false);
     const [loading, setLoading] = useState(false);
     const [frameStyles, setFrameStyles] = useState<FrameStyle[]>([]);
     const [isInitialized, setIsInitialized] = useState(false);

     const {
          register,
          handleSubmit,
          setValue,
          watch,
          control,
          reset,
          formState: { errors },
     } = useForm<FrameFormData>({
          resolver: zodResolver(frameSchema),
          mode: "onChange",
          defaultValues: {
               name: "",
               frameStyleId: "",
               slotCount: 1,
               forMobile: false,
               coordinateDTOs: [{ x: 0, y: 0, width: 0, height: 0 }],
          },
     });

     const { fields, append, remove, replace } = useFieldArray({
          control,
          name: "coordinateDTOs",
     });

     const forMobile = watch("forMobile");

     const fetchData = async () => {
          try {
               setLoading(true);
               const [frameRes, stylesRes] = await Promise.all([
                    AxiosAPI.get<Frame>(`/api/Frame/${id}`),
                    AxiosAPI.get<FrameStyle[]>("/api/FrameStyle"),
               ]);

               const frameData = frameRes.data;
               setFrameStyles(stylesRes.data as FrameStyle[]);

               // Set form values
               reset({
                    name: frameData?.name || "",
                    frameStyleId: frameData?.frameStyleId ? frameData?.frameStyleId.toString() : "",
                    slotCount: frameData?.slotCount || 0,
                    forMobile: frameData?.forMobile ?? false,
                    coordinateDTOs: frameData?.coordinates?.length
                    ? frameData.coordinates.map(coord => ({
                         x: coord.x ?? 0,
                         y: coord.y ?? 0,
                         width: coord.width ?? 0,
                         height: coord.height ?? 0,
                    }))
                    : [{ x: 0, y: 0, width: 0, height: 0 }],
               });

               setIsInitialized(true);
          } catch (error) {
               console.error("Failed to fetch data", error);
               toast.error("Failed to load frame data");
          } finally {
               setLoading(false);
          }
     };

     useEffect(() => {
          if (isOpen) {
               fetchData();
          } else {
               setIsInitialized(false);
               reset();
          }
     }, [isOpen]);

     const onSubmit = async (data: FrameFormData) => {
          try {
               setLoading(true);

               const formData = new FormData();
               formData.append("Id", id.toString());
               formData.append("Name", data.name);
               formData.append("FrameStyleId", data.frameStyleId);
               formData.append("SlotCount", data.slotCount.toString());
               formData.append("ForMobile", String(data.forMobile));

               if (data.frameFile instanceof File) {
                    formData.append("FrameFile", data.frameFile);
               }

               data.coordinateDTOs.forEach((coord, index) => {
                    formData.append(`CoordinateDTOs[${index}].x`, coord.x.toString());
                    formData.append(`CoordinateDTOs[${index}].y`, coord.y.toString());
                    formData.append(`CoordinateDTOs[${index}].width`, coord.width.toString());
                    formData.append(`CoordinateDTOs[${index}].height`, coord.height.toString());
               });

               await AxiosAPI.put(`/api/Frame/${id}`, formData, {
                    headers: { "Content-Type": "multipart/form-data" },
               });

               toast.success("Cập nhật thành công!");
               setIsOpen(false);
               onUpdated?.();
          } catch (err: any) {
               console.error(err);
               toast.error(err?.response?.data?.message || "Update failed");
          } finally {
               setLoading(false);
          }
     };

     return (
          <Dialog open={isOpen} onOpenChange={setIsOpen}>
               <DialogTrigger asChild>
                    <Button variant="ghost" size="sm">
                         <Pencil className="w-4 h-4 mr-1" />
                         Edit
                    </Button>
               </DialogTrigger>
               <DialogContent className="sm:max-w-[600px] max-h-[80vh] overflow-y-auto">
                    <DialogHeader>
                         <DialogTitle>Update Frame</DialogTitle>
                         <DialogDescription>Modify frame details</DialogDescription>
                    </DialogHeader>
                    {loading && !isInitialized ? (
                         <div className="flex justify-center py-8">Loading...</div>
                    ) : (
                         <form onSubmit={handleSubmit(onSubmit)} noValidate>
                              <div className="grid gap-4">
                                   {/* Name */}
                                   <div className="flex flex-col space-y-1.5">
                                        <Label htmlFor="name">Name</Label>
                                        <Input id="name" {...register("name")} disabled={loading} />
                                        {errors.name && <p className="text-sm text-red-500">{errors.name.message}</p>}
                                   </div>

                                   {/* Frame Style */}
                                   <div className="flex flex-col space-y-1.5">
                                        <Label htmlFor="frameStyleId">Frame Style</Label>
                                        <select
                                             id="frameStyleId"
                                             {...register("frameStyleId")}
                                             className="border border-input bg-background px-3 py-2 rounded-md text-sm"
                                             disabled={loading}
                                        >
                                             <option value="">Select frame style</option>
                                             {frameStyles.map((style) => (
                                                  <option key={style.id} value={style.id}>
                                                       {style.name}
                                                  </option>
                                             ))}
                                        </select>
                                        {errors.frameStyleId && (
                                             <p className="text-sm text-red-500">{errors.frameStyleId.message}</p>
                                        )}
                                   </div>

                                   {/* Slot Count */}
                                   <div className="flex flex-col space-y-1.5">
                                        <Label htmlFor="slotCount">Slot Count</Label>
                                        <Input
                                             id="slotCount"
                                             type="number"
                                             min="1"
                                             {...register("slotCount", { valueAsNumber: true })}
                                             disabled={loading}
                                        />
                                        {errors.slotCount && (
                                             <p className="text-sm text-red-500">{errors.slotCount.message}</p>
                                        )}
                                   </div>

                                   {/* File Upload */}
                                   <div className="flex flex-col space-y-1.5">
                                        <Label htmlFor="frameFile">Update Image (Optional)</Label>
                                        <Input
                                             id="frameFile"
                                             type="file"
                                             accept="image/*"
                                             disabled={loading}
                                             onChange={(e) => {
                                                  const file = e.target.files?.[0];
                                                  if (file) setValue("frameFile", file);
                                             }}
                                        />
                                   </div>

                                   {/* Mobile Toggle */}
                                   <div className="flex items-center space-x-4 rounded-md border p-4">
                                        <Smartphone />
                                        <div className="flex-1 space-y-1">
                                             <p className="text-sm font-medium">Mobile Support</p>
                                             <p className="text-sm text-muted-foreground">Is this frame for mobile?</p>
                                        </div>
                                        <Switch
                                             id="forMobile"
                                             checked={forMobile}
                                             onCheckedChange={(checked) => setValue("forMobile", checked)}
                                             disabled={loading}
                                        />
                                   </div>

                                   {/* Coordinates */}
                                   <div className="grid gap-2">
                                        <Label>Coordinates</Label>
                                        {fields.map((field, index) => (
                                             <div key={field.id} className="flex items-start gap-4 border p-3 rounded-md">
                                                  <div className="space-y-1">
                                                       <Label>X</Label>
                                                       <Input
                                                            type="number"
                                                            {...register(`coordinateDTOs.${index}.x`, { valueAsNumber: true })}
                                                            disabled={loading}
                                                       />
                                                  </div>
                                                  <div className="space-y-1">
                                                       <Label>Y</Label>
                                                       <Input
                                                            type="number"
                                                            {...register(`coordinateDTOs.${index}.y`, { valueAsNumber: true })}
                                                            disabled={loading}
                                                       />
                                                  </div>
                                                  <div className="space-y-1">
                                                       <Label>Width</Label>
                                                       <Input
                                                            type="number"
                                                            min="1"
                                                            {...register(`coordinateDTOs.${index}.width`, { valueAsNumber: true })}
                                                            disabled={loading}
                                                       />
                                                  </div>
                                                  <div className="space-y-1">
                                                       <Label>Height</Label>
                                                       <Input
                                                            type="number"
                                                            min="1"
                                                            {...register(`coordinateDTOs.${index}.height`, { valueAsNumber: true })}
                                                            disabled={loading}
                                                       />
                                                  </div>
                                                  <Button
                                                       type="button"
                                                       variant="destructive"
                                                       onClick={() => remove(index)}
                                                       className="mt-6"
                                                       disabled={fields.length <= 1 || loading}
                                                  >
                                                       -
                                                  </Button>
                                             </div>
                                        ))}
                                        <Button
                                             type="button"
                                             variant="outline"
                                             onClick={() => append({ x: 0, y: 0, width:0 , height: 0 })}
                                             disabled={loading}
                                        >
                                             Add Coordinate
                                        </Button>
                                        {errors.coordinateDTOs?.root && (
                                             <p className="text-sm text-red-500">{errors.coordinateDTOs.root.message}</p>
                                        )}
                                   </div>
                              </div>
                              <DialogFooter>
                                   <Button type="submit" disabled={loading }>
                                        {loading ? "Saving..." : "Update Frame"}
                                   </Button>
                              </DialogFooter>
                         </form>
                    )}
               </DialogContent>
          </Dialog>
     );
};

export default UpdateFrame;