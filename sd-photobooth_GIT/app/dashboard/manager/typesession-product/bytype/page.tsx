"use client";
import { useEffect, useState } from "react";
import AxiosAPI from "@/configs/axios";

import { TypeSession, TypeSessionProduct } from "@/types/type";
import { Card, Group, Loader, Select, SimpleGrid, Stack, Text, Title, Tooltip } from "@mantine/core";
const ByTypeSessionProduct = () => {
  const [type, setType] = useState<TypeSession[]>([]);
  const [selectedStyleId, setSelectedStyleId] = useState<number | null>(null);
  const [typep, setTypep] = useState<TypeSessionProduct[]>([]);
  const [loading, setLoading] = useState(false);
  useEffect(() => {
    const fetchStyles = async () => {
      try {
        const response = await AxiosAPI.get<TypeSession[]>("/api/TypeSession");
        setType(response.data || []);
      } catch (error) {
        console.error("Lỗi khi lấy danh sách sticker style:", error);
      }
    };

    fetchStyles();
  }, []);

  useEffect(() => {
    const fetchByType = async () => {
      if (!selectedStyleId) return;
      setLoading(true);
      try {
        const response = await AxiosAPI.get<TypeSessionProduct[]>(
          `/api/TypeSessionProduct/GetByTypeSessionId/${selectedStyleId}`
        );
        setTypep(response.data || []);
      } catch (error) {
        console.error("Lỗi khi lấy dữ liệu TypeSessionProduct:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchByType();
  }, [selectedStyleId]);

  return (
    <div className="p-8 max-w-6xl mx-auto">
      <Title order={3} className="mb-8 text-gray-800 font-semibold dark:text-gray-100">Chọn TypeSession</Title>

      <Select
        placeholder="Chọn một TypeSession"
        data={type.map((style) => ({
          value: style.id.toString(),
          label: style.name,
        }))}
        value={selectedStyleId?.toString() || null}
        onChange={(value) => setSelectedStyleId(Number(value))}
        className="mb-8 w-full max-w-md border  border-gray-300 dark:border-gray-600 bg-white dark:bg-[#1f1f1f] text-gray-900 dark:text-gray-100 placeholder:text-gray-400 dark:placeholder:text-gray-500 focus:border-blue-500 focus:ring-2 focus:ring-blue-200 dark:focus:ring-blue-400 rounded-lg transition"
        size="md"
      />

      {loading ? (
        <Group justify="center" mt="lg">
          <Loader />
        </Group>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
          {typep.map((item) => (
            <Card
              key={item.id}
              shadow="sm"
              padding="lg"
              radius="lg"
              withBorder
              className="transition-all hover:scale-[1.01] hover:shadow-md 
                       bg-white border border-gray-200 
                       dark:bg-[#1F1F1F] dark:border-[#333333] 
                       flex flex-col min-h-[280px]"
            >
              <div className="space-y-3 flex-1">
                <Group gap="sm" wrap="nowrap" align="flex-start">
                  <Text size="sm" className="min-w-[60px] text-gray-500 dark:text-gray-400">ID:</Text>
                  <Text size="sm" fw={500} className="break-all truncate text-gray-900 dark:text-gray-100">
                    {item.id}
                  </Text>
                </Group>

                <Group gap="sm" wrap="nowrap" align="flex-start">
                  <Text size="sm" className="min-w-[60px] text-gray-500 dark:text-gray-400">Tên:</Text>
                  <Text size="sm" fw={500} className="break-all line-clamp-2 text-gray-900 dark:text-gray-100">
                    {item.name}
                  </Text>
                </Group>

                <Group gap="sm" wrap="nowrap" align="flex-start">
                  <Text size="sm" className="min-w-[100px] text-gray-500 dark:text-gray-400">Product ID:</Text>
                  <Tooltip label={item.productId} withArrow>
                    <Text
                      size="sm"
                      fw={500}
                      className="break-all truncate hover:underline cursor-pointer text-blue-600 dark:text-blue-400"
                    >
                      {item.productId}
                    </Text>
                  </Tooltip>
                </Group>

                <Group gap="sm" wrap="nowrap" align="flex-start">
                  <Text size="sm" className="min-w-[80px] text-gray-500 dark:text-gray-400">Coupon ID:</Text>
                  <Text size="sm" fw={500} className="break-all text-gray-900 dark:text-gray-100">
                    {item.couponId}
                  </Text>
                </Group>

                <Group gap="sm" wrap="nowrap" align="flex-start">
                  <Text size="sm" className="min-w-[100px] text-gray-500 dark:text-gray-400">TypeSession ID:</Text>
                  <Text size="sm" fw={500} className="text-gray-900 dark:text-gray-100">{item.typeSessionId}</Text>
                </Group>
              </div>

              <Text size="xs" className="mt-4 pt-2 border-t border-gray-100 dark:border-[#333333] text-gray-500 dark:text-gray-400">
                Tạo lúc: {new Date(item.createdAt).toLocaleString()}
              </Text>
            </Card>

          ))}
        </div>
      )}
    </div>

  );
};

export default ByTypeSessionProduct;
