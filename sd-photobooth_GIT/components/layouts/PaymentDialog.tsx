import * as React from "react";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";

interface PaymentDialogProps {
  open: boolean;
  onClose: () => void;
  paymentLink: string | null;
}

const PaymentDialog: React.FC<PaymentDialogProps> = ({
  open,
  onClose,
  paymentLink,
}) => {
  return (
    <Dialog open={open} onOpenChange={onClose}>
      <DialogContent className="max-w-3xl">
        <DialogHeader>
          <DialogTitle>Payment Page</DialogTitle>
        </DialogHeader>
        <div className="w-full h-[600px] border rounded-md overflow-hidden">
          {paymentLink && (
            <iframe
              src={paymentLink}
              className="w-full h-full"
              title="Payment Page"
              frameBorder={0}
            />
          )}
        </div>
        <DialogFooter>
          <Button variant="outline" onClick={onClose}>
            Close
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

export default PaymentDialog;
