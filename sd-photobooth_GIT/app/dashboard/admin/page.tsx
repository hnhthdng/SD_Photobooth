"use client";
import ChartCard from "@/components/component/ChartCard";
import Image from "next/image";

import React from "react";

const AdminPage = () => {
  return (
    // <>
    //   <DashboardCard />

    //   <ChartCard />
    // </>
    <div className="relative w-full h-[500px]">
    <Image
      src="/admin.png"
      alt="admin"
      fill
      className="object-cover"
    />
  </div>
    
  );
};

export default AdminPage;


