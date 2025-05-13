import { useEffect, useState } from "react";
import {
  Modal,
  Button,
  TextInput,
  Stack,
  Group,
  LoadingOverlay,
} from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { toast } from "react-toastify";
import { CiEdit } from "react-icons/ci";
import AxiosAPI from "@/configs/axios";
import { Location } from "@/types/type";

interface UpdateLocationProps {
  id: number;
  onUpdateSuccess: () => void;
}

const UpdateLocation = ({ id, onUpdateSuccess }: UpdateLocationProps) => {
  const [opened, { open, close }] = useDisclosure(false);
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState({
    locationName: "",
    address: "",
  });

  useEffect(() => {
    if (!opened) return;

    const fetchLocation = async () => {
      try {
        setLoading(true);
        const res = await AxiosAPI.get<Location>(`/api/Location/${id}`);
        const data = res.data;
        setFormData({
          locationName: data?.locationName || "",
          address: data?.address || "",
        });
      } catch (error) {
        console.error("Fetch error:", error);
        toast.error("Không thể tải thông tin địa điểm");
      } finally {
        setLoading(false);
      }
    };

    fetchLocation();
  }, [opened, id]);

  const handleChange = (field: "locationName" | "address", value: string) => {
    setFormData((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handleSubmit = async () => {
    try {
      setLoading(true);
      await AxiosAPI.put(`/api/Location/${id}`, formData);
      toast.success("Cập nhật địa điểm thành công!");
      close();
      onUpdateSuccess();
    } catch (error) {
      console.error("Update error:", error);
      toast.error("Cập nhật địa điểm thất bại.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <Button variant="outline" onClick={open}>
        <CiEdit />
      </Button>

      <Modal opened={opened} onClose={close} title="Chỉnh sửa Địa điểm" centered>
        <LoadingOverlay visible={loading} overlayProps={{ blur: 2 }} />
        <Stack gap="sm">
          <TextInput
            label="Tên địa điểm"
            value={formData.locationName}
            onChange={(e) => handleChange("locationName", e.currentTarget.value)}
            required
          />
          <TextInput
            label="Địa chỉ"
            value={formData.address}
            onChange={(e) => handleChange("address", e.currentTarget.value)}
            required
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

export default UpdateLocation;
