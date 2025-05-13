export enum PaymentStatus {
  Pending,
  Success,
  Failed,
}

import { FaCheckCircle, FaTimesCircle, FaHourglassHalf } from "react-icons/fa";
import { IconType } from "react-icons";

type StatusMeta = {
  label: string;
  colorClass: string;
  icon: IconType;
};

export const PaymentStatusMeta: Record<PaymentStatus, StatusMeta> = {
  [PaymentStatus.Pending]: {
    label: "Pending",
    colorClass: "text-yellow-500",
    icon: FaHourglassHalf,
  },
  [PaymentStatus.Success]: {
    label: "Success",
    colorClass: "text-green-500",
    icon: FaCheckCircle,
  },
  [PaymentStatus.Failed]: {
    label: "Failed",
    colorClass: "text-red-500",
    icon: FaTimesCircle,
  },
};
