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
import { BellRing, Smartphone, Wifi } from "lucide-react";
import { Switch } from "@/components/ui/switch";
import AxiosAPI from "@/configs/axios";
import { toast } from "react-toastify";
import { CirclePlus } from 'lucide-react';

const paymentSchema = z.object({
    methodName: z.string().min(1, "Method Name is required").max(100, "Max length is 100 characters"),
    description: z.string().max(500, "Max length is 500 characters").nullable().optional(),
    isActive: z.boolean(),
    isOnline: z.boolean().optional(),
    forMobile: z.boolean().optional(),
});

type PaymentFormData = z.infer<typeof paymentSchema>;

interface AddPaymentProps {
    onAddSuccess: () => void;
}

const AddPayment: React.FC<AddPaymentProps> = ({ onAddSuccess }) => {
    const [isOpen, setIsOpen] = useState(false);
    const [loading, setLoading] = useState(false);

    const {
        register,
        handleSubmit,
        setValue,
        watch,
        formState: { errors },
        reset,
    } = useForm<PaymentFormData>({
        resolver: zodResolver(paymentSchema),
        mode: "onChange",
        defaultValues: {
            methodName: "",
            description: "",
            isActive: true,
            isOnline: true,
            forMobile: true,
        },
    });

    const isActive = watch("isActive");
    const isOnline = watch("isOnline");
    const forMobile = watch("forMobile");

    const onSubmit = async (data: PaymentFormData) => {
        if (loading) return;
        setLoading(true);

        try {
            await AxiosAPI.post("api/PaymentMethod", data);
            toast.success("Payment method added successfully!");
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
                <Button variant="outline"><CirclePlus /></Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Add Payment Method</DialogTitle>
                    <DialogDescription>
                        Fill in the details and click save when you're done.
                    </DialogDescription>
                </DialogHeader>
                <form onSubmit={handleSubmit(onSubmit)}>
                    <div className="grid w-full items-center gap-4">
                        <div className="flex flex-col space-y-1.5">
                            <Label htmlFor="methodName">Method Name</Label>
                            <Input id="methodName" placeholder="Enter method name" {...register("methodName")} />
                            {errors.methodName && (
                                <p className="text-sm text-red-500">{errors.methodName.message}</p>
                            )}
                        </div>
                        <div className="flex flex-col space-y-1.5">
                            <Label htmlFor="description">Description</Label>
                            <Input id="description" placeholder="Enter description" {...register("description")} />
                            {errors.description && (
                                <p className="text-sm text-red-500">{errors.description.message}</p>
                            )}
                        </div>

                        {/* isActive */}
                        <div className="flex items-center space-x-4 rounded-md border p-4">
                            <BellRing />
                            <div className="flex-1 space-y-1">
                                <p className="text-sm font-medium leading-none">Status</p>
                                <p className="text-sm text-muted-foreground">
                                    Enable or disable this payment method.
                                </p>
                            </div>
                            <Switch
                                id="isActive"
                                checked={isActive}
                                onCheckedChange={(checked) => setValue("isActive", checked)}
                            />
                        </div>

                        {/* isOnline */}
                        <div className="flex items-center space-x-4 rounded-md border p-4">
                            <Wifi />
                            <div className="flex-1 space-y-1">
                                <p className="text-sm font-medium leading-none">Online Payment</p>
                                <p className="text-sm text-muted-foreground">
                                    This method supports online transactions.
                                </p>
                            </div>
                            <Switch
                                id="isOnline"
                                checked={isOnline}
                                onCheckedChange={(checked) => setValue("isOnline", checked)}
                            />
                        </div>

                        {/* forMobile */}
                        <div className="flex items-center space-x-4 rounded-md border p-4">
                            <Smartphone />
                            <div className="flex-1 space-y-1">
                                <p className="text-sm font-medium leading-none">Mobile Support</p>
                                <p className="text-sm text-muted-foreground">
                                    Available on mobile platform.
                                </p>
                            </div>
                            <Switch
                                id="forMobile"
                                checked={forMobile}
                                onCheckedChange={(checked) => setValue("forMobile", checked)}
                            />
                        </div>
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

export default AddPayment;
