import { useState } from "react";
import {
  Button,
  Modal,
  TextInput,
  Stack,
  Group,
  LoadingOverlay,
} from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { toast } from "react-toastify";
import AxiosAPI from "@/configs/axios";

const GPUpgradeLevel = ({ onUpdateSuccess }: { onUpdateSuccess: () => void }) => {
  const [opened, { open, close }] = useDisclosure(false);
  const [loading, setLoading] = useState(false);
  const [email, setEmail] = useState("");

  const handleClose = () => {
    setEmail("");
    close();
  };

  const handleSubmit = async () => {
    const trimmedEmail = email.trim();

    if (!trimmedEmail) {
      toast.error("Vui lòng nhập email");
      return;
    }

    try {
      setLoading(true);
      await AxiosAPI.put("/api/MembershipCard/upgrade-level", {
        email: trimmedEmail,
      });
      toast.success("Nâng cấp thành viên thành công!");
      handleClose();
      onUpdateSuccess();
    } catch (err: any) {
      const errorMessage =
        err.response?.data?.message || err.message || "Lỗi khi nâng cấp thành viên";
      toast.error(errorMessage);
      console.error("Upgrade error:", err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <Button onClick={open}>Nâng cấp thành viên</Button>

      <Modal opened={opened} onClose={handleClose} title="Nâng cấp thành viên" centered>
        <LoadingOverlay visible={loading} overlayProps={{ blur: 2 }} />
        <Stack>
          <TextInput
            label="Email khách hàng"
            placeholder="Nhập email người dùng"
            value={email}
            onChange={(e) => setEmail(e.currentTarget.value)}
            required
          />
          <Group justify="flex-end" mt="md">
            <Button variant="outline" onClick={handleClose} disabled={loading}>
              Hủy
            </Button>
            <Button
              onClick={handleSubmit}
              loading={loading}
              disabled={loading || !email.trim()}
            >
              Xác nhận nâng cấp
            </Button>
          </Group>
        </Stack>
      </Modal>
    </>
  );
};

export default GPUpgradeLevel;
