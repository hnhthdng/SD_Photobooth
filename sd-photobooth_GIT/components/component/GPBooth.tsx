import { useEffect, useState } from "react";
import {
  Modal,
  Button,
  TextInput,
  Stack,
  Group,
  LoadingOverlay,
  Checkbox,
  Select,
} from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { toast } from "react-toastify";
import { CiEdit } from "react-icons/ci";
import AxiosAPI from "@/configs/axios";
import { Booth, Location } from "@/types/type";

const UpdateBooth = ({
  id,
  onUpdateSuccess,
}: {
  id: number;
  onUpdateSuccess: () => void;
}) => {
  const [opened, { open, close }] = useDisclosure(false);
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState({
    boothName: "",
    description: "",
    status: true,
    locationName: "",
  });
  const [locations, setLocations] = useState<Location[]>([]);

  useEffect(() => {
    if (!opened) return;

    const fetchBoothAndLocations = async () => {
      try {
        setLoading(true);

        const [boothRes, locationsRes] = await Promise.all([
          AxiosAPI.get<Booth>(`/api/Booth/${id}`),
          AxiosAPI.get<Location[]>(`api/Location`),
        ]);

        const booth = boothRes.data;
        const allLocations = locationsRes.data as any;

        const matchedLocation: Location | undefined = allLocations.find(
          (loc: Location) => loc.id === booth?.location?.id
        );

        setLocations(allLocations);
        setFormData({
          boothName: booth?.boothName || "",
          description: booth?.description || "",
          status: booth?.status ?? true,
          locationName: matchedLocation?.locationName || "",
        });
      } catch (error) {
        toast.error("Không thể tải thông tin Booth hoặc Location");
        console.error(error);
      } finally {
        setLoading(false);
      }
    };

    fetchBoothAndLocations();
  }, [opened, id]);

  const handleChange = (
    field: keyof typeof formData,
    value: string | boolean
  ) => {
    setFormData((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handleSubmit = async () => {
    try {
      setLoading(true);
      const selectedLocation = locations.find(
        (loc) => loc.locationName === formData.locationName
      );
      if (!selectedLocation) {
        toast.error("Vui lòng chọn địa điểm hợp lệ");
        return;
      }

      const payload = {
        boothName: formData.boothName,
        description: formData.description,
        status: formData.status,
        locationId: selectedLocation.id,
      };

      await AxiosAPI.put(`/api/Booth/${id}`, payload);
      toast.success("Cập nhật Booth thành công!");
      close();
      onUpdateSuccess();
    } catch (error) {
      console.error("Update error:", error);
      toast.error("Cập nhật Booth thất bại.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <Button variant="outline" onClick={open}>
        <CiEdit />
      </Button>

      <Modal opened={opened} onClose={close} title="Chỉnh sửa Booth" centered>
        <LoadingOverlay visible={loading} overlayProps={{ blur: 2 }} />
        <Stack gap="sm">
          <TextInput
            label="Tên Booth"
            value={formData.boothName}
            onChange={(e) => handleChange("boothName", e.currentTarget.value)}
            required
          />
          <TextInput
            label="Mô tả"
            value={formData.description}
            onChange={(e) => handleChange("description", e.currentTarget.value)}
          />
          <Select
            label="Địa điểm"
            placeholder="Chọn địa điểm"
            data={locations.map((loc) => ({
              value: loc.locationName,
              label: loc.locationName,
            }))}
            value={formData.locationName}
            onChange={(value) => handleChange("locationName", value || "")}
            required
          />
          <Checkbox
            label="Kích hoạt"
            checked={formData.status}
            onChange={(e) => handleChange("status", e.currentTarget.checked)}
          />
          <Group justify="flex-end" mt="md">
            <Button variant="outline" onClick={close}>
              Hủy
            </Button>
            <Button onClick={handleSubmit} loading={loading}>
              Cập nhật
            </Button>
          </Group>
        </Stack>
      </Modal>
    </>
  );
};

export default UpdateBooth;
