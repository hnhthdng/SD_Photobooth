import { IconType } from "react-icons";
import {
  FaHourglassHalf,
  FaCog,
  FaCheckCircle,
  FaTimesCircle,
  FaBan,
} from "react-icons/fa";

export enum OrderStatus {
  Pending,
  Processing,
  Completed,
  Failed,
  Cancelled,
}

type StatusMeta = {
  label: string;
  colorClass: string;
  icon: IconType;
};

export const OrderStatusMeta: Record<OrderStatus, StatusMeta> = {
  [OrderStatus.Pending]: {
    label: "Pending",
    colorClass: "text-yellow-500",
    icon: FaHourglassHalf,
  },
  [OrderStatus.Processing]: {
    label: "Processing",
    colorClass: "text-blue-500",
    icon: FaCog,
  },
  [OrderStatus.Completed]: {
    label: "Completed",
    colorClass: "text-green-500",
    icon: FaCheckCircle,
  },
  [OrderStatus.Failed]: {
    label: "Failed",
    colorClass: "text-red-500",
    icon: FaTimesCircle,
  },
  [OrderStatus.Cancelled]: {
    label: "Cancelled",
    colorClass: "text-gray-500",
    icon: FaBan,
  },
};
