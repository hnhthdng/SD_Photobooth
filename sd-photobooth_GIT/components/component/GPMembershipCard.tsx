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
  Select,
} from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import AxiosAPI from "@/configs/axios";
import { toast } from "react-toastify";
import { BookUser } from "lucide-react";
import { LevelMembership, MembershipCard } from "@/types/type";

const EditMembershipCard = ({ id, onUpdated }: { id: number; onUpdated?: () => void }) => {
  const [opened, { open, close }] = useDisclosure(false);
  const [loading, setLoading] = useState(false);
  const [levels, setLevels] = useState<LevelMembership[]>([]);
  const [form, setForm] = useState({
    levelMemberShipId: 0,
    description: "",
    points: 0,
    isActive: false,
  });

  // Tải danh sách cấp độ khi component mount
  useEffect(() => {
    const fetchLevels = async () => {
      try {
        const res = await AxiosAPI.get<LevelMembership[]>("/api/LevelMemberShip");
        setLevels(res.data as any);
      } catch (err) {
        toast.error("Không thể tải danh sách cấp độ");
        console.error(err);
      }
    };

    fetchLevels();
  }, []);

  // Tải dữ liệu thẻ thành viên khi modal mở
  useEffect(() => {
    if (!opened) return;

    const fetchData = async () => {
      try {
        setLoading(true);
        const res = await AxiosAPI.get<MembershipCard>(`/api/MembershipCard/${id}`);
        const data = res.data;
        
        setForm({
          levelMemberShipId: data?.levelMemberShip?.id ?? 0,
          description: data?.description ?? "",
          points: data?.points ?? 0,
          isActive: data?.isActive ?? true,
        });
      } catch (err) {
        toast.error("Lỗi khi lấy thông tin thẻ thành viên");
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [opened, id]);

  const handleUpdate = async () => {
    try {
      setLoading(true);
      await AxiosAPI.put(`/api/MembershipCard/${id}`, form);
      toast.success("Cập nhật thành công!");
      close();
      onUpdated?.();
    } catch (error) {
      toast.error("Cập nhật thất bại");
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  const handleFormChange = (field: keyof typeof form, value: any) => {
    setForm(prev => ({
      ...prev,
      [field]: typeof value === 'string' && field === 'levelMemberShipId' 
        ? parseInt(value || "0") 
        : value
    }));
  };

  return (
    <>
      <Button variant="subtle" onClick={open}>
        <BookUser size={18} />
      </Button>

      <Modal opened={opened} onClose={close} title="Sửa thẻ thành viên" centered>
        <LoadingOverlay visible={loading} overlayProps={{ blur: 2 }} />
        <Stack gap="sm">
          <Select
            label="Cấp độ thành viên"
            placeholder="Chọn cấp độ"
            data={levels.map((level) => ({
              value: level.id.toString(),
              label: level.name,
            }))}
            value={form.levelMemberShipId.toString()}
            onChange={(value) => handleFormChange('levelMemberShipId', value)}
          />
          <TextInput
            label="Mô tả"
            placeholder="Nhập mô tả"
            value={form.description}
            onChange={(e) => handleFormChange('description', e.currentTarget.value)}
          />
          <NumberInput
            label="Điểm"
            value={form.points}
            onChange={(val) => handleFormChange('points', val)}
          />
          <Checkbox
            label="Kích hoạt"
            checked={form.isActive}
            onChange={(e) => handleFormChange('isActive', e.currentTarget.checked)}
          />
          <Group mt="md" justify="flex-end">
            <Button variant="outline" onClick={close}>
              Hủy
            </Button>
            <Button onClick={handleUpdate} loading={loading}>
              Cập nhật
            </Button>
          </Group>
        </Stack>
      </Modal>
    </>
  );
};

export default EditMembershipCard;