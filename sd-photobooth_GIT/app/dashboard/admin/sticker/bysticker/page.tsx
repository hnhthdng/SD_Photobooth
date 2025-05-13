"use client";
import React, { useEffect, useState } from "react";
import AxiosAPI from "@/configs/axios";
import { StickerStyle, Sticker } from "@/types/type";
import { Indicator, SimpleGrid } from "@mantine/core";
import Image from "next/image";
const BySticker = () => {
  const [stickerStyles, setStickerStyles] = useState<StickerStyle[]>([]);
  const [selectedStyleId, setSelectedStyleId] = useState<number | null>(null);
  const [stickers, setStickers] = useState<Sticker[]>([]);

  useEffect(() => {
    const fetchStyles = async () => {
      try {
        const response = await AxiosAPI.get<StickerStyle[]>(
          "/api/StickerStyle/all"
        );
        setStickerStyles(response.data || []);
      } catch (error) {
        console.error("Lỗi khi lấy danh sách sticker style:", error);
      }
    };

    fetchStyles();
  }, []);

  // Fetch sticker by style when selectedStyleId changes
  useEffect(() => {
    const fetchStickersByStyle = async () => {
      if (!selectedStyleId) return;
      try {
        const response = await AxiosAPI.get<Sticker[]>(
          `/api/Sticker/by-style/${selectedStyleId}`
        );
        setStickers(response.data || []);
      } catch (error) {
        console.error("Lỗi khi lấy sticker theo style:", error);
      }
    };

    fetchStickersByStyle();
  }, [selectedStyleId]);

  return (
    <div className="p-4 max-w-4xl mx-auto">
      <label className="block mb-2 font-medium">Chọn Sticker Style:</label>
      <select
        className="w-full border border-gray-300 rounded p-2 mb-4"
        onChange={(e) => setSelectedStyleId(parseInt(e.target.value))}
        defaultValue=""
      >
        <option value="" disabled>
          -- Chọn một style --
        </option>
        {stickerStyles.length > 0 ? (
          stickerStyles.map((style) => (
            <option key={style.id} value={style.id}>
              {style.stickerStyleName}
            </option>
          ))
        ) : (
          <option value="" disabled>
            Không có dữ liệu
          </option>
        )}
      </select>

      {stickers.length > 0 ? (
        <div>
          <h2 className="text-lg font-semibold mb-4">Sticker Management</h2>
          <SimpleGrid cols={6} spacing="lg" verticalSpacing="sm">
            {stickers.map((sticker) => (
              <div key={sticker.id} className="flex flex-col items-center">
                <Indicator inline label={sticker.name} color="blue" size={16}>
                  <Image
                    alt={sticker.name}
                    width={100}
                    height={100}
                    src={sticker.stickerUrl}
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

export default BySticker;
