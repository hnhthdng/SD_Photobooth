import { useEffect, useState } from "react";
import {
  Modal,
  Button,
  TextInput,
  NumberInput,
  Stack,
  Group,
  LoadingOverlay,
} from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { toast } from "react-toastify";
import { CiEdit } from "react-icons/ci";
import AxiosAPI from "@/configs/axios";
import { TypeSessionProduct } from "@/types/type";

const UpdateTypeSessionProduct = ({
  id,
  onUpdateSuccess,
}: {
  id: number;
  onUpdateSuccess: () => void;
}) => {
  const [opened, { open, close }] = useDisclosure(false);
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState({
    name: "",
    productId: "",
   
  });

  useEffect(() => {
    if (!opened) return;

    const fetchData = async () => {
      try {
        setLoading(true);
        const res = await AxiosAPI.get<TypeSessionProduct>(`/api/TypeSessionProduct/${id}`);
        const data = res.data;
        setFormData({
          name: data?.name || "",
          productId: data?.productId || "",
         
        });
      } catch (error) {
        console.error("Fetch error:", error);
        toast.error("Không thể tải thông tin Type Session Product");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [opened, id]);

  const handleChange = (field: keyof typeof formData, value: string | number) => {
    setFormData((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handleSubmit = async () => {
    try {
      setLoading(true);
      await AxiosAPI.put(`/api/TypeSessionProduct/${id}`, formData);
      toast.success("Cập nhật Type Session Product thành công!");
      close();
      onUpdateSuccess();
    } catch (error) {
      console.error("Update error:", error);
      toast.error("Cập nhật thất bại.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <Button variant="outline" onClick={open}>
        <CiEdit />
      </Button>

      <Modal opened={opened} onClose={close} title="Chỉnh sửa Type Session Product" centered>
        <LoadingOverlay visible={loading} overlayProps={{ blur: 2 }} />
        <Stack gap="sm">
          <TextInput
            label="Tên"
            value={formData.name}
            onChange={(e) => handleChange("name", e.currentTarget.value)}
            required
          />
          <TextInput
            label="Product ID"
            value={formData.productId}
            onChange={(e) => handleChange("productId", e.currentTarget.value)}
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

export default UpdateTypeSessionProduct;
