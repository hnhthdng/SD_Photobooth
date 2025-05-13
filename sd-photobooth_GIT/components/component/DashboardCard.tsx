"use client";
import {
    IconArrowDownRight,
    IconArrowUpRight,
    IconDiscount2,
    IconReceipt2,
    IconUserPlus,
} from "@tabler/icons-react";
import { Paper, Group, SimpleGrid, Text, Select, Badge, Skeleton } from "@mantine/core";
import { useEffect, useState } from "react";
import AxiosAPI from "@/configs/axios";

const icons = {
    user: IconUserPlus,
    revenue: IconReceipt2,
    order: IconDiscount2,
};

type DashboardData = [
    { totalOrder: number, totalOrderPrev: number },
    { totalUser: number, totalUserPrev: number },
    { totalRevenue: number, totalRevenuePrev: number },
];

const DashboardCard = () => {
    const [loading, setLoading] = useState(true);
    const [dateFilter, setDateFilter] = useState(2); 
    const [data, setData] = useState<DashboardData | null>(null);

    const fetchData = async () => {
        setLoading(true);
        try {
            const x = await Promise.all([
                (await AxiosAPI.post("/api/Dashboard/statictis-order", dateFilter)).data,
                (await AxiosAPI.post("/api/Dashboard/statictis-user", dateFilter)).data,
                (await AxiosAPI.post("/api/Dashboard/statictis-revenue", dateFilter)).data,
            ]) as unknown as DashboardData;

            setData(x);
        } catch (error) {
            console.error("Error fetching dashboard data:", error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchData();
    }, [dateFilter]);

    if (loading || !data) return <Skeleton  height={130} radius="md" />;

    

    const stats = [
        {
            title: "Total Orders",
            icon: "order",
            value: data[0].totalOrder,
            diff:
                data[0].totalOrderPrev > 0
                    ? ((data[0].totalOrder - data[0].totalOrderPrev) /
                         data[0].totalOrderPrev) *
                      100
                    : data[0].totalOrder === 0
                    ? 0
                    : 100,
        },
        {
            title: "Total Users",
            icon: "user",
            value: data[1].totalUser,
            diff:
                data[1].totalUserPrev > 0
                    ? ((data[1].totalUser - data[1].totalUserPrev) /
                         data[1].totalUserPrev) *
                      100
                    : data[1].totalUser === 0
                    ? 0
                    : 100,
        },
        {
            title: "Total Revenue",
            icon: "revenue",
            value: data[2].totalRevenue,
            diff:
                data[2].totalRevenuePrev > 0
                    ? ((data[2].totalRevenue - data[2].totalRevenuePrev) /
                         data[2].totalRevenuePrev) *
                      100
                    : data[2].totalRevenue === 0
                    ? 0
                    : 100,
        },
    ];
    
    return (
        <div className="pb-4">
        <Select
          value={dateFilter.toString()}
          onChange={(value) => setDateFilter(Number(value))}
          data={[
            { value: "0", label: "Ngày" },
            { value: "1", label: "Tuần" },
            { value: "2", label: "Tháng" },
            { value: "3", label: "Năm" },
          ]}
          className="mb-4 max-w-xs"
          placeholder="Chọn thời gian"
        />
  
        <SimpleGrid cols={{ base: 1, sm: 2, md: 3 }} spacing="lg">
          {(loading ? Array(3).fill(null) : stats).map((stat, index) => {
            if (loading || !stat) {
              return (
                <Skeleton key={index} height={130} radius="md" />
              );
            }
  
            const Icon = icons[stat.icon as keyof typeof icons];
            const DiffIcon = stat.diff >= 0 ? IconArrowUpRight : IconArrowDownRight;
  
            return (
              <Paper
                withBorder
                radius="lg"
                p="md"
                key={stat.title}
                className="shadow-sm bg-white dark:bg-dark-800 transition hover:shadow-md"
              >
                <Group justify="space-between" mb="xs">
                  <Text size="xs" className="text-gray-500 font-semibold uppercase tracking-wide">
                    {stat.title}
                  </Text>
                  <Icon size={22} stroke={1.5} className="text-gray-400" />
                </Group>
  
                <Group align="flex-end" gap="xs" mt="md">
                  <Text className="text-3xl font-bold">{stat.value}</Text>
                  <Badge
                    color={stat.diff >= 0 ? "teal" : "red"}
                    variant="light"
                    size="sm"
                    className=" min-w-[100px] h-[100px]"
                  >
                    {stat.diff.toFixed(2)}%
                    <DiffIcon size={14} />
                  </Badge>
                </Group>
  
                <Text size="xs" className="text-gray-400 mt-2">
                  Compared to previous period
                </Text>
              </Paper>
            );
          })}
        </SimpleGrid>
      </div>
    );
};

export default DashboardCard;
