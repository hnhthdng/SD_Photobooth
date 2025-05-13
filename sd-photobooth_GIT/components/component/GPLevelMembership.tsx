import { useEffect, useState } from "react";
import {
  Modal,
  Button,
  TextInput,
  NumberInput,
  Checkbox,
  Stack,
  Group,
  LoadingOverlay,
} from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import AxiosAPI from "@/configs/axios";
import { toast } from "react-toastify";
import { BookUser } from "lucide-react";
import { LevelMembership } from "@/types/type"; // Đảm bảo đã định nghĩa type này

const EditLevelMembership = ({
  id,
  onUpdated,
}: {
  id: number;
  onUpdated?: () => void;
}) => {
  const [opened, { open, close }] = useDisclosure(false);
  const [loading, setLoading] = useState(false);

  const [form, setForm] = useState({
    name: "",
    description: "",
    point: 0,
    isActive: true,
    discountPercent: 0,
    maxDiscount: 0,
    minOrder: 0,
  });

  useEffect(() => {
    if (!opened) return;

    const fetchData = async () => {
      try {
        setLoading(true);
        const res = await AxiosAPI.get<LevelMembership>(`/api/LevelMembership/${id}`);
        const data = res.data;
        setForm({
          name: data?.name || "",
          description: data?.description || "",
          point: data?.point || 0,
          isActive: data?.isActive ?? true,
          discountPercent: data?.discountPercent || 0,
          maxDiscount: data?.maxDiscount || 0,
          minOrder: data?.minOrder || 0,
        });
      } catch (err) {
        toast.error("Lỗi khi tải thông tin hạng thành viên");
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [opened, id]);

  const handleUpdate = async () => {
    try {
      await AxiosAPI.put(`/api/LevelMembership/${id}`, form);
      toast.success("Cập nhật hạng thành viên thành công!");
      close();
      onUpdated?.();
    } catch (error) {
      toast.error("Cập nhật thất bại");
      console.error(error);
    }
  };

  return (
    <>
      <Button variant="subtle" onClick={open}>
        <BookUser size={18} />
      </Button>

      <Modal
        opened={opened}
        onClose={close}
        title="Sửa hạng thành viên"
        centered
      >
        <LoadingOverlay visible={loading} overlayProps={{ blur: 2 }} />
        {!loading && (
          <Stack gap="sm">
            <TextInput
              label="Tên hạng"
              placeholder="Nhập tên hạng"
              value={form.name}
              onChange={(e) =>
                setForm({ ...form, name: e.currentTarget.value })
              }
            />
            <TextInput
              label="Mô tả"
              placeholder="Nhập mô tả"
              value={form.description}
              onChange={(e) =>
                setForm({ ...form, description: e.currentTarget.value })
              }
            />
            <NumberInput
              label="Điểm yêu cầu"
              value={form.point}
              onChange={(value) => setForm({ ...form, point: typeof value === "number" ? value : 0 })}
              min={0}
              step={1}
            />
            <Checkbox
              label="Kích hoạt"
              checked={form.isActive}
              onChange={(e) =>
                setForm({ ...form, isActive: e.currentTarget.checked })
              }
            />
            <NumberInput
              label="Phần trăm giảm giá"
              value={form.discountPercent}
              onChange={(value) =>
                setForm({ ...form, discountPercent: typeof value === "number" ? value : 0 })
              }
              min={0}
              max={1}
              step={0.001}
              decimalScale={3}
            />
            <NumberInput
              label="Giảm giá tối đa"
              value={form.maxDiscount}
              onChange={(value) =>
                setForm({ ...form, maxDiscount: typeof value === "number" ? value : 0 })
              }
              min={0}
              decimalScale={5}
            />
            <NumberInput
              label="Giá trị đơn tối thiểu"
              value={form.minOrder}
              onChange={(value) =>
                setForm({ ...form, minOrder: typeof value === "number" ? value : 0})
              }
              min={0}
              decimalScale={0}
            />
            <Group mt="md">
              <Button onClick={handleUpdate}>Cập nhật</Button>
            </Group>
          </Stack>
        )}
      </Modal>
    </>
  );
};

export default EditLevelMembership;
