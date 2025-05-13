"use client";

import { useMemo, useState } from "react";
import { usePathname } from "next/navigation";
import Image from "next/image";
import Link from "next/link";
import {
  Sidebar,
  SidebarContent,
  SidebarGroup,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarRail,
} from "@/components/ui/sidebar";
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
  User,
  IdCard,
  History,
  Badge,
  Clock,
  ChevronUp,
  ChevronDown,
} from "lucide-react";

import fpt from "@/assets/tech-x.png";
import { useTranslations } from "next-intl";
import { getMenuItems } from "@/lib/menuItems";




export function AppSidebar(props: React.ComponentProps<typeof Sidebar>) {
  const t = useTranslations("AppSidebar");
 
  const pathname = usePathname();
  const menuItems = useMemo(() => getMenuItems(t), [t]);
  const currentPath = useMemo(
    () => Object.keys(menuItems).find((key) => pathname.startsWith(key)) || "/dashboard/staff",
    [pathname, menuItems]
  );
  const items = menuItems[currentPath];
  
  const [openMenus, setOpenMenus] = useState<Record<string, boolean>>({});

  const toggleMenu = (label: string) => {
    setOpenMenus((prev) => ({ ...prev, [label]: !prev[label] }));
  };

  return (
    <Sidebar {...props}>
      <SidebarHeader>
        <SidebarMenu>
          <SidebarMenuItem>
            <div className="flex justify-center items-center py-4 rounded-xl shadow-lg">
              <Image src={fpt} width={150} height={150} alt="logo" />
            </div>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarHeader>

      <SidebarContent className="bg-white dark:bg-black dark:text-white shadow-xl rounded-2xl">
        <SidebarGroup>
          <SidebarMenu>
            {items.map((item) => (
              <div key={item.label}>
                <SidebarMenuItem>
                  {item.link ? (
                    <SidebarMenuButton asChild>
                      <Link
                        href={item.link}
                        className={`flex items-center font-sans font-medium gap-2 px-4 py-3 w-full rounded-lg transition-colors ${pathname === item.link
                            ? "bg-gray-200 text-gray-900 dark:bg-gray-800 dark:text-white font-semibold"
                            : "hover:bg-gray-50 dark:hover:bg-gray-800 text-gray-900 dark:text-gray-300"
                          }`}
                      >
                        {item.icon && <item.icon size={30} />}
                        {item.label}
                      </Link>
                    </SidebarMenuButton>
                  ) : (
                    <button
                      className="flex items-center justify-between font-sans font-medium text-sm w-full px-4 py-2 rounded-lg text-gray-900 dark:text-gray-300"
                      onClick={() => toggleMenu(item.label)}
                    >
                      <div className="flex items-center gap-2">
                        {item.icon && <item.icon size={18} />}
                        {item.label}
                      </div>
                      {item.subMenu &&
                        (openMenus[item.label] ? (
                          <ChevronUp size={20} className="text-gray-500 dark:text-gray-400" />
                        ) : (
                          <ChevronDown size={20} className="text-gray-500 dark:text-gray-400" />
                        ))}
                    </button>
                  )}
                </SidebarMenuItem>

                {item.subMenu && openMenus[item.label] && (
                  <div className="ml-5 border-l my-1 border-gray-300 dark:border-gray-600 pl-3">
                    {item.subMenu.map((sub) => (
                      <SidebarMenuItem key={sub.label}>
                        <SidebarMenuButton asChild>
                          <Link
                            href={sub.link}
                            className={`flex items-center gap-2 font-sans font-semibold text-sm px-4 py-2 rounded-lg transition-colors ${pathname === sub.link
                                ? "bg-gray-200 text-gray-900 dark:bg-gray-800 dark:text-white"
                                : "text-gray-600 dark:text-gray-400 hover:bg-gray-50 dark:hover:bg-gray-800"
                              }`}
                          >
                            {sub.label}
                          </Link>
                        </SidebarMenuButton>
                      </SidebarMenuItem>
                    ))}
                  </div>
                )}
              </div>
            ))}
          </SidebarMenu>
        </SidebarGroup>
      </SidebarContent>

      <SidebarRail />
    </Sidebar>



  );
}
