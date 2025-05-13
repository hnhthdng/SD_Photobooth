import React from "react";
import { Button } from "@mantine/core";
import AxiosAPI from "@/configs/axios";
import { toast } from "react-toastify";

interface ExportButtonProps {
  endpoint: string;
  filename: string;
}

const ExportButton: React.FC<ExportButtonProps> = ({ endpoint, filename }) => {
  const handleExport = async () => {
    try {
      const response = await AxiosAPI.get(endpoint, {
        responseType: "blob",
      });

      const blob = new Blob([response.data as any], {
        type: "text/csv;charset=utf-8;",
      });
      const url = window.URL.createObjectURL(blob);

      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", filename);
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);

      toast.success("Export thành công!");
    } catch (error) {
      console.error("Export failed:", error);
      toast.error("Xuất file thất bại!");
    }
  };

  return (
    <Button onClick={handleExport} variant="filled" color="blue">
      Export to CSV
    </Button>
  );
};

export default ExportButton;
