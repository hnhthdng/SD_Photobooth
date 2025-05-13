"use client";

import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { ScrollArea } from "@mantine/core";


import { Terminal } from "lucide-react";

interface CashOrderDialogProps {
  open: boolean;
  onClose: () => void;
  orderCode: string;
  sessionInfo: any;
}

export default function CashOrderDialog({
  open,
  onClose,
  orderCode,
  sessionInfo,
}: CashOrderDialogProps) {
  return (
    <Dialog open={open} onOpenChange={onClose}>
      <DialogContent className="max-w-2xl">
        <DialogHeader>
          <DialogTitle className="flex items-center gap-2">
            <Terminal className="h-5 w-5" />
            Cash Order Created
          </DialogTitle>
        </DialogHeader>
        <div className="text-sm space-y-2">
          <p>
            <strong>Order Code:</strong> {orderCode}
          </p>
          <div>
            <p className="font-medium mb-1">Session Info:</p>
            <ScrollArea className="h-64 w-full rounded-md border bg-muted p-3">
              <pre className="text-xs whitespace-pre-wrap">
                {JSON.stringify(sessionInfo, null, 2)}
              </pre>
            </ScrollArea>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}
