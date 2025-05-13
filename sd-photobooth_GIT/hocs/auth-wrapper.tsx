"use client";

import { signOut, useSession } from "next-auth/react";
import { useRouter, usePathname } from "next/navigation";
import { useEffect } from "react";
import Cookies from "js-cookie";
import Loading from "@/components/component/Loading";

const protectedRoutes: Record<string, string[]> = {
  "/dashboard/admin": ["Admin"],
  "/dashboard/manager": ["Manager"],
  "/dashboard/staff": ["Staff"],
};

const publicRoutes = new Set([
  "/", "/forget-password", "/reset-password",
  "/confirm-payment-payos", "/success", "/failed",
  "/privacy", "/about"
]);

function AuthWrapper({ children }: { children: React.ReactNode }) {
  const { data: session, status } = useSession();
  const router = useRouter();
  const pathname = usePathname();
  

  useEffect(() => {
    if (status === "loading") return;

    const accessToken = Cookies.get("AccessToken");

    
    if (publicRoutes.has(pathname)) return;

    
    if (!accessToken || !session) {
      signOut({ callbackUrl: "/" });
      return;
    }

   
    const matchedRoute = Object.keys(protectedRoutes).find((route) =>
      pathname.startsWith(route)
    );

    if (matchedRoute) {
      const allowedRoles = protectedRoutes[matchedRoute];
      const userRole = session.user?.role || "";

      if (!allowedRoles.includes(userRole)) {
        router.replace("/");
      }
    }
  }, [pathname, session, status, router]);

  if (status === "loading") return <Loading />;

  return <>{children}</>;
}

export default AuthWrapper;
