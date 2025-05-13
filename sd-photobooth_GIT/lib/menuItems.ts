import {
  Gauge,
  CreditCard,
  Layers,
  ImageDown,
  Brush,
  StickyNote,
  Shapes,
  ListOrdered,
  UserCog,
  Camera,
  Settings,
  MapPin,
  Gift,
  ShoppingCart,
  Store,
  Calendar,
  Package,
  Boxes,
  Repeat,
  Badge,
  Clock,
} from "lucide-react";
type MenuItem = {
  label: string;
  link?: string;
  icon?: React.ElementType;
  subMenu?: { label: string; link: string }[];
};

type TFunction = (key: string) => string;

export function getMenuItems(t: TFunction): Record<string, MenuItem[]> {
  return {
    "/dashboard/admin": [
      { label: t("dashboard"), link: "/dashboard/admin", icon: Gauge },
      {
        label: t("paymentMethod"),
        link: "/dashboard/admin/payment",
        icon: CreditCard,
      },
      {
        label: t("levelMembership"),
        link: "/dashboard/admin/levelmembership",
        icon: Layers,
      },
      {
        label: t("frame"),
        icon: ImageDown,
        subMenu: [
          { label: t("listFrame"), link: "/dashboard/admin/frame/list" },
          {
            label: t("getByFrameStyle"),
            link: "/dashboard/admin/frame/byframe",
          },
        ],
      },
      {
        label: t("frameStyle"),
        link: "/dashboard/admin/frame-style",
        icon: Brush,
      },
      {
        label: t("sticker"),
        icon: StickyNote,
        subMenu: [
          { label: t("listSticker"), link: "/dashboard/admin/sticker/list" },
          {
            label: t("getByStickerStyle"),
            link: "/dashboard/admin/sticker/bysticker",
          },
        ],
      },
      {
        label: t("stickerStyle"),
        link: "/dashboard/admin/sticker-style",
        icon: Shapes,
      },
      {
        label: t("typeSession"),
        link: "/dashboard/admin/type",
        icon: ListOrdered,
      },
      {
        label: t("user"),
        icon: UserCog,
        subMenu: [
          { label: t("function"), link: "/dashboard/admin/user/function" },
          {
            label: t("accountManager"),
            link: "/dashboard/admin/user/account-manager",
          },
          {
            label: t("accountStaff"),
            link: "/dashboard/admin/user/account-staff",
          },
        ],
      },
      { label: t("photoStyle"), link: "/dashboard/admin/style", icon: Camera },
      {
        label: t("settings"),
        icon: Settings,
        subMenu: [
          { label: t("profile"), link: "/dashboard/admin/settings/profile" },
        ],
      },
    ],

    "/dashboard/manager": [
      { label: t("dashboard"), link: "/dashboard/manager", icon: Gauge },
      {
        label: t("location"),
        link: "/dashboard/manager/location",
        icon: MapPin,
      },
      { label: t("coupon"), link: "/dashboard/manager/coupon", icon: Gift },
      {
        label: t("order"),
        icon: ShoppingCart,
        link: "/dashboard/manager/order",
      },
      {
        label: t("booth"),
        icon: Store,
        subMenu: [
          {
            label: t("getBoothByLocation"),
            link: "/dashboard/manager/booth/bylocation",
          },
          { label: t("listBooth"), link: "/dashboard/manager/booth/list" },
        ],
      },
      {
        label: t("session"),
        link: "/dashboard/manager/session",
        icon: Calendar,
      },
      {
        label: t("payment"),
        link: "/dashboard/manager/payment",
        icon: CreditCard,
      },
      {
        label: t("depositProduct"),
        link: "/dashboard/manager/deposit-product",
        icon: Package,
      },
      {
        label: t("typeSessionProduct"),
        icon: Boxes,
        subMenu: [
          {
            label: t("typeSessionProductList"),
            link: "/dashboard/manager/typesession-product/list",
          },
          {
            label: t("getByCoupon"),
            link: "/dashboard/manager/typesession-product/bycoupon",
          },
          {
            label: t("getByTypeSession"),
            link: "/dashboard/manager/typesession-product/bytype",
          },
        ],
      },
      {
        label: t("transaction"),
        link: "/dashboard/manager/transaction",
        icon: Repeat,
      },
      {
        label: t("user"),
        icon: UserCog,
        subMenu: [
          {
            label: t("viewDetailCustomer"),
            link: "/dashboard/manager/user/detailc",
          },
          {
            label: t("accountCustomer"),
            link: "/dashboard/manager/user/account-customer",
          },
          {
            label: t("accountStaff"),
            link: "/dashboard/manager/user/account-staff",
          },
          {
            label: t("banUnbanCustomer"),
            link: "/dashboard/manager/user/func",
          },
        ],
      },
      {
        label: t("settings"),
        icon: Settings,
        subMenu: [
          { label: t("profile"), link: "/dashboard/manager/settings/profile" },
        ],
      },
    ],

    "/dashboard/staff": [
      { label: t("staffDashboard"), link: "/dashboard/staff", icon: Gauge },
      {
        label: t("sessionCode"),
        link: "/dashboard/staff/session",
        icon: Calendar,
      },
      {
        label: t("membershipCard"),
        link: "/dashboard/staff/membership",
        icon: Badge,
      },
      { label: t("photoHistory"), link: "/dashboard/staff/photo", icon: Clock },
      {
        label: t("user"),
        icon: UserCog,
        subMenu: [
          {
            label: t("viewDetailCustomer"),
            link: "/dashboard/staff/user/detailc",
          },
          {
            label: t("accountCustomer"),
            link: "/dashboard/staff/user/account-customer",
          },
        ],
      },
      { label: t("order"), icon: ShoppingCart, link: "/dashboard/staff/order" },
      {
        label: t("settings"),
        icon: Settings,
        subMenu: [
          { label: t("profile"), link: "/dashboard/staff/settings/profile" },
        ],
      },
    ],
  };
}
