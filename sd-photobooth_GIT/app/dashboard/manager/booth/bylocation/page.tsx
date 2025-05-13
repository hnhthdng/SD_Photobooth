"use client";

import { useEffect, useState } from "react";
import AxiosAPI from "@/configs/axios";
import { Location, Booth } from "@/types/type";
import {
  Badge,
  Card,
  Group,
  SimpleGrid,
  Text,
  Select,
  Title,
  Center,
  Loader,
  Skeleton,
} from "@mantine/core";


const ByLocation = () => {
  const [locations, setLocations] = useState<Location[]>([]);
  const [selectedLocationId, setSelectedLocationId] = useState<string | null>(null);
  const [booths, setBooths] = useState<Booth[]>([]);
  const [isLoading, setIsLoading] = useState(false);


  useEffect(() => {
    const fetchLocations = async () => {
      try {
        const response = await AxiosAPI.get<Location[]>("/api/Location");
        setLocations(response.data || []);
      } catch (error) {
        console.error("Lỗi khi lấy danh sách địa điểm:", error);
      }
    };
    fetchLocations();
  }, []);


  useEffect(() => {
    const fetchBoothsByLocation = async () => {
      if (!selectedLocationId) return;
      setIsLoading(true);
      try {
        const response = await AxiosAPI.get<Booth[]>(
          `/api/Booth/by-location/${selectedLocationId}`
        );
        setBooths(response.data || []);
      } catch (error) {
        console.error("Lỗi khi lấy danh sách booth:", error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchBoothsByLocation();
  }, [selectedLocationId]);

  return (
    <div className="p-6 max-w-5xl pl-6">
      <Title order={2} mb="lg" className="text-gray-900 font-semibold ">
        Danh sách Booth theo địa điểm
      </Title>

      <Select
        // label="Chọn địa điểm"
        placeholder="Chọn một địa điểm"
        data={locations.map((loc) => ({
          label: loc.locationName,
          value: loc.id.toString(),
        }))}
        value={selectedLocationId}
        onChange={setSelectedLocationId}
        mb="xl"
        searchable
        nothingFoundMessage="Không có địa điểm"
        classNames={{
          root: "w-full",
          input: "rounded-lg border-gray-300 shadow-sm hover:shadow-md transition duration-150",
          dropdown: "rounded-md shadow-lg",
          // item: "hover:bg-gray-100 text-gray-800",
        }}
        styles={{ label: { fontWeight: 500, fontSize: "16px" } }}
      />

      {isLoading ? (
        <Center h={200}>
          <Skeleton height={40} width={200} radius="xl" animate />
        </Center>
      ) : booths.length === 0 ? (
        <Text c="dimmed" className="text-center mt-8">
          Không có booth nào cho địa điểm này.
        </Text>
      ) : (
        <SimpleGrid
          cols={{ base: 1, sm: 2, md: 3, lg: 4 }}
          spacing="lg"
          verticalSpacing="xl"
        >
          {booths.map((booth) => (
            <Card
              key={booth.id}
              shadow="sm"
              padding="lg"
              radius="xl"
              withBorder
              className="transition-shadow hover:shadow-md bg-white"
            >
              <Group justify="space-between" mb="sm">
                <Text fw={600} className="text-gray-900">
                  {booth.boothName}
                </Text>
                <Badge
                  color={booth.status ? "green" : "gray"}
                  variant="filled"
                  className="text-sm"
                >
                  {booth.status ? "Hoạt động" : "Không hoạt động"}
                </Badge>
              </Group>
              <Text size="xs" c="dimmed">
                Tạo lúc: {new Date(booth.createdAt).toLocaleString()}
              </Text>
            </Card>
          ))}
        </SimpleGrid>
      )}
    </div>

  );
};

export default ByLocation;
