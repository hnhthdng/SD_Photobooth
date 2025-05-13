import React, { useState } from "react";
import { Modal, TextInput, Button, ActionIcon } from "@mantine/core";
import { IconTrash } from "@tabler/icons-react";
import { toast } from "react-toastify";
import AxiosAPI from "@/configs/axios";

interface AddCouponProps {
  onAddSuccess: () => void;
}

const DeleteCoupon: React.FC<AddCouponProps> = ({ onAddSuccess }) => {
  const [code, setCode] = useState("");
  const [loading, setLoading] = useState(false);
  const [opened, setOpened] = useState(false); // Trạng thái mở Modal

  const deleteCoupon = async () => {
    if (!code) {
      toast.error("Vui lòng nhập mã giảm giá cần xóa");
      return;
    }
    setLoading(true);
    try {
      await AxiosAPI.delete(`api/Coupon/${code}`);
      toast.success("Xóa mã giảm giá thành công");
      setCode("");
      setOpened(false); // Đóng modal sau khi xóa thành công
      onAddSuccess();
    } catch (err) {
      toast.error("Không thể xóa mã giảm giá");
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      {/* Nút mở modal */}
      <ActionIcon variant="outline" color="red" onClick={() => setOpened(true)}>
        <IconTrash size={20} />
      </ActionIcon>

      {/* Modal nhập mã giảm giá */}
      <Modal opened={opened} onClose={() => setOpened(false)} title="Xóa mã giảm giá">
        <TextInput
          label="Mã giảm giá"
          placeholder="Nhập mã giảm giá cần xóa"
          value={code}
          onChange={(e) => setCode(e.target.value)}
          disabled={loading}
        />
        <div className="flex justify-end mt-4 space-x-2">
          <Button variant="default" onClick={() => setOpened(false)} disabled={loading}>
            Hủy
          </Button>
          <Button color="red" onClick={deleteCoupon} loading={loading}>
            Xóa mã
          </Button>
        </div>
      </Modal>
    </>
  );
};

export default DeleteCoupon;
