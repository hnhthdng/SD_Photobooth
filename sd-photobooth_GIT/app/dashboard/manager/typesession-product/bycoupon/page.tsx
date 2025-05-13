"use client";

import { useEffect, useState } from "react";
import AxiosAPI from "@/configs/axios";
import { TypeSession, TypeSessionProduct } from "@/types/type";
import { Card, Group, SimpleGrid, Text, Tooltip } from "@mantine/core";

const ByCoupon = () => {
  const [typeSessions, setTypeSessions] = useState<TypeSession[]>([]);
  const [selectedTypeId, setSelectedTypeId] = useState<number | null>(null);
  const [couponCode, setCouponCode] = useState("");
  const [result, setResult] = useState<TypeSessionProduct[]>([]);

  useEffect(() => {
    const fetchTypeSessions = async () => {
      try {
        const response = await AxiosAPI.get<TypeSession[]>("/api/TypeSession");
        setTypeSessions(response.data || []);
      } catch (error) {
        console.error("Lỗi khi lấy danh sách TypeSession:", error);
      }
    };

    fetchTypeSessions();
  }, []);

  const handleFetch = async () => {
    if (!couponCode || !selectedTypeId) return;
    try {
      const response = await AxiosAPI.get<TypeSessionProduct[]>(
        `/api/TypeSessionProduct/coupon?coupon=${couponCode}&typeSessionId=${selectedTypeId}`
      );
      setResult(response.data || []);
    } catch (error) {
      console.error(
        "Lỗi khi lấy dữ liệu TypeSessionProduct theo coupon và typeSessionId:",
        error
      );
    }
  };

  return (
    <div className="p-6 max-w-4xl mx-auto space-y-6 bg-white rounded-2xl shadow-lg border border-gray-100">
      <div>
        <label className="block mb-1 text-gray-700 font-semibold text-sm">Mã Coupon</label>
        <input
          type="text"
          placeholder="Nhập mã coupon..."
          className="w-full border border-gray-300 rounded-xl px-4 py-2 text-gray-800 placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500"
          value={couponCode}
          onChange={(e) => setCouponCode(e.target.value)}
        />
      </div>

      <div>
        <label className="block mb-1 text-gray-700 font-semibold text-sm">Chọn Type Session</label>
        <select
          className="w-full border border-gray-300 rounded-xl px-4 py-2 text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-500"
          onChange={(e) => setSelectedTypeId(parseInt(e.target.value))}
          defaultValue=""
        >
          <option value="" disabled>
            -- Chọn một TypeSession --
          </option>
          {typeSessions.map((type) => (
            <option key={type.id} value={type.id}>
              {type.name}
            </option>
          ))}
        </select>
      </div>

      <div className="text-right">
        <button
          className="px-6 py-2 rounded-xl bg-black text-white font-medium hover:bg-gray-900 transition duration-200 shadow-sm"
          onClick={handleFetch}
        >
          Tìm kiếm
        </button>
      </div>

      {result.length > 0 && (
        <SimpleGrid
          cols={{ base: 1, sm: 2, md: 3 }}
          spacing="lg"
          pt="lg"
        >
          {result.map((item) => (
            <Card
              key={item.id}
              shadow="sm"
              padding="lg"
              radius="lg"
              withBorder
              className="transition-all hover:scale-[1.01] hover:shadow-md"
              mih={280}
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

              <Text size="xs" c="dimmed" mt="md" pt="sm" style={{ borderTop: "1px solid var(--mantine-color-border)" }} ta="right" >
                {new Date(item.createdAt).toLocaleString()}
              </Text>
            </Card>
          ))}
        </SimpleGrid>
      )}


    </div>

  );
};

export default ByCoupon;
