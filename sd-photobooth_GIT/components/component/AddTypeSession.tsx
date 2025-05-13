"use client"

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
import { FileText } from "lucide-react";
import { Switch } from "@/components/ui/switch";
import AxiosAPI from "@/configs/axios";
import { toast } from "react-toastify";

const formSchema = z.object({
     name: z.string().min(1, "Name is required").max(100, "Max length is 100 characters"),
     description: z.string().max(500, "Max length is 500 characters").optional(),
     duration: z.coerce.number().min(0, "Duration must be a non-negative value"),
     price: z.coerce.number().min(0, "Price must be a positive value"),
     ableTakenNumber: z.coerce.number().min(0, "Max participants must be a non-negative value"),
     
     forMobile: z.boolean().optional(),
});

interface AddTypeSessionProps {
     onAddSuccess: () => void;
}

const AddTypeSession: React.FC<AddTypeSessionProps> = ({ onAddSuccess }) => {
     const [isOpen, setIsOpen] = useState(false);
     const [loading, setLoading] = useState(false);

     const {
          control,
          handleSubmit,
          reset,
          formState: { errors },
     } = useForm({
          resolver: zodResolver(formSchema),
          mode: "onChange",
          defaultValues: {
               name: "",
               description: "",
               duration: 0,
               price: 0,
               
               ableTakenNumber: 0,
               forMobile: true,
          },
     });

     const onSubmit = async (values: any) => {
          if (loading) return;
          setLoading(true);

          try {
               await AxiosAPI.post("api/TypeSession", values);
               toast.success("Thêm thành công!");
               reset();
               setIsOpen(false);
               onAddSuccess();
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
                    <Button variant="outline">Add Type Session</Button>
               </DialogTrigger>
               <DialogContent className="sm:max-w-[425px] max-h-[80vh] overflow-y-auto">
                    <DialogHeader>
                         <DialogTitle>Add Type Session</DialogTitle>
                         <DialogDescription>
                              Fill in the details and click save when you're done.
                         </DialogDescription>
                    </DialogHeader>
                    <form onSubmit={handleSubmit(onSubmit)} className="grid gap-4">
                         <div>
                              <Label htmlFor="name">Session Name</Label>
                              <Input id="name" placeholder="Enter session name" {...control.register("name")} />
                              {errors.name && <p className="text-red-500 text-sm">{errors.name.message}</p>}
                         </div>
                         <div>
                              <Label htmlFor="description">Description</Label>
                              <Input id="description" placeholder="Enter description" {...control.register("description")} />
                              {errors.description && <p className="text-red-500 text-sm">{errors.description.message}</p>}
                         </div>
                         <div>
                              <Label htmlFor="duration">Duration (minutes)</Label>
                              <Input id="duration" type="number" min="0" {...control.register("duration", { valueAsNumber: true })} />
                              {errors.duration && <p className="text-red-500 text-sm">{errors.duration.message}</p>}
                         </div>
                         <div>
                              <Label htmlFor="price">Price</Label>
                              <Controller
                                   control={control}
                                   name="price"
                                   render={({ field }) => (
                                        <NumericFormat
                                             id="price"
                                             thousandSeparator=","
                                             decimalSeparator="."
                                             allowNegative={false}
                                             decimalScale={0}
                                             fixedDecimalScale
                                             customInput={Input}
                                             value={field.value}
                                             onValueChange={(values) => field.onChange(values.floatValue)}
                                        />
                                   )}
                              />
                              {errors.price && <p className="text-red-500 text-sm">{errors.price.message}</p>}
                         </div>
                         <div>
                              <Label htmlFor="ableTakenNumber">Max Participants</Label>
                              <Input id="ableTakenNumber" type="number" min="0" {...control.register("ableTakenNumber", { valueAsNumber: true })} />
                              {errors.ableTakenNumber && <p className="text-red-500 text-sm">{errors.ableTakenNumber.message}</p>}
                         </div>

                        
                       

                        
                         <div className="flex items-center space-x-4 rounded-md border p-4">
                              <FileText />
                              <div className="flex-1 space-y-1">
                                   <p className="text-sm font-medium leading-none">Mobile Support</p>
                                   <p className="text-sm text-muted-foreground">Enable or disable mobile mode.</p>
                              </div>
                              <Controller
                                   name="forMobile"
                                   control={control}
                                   render={({ field }) => (
                                        <Switch checked={field.value} onCheckedChange={field.onChange} />
                                   )}
                              />
                         </div>

                         <DialogFooter>
                              <Button type="submit" disabled={loading}>
                                   {loading ? "Saving..." : "Save Changes"}
                              </Button>
                         </DialogFooter>
                    </form>
               </DialogContent>
          </Dialog>
     );
};

export default AddTypeSession;
