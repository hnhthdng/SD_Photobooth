"use client";
import React, { useEffect, useState } from "react";
import AxiosAPI from "@/configs/axios";
import { FrameStyle, Frame } from "@/types/type";
import { Indicator, SimpleGrid } from "@mantine/core";
import Image from "next/image";
const ByFrame = () => {
  const [frameStyles, setFrameStyles] = useState<FrameStyle[]>([]);
  const [selectedStyleId, setSelectedStyleId] = useState<number | null>(null);
  const [frames, setFrames] = useState<Frame[]>([]);

  useEffect(() => {
    const fetchStyles = async () => {
      try {
        const response = await AxiosAPI.get<FrameStyle[]>("/api/FrameStyle");
        setFrameStyles(response.data || []);
      } catch (error) {
        console.error("Lỗi khi lấy danh sách sticker style:", error);
      }
    };

    fetchStyles();
  }, []);

  // Fetch sticker by style when selectedStyleId changes
  useEffect(() => {
    const fetchFramesByStyle = async () => {
      if (!selectedStyleId) return;
      try {
        const response = await AxiosAPI.get<Frame[]>(
          `/api/Frame/frame-style/${selectedStyleId}`
        );
        setFrames(response.data || []);
      } catch (error) {
        console.error("Lỗi khi lấy sticker theo style:", error);
      }
    };

    fetchFramesByStyle();
  }, [selectedStyleId]);

  return (
    <div className="p-4 max-w-4xl mx-auto">
      <label className="block mb-2 font-medium">Chọn Frame Style:</label>
      <select
        className="w-full border border-gray-300 rounded p-2 mb-4"
        onChange={(e) => setSelectedStyleId(parseInt(e.target.value))}
        defaultValue=""
      >
        <option value="" disabled>
          -- Chọn một style --
        </option>
        {frameStyles.length > 0 ? (
          frameStyles.map((style) => (
            <option key={style.id} value={style.id}>
              {style.name}
            </option>
          ))
        ) : (
          <option value="" disabled>
            Không có dữ liệu
          </option>
        )}
      </select>

      {frames.length > 0 ? (
        <div>
          <h2 className="text-lg font-semibold mb-4">Danh sách Sticker</h2>
          <SimpleGrid cols={6} spacing="lg" verticalSpacing="sm">
            {frames.map((frame) => (
              <div key={frame.id} className="flex flex-col items-center">
                <Indicator inline label={frame.name} color="blue" size={16}>
                  <Image
                    alt={frame.name}
                    width={100}
                    height={100}
                    src={frame.frameUrl}
                    className="rounded-lg"
                  />
                </Indicator>
              </div>
            ))}
          </SimpleGrid>
        </div>
      ) : selectedStyleId ? (
        <p className="text-gray-500">Không có sticker nào cho style này</p>
      ) : null}
    </div>
  );
};

export default ByFrame;
