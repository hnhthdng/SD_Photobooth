import ChartCard from "@/components/component/ChartCard";

import React from "react";
import { useTranslations } from "next-intl";

const Staff = () => {
  const t = useTranslations("HomePage");

  return (
    <>
      <h1>{t("Dashboard")}</h1>

      {/* <DashboardCard /> */}

      <ChartCard />
    </>
  );
  // }
};

export default Staff;
