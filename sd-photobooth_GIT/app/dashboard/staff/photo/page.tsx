"use client";

import React, { useCallback, useState } from "react";
import AxiosAPI from "@/configs/axios";
import { Indicator, SimpleGrid } from "@mantine/core";
import Image from "next/image";
import { PhotoHistory } from "@/types/type";

const ByPhotoHistory = () => {
  const [inputId, setInputId] = useState<string>("");
  const [photos, setPhotos] = useState<PhotoHistory[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const fetchPhotoHistory = useCallback(async () => {
    if (!inputId.trim()) {
      setError("Please enter a valid ID");
      return;
    }

    const id = parseInt(inputId);
    if (isNaN(id)) {
      setError("ID must be a number");
      return;
    }

    setIsLoading(true);
    setError(null);

    try {
      const { data } = await AxiosAPI.get(`/api/PhotoHistory/manage/photo/${id}`);

      const processedData = Array.isArray(data)
        ? data.map((item, index) => ({
            ...item,
            id: item.id || `photo-${index}`,
            url: item.url?.replace(/\s+/g, ""),
          }))
        : [];

      setPhotos(processedData);
    } catch (err) {
      console.error("Failed to fetch photo history", err);
      setError("No photo history found for this ID");
      setPhotos([]);
    } finally {
      setIsLoading(false);
    }
  }, [inputId]);

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      fetchPhotoHistory();
    }
  };

  return (
    <div className="p-4 max-w-4xl mx-auto">
      <div className="flex gap-2 mb-4">
        <input
          type="text"
          className="flex-1 border border-gray-300 rounded p-2"
          placeholder="Enter ID"
          value={inputId}
          onChange={(e) => setInputId(e.target.value)}
          onKeyDown={handleKeyDown}
        />
        <button
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
          onClick={fetchPhotoHistory}
          disabled={isLoading}
        >
          {isLoading ? "Loading..." : "Search"}
        </button>
      </div>

      {error && <p className="text-red-500 mb-4">{error}</p>}

      {isLoading ? (
        <p className="text-gray-500">Loading...</p>
      ) : photos.length > 0 ? (
        <SimpleGrid cols={{ base: 2, sm: 4, md: 6 }} spacing="lg">
          {photos.map((photo) => (
            <div key={photo.id} className="flex flex-col items-center">
              <div className="relative w-[100px] h-[100px]">
                <Image
                  src={photo.url}
                  alt={`${photo.photoStyleName} style photo`}
                  fill
                  className="object-cover rounded-lg"
                  onError={(e) => {
                    const target = e.target as HTMLImageElement;
                    target.onerror = null;
                    target.src = "/placeholder-image.jpg";
                  }}
                />
              </div>
              <p className="mt-2 text-sm text-center">{photo.photoStyleName}</p>
            </div>
          ))}
        </SimpleGrid>
      ) : inputId && !error ? (
        <p className="text-gray-500">No Photo History found!</p>
      ) : null}
    </div>
  );
};

export default ByPhotoHistory;
