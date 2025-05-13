import React, { useState } from "react";
import { Button, Modal, Group, Text, LoadingOverlay } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import AxiosAPI from "@/configs/axios";
import { toast } from "react-toastify";
import { IconTrash } from "@tabler/icons-react";

interface DeleteDepositProductProps {
  id: number;
  onDeleteSuccess: () => void; // Callback khi xóa thành công
}

const DeleteDepositProduct = ({
  id,
  onDeleteSuccess,
}: DeleteDepositProductProps) => {
  const [opened, { open, close }] = useDisclosure(false);
  const [isDeleting, setIsDeleting] = useState(false);

  const handleDelete = async () => {
    try {
      setIsDeleting(true);
      const response = await AxiosAPI.delete(`/api/DepositProduct/${id}`);

      if (response.status === 204) {
        toast.success("Xóa sản phẩm thành công");
        onDeleteSuccess();
        close();
      } else {
        toast.error("Không thể xóa sản phẩm");
      }
    } catch (error) {
      toast.error("Xóa sản phẩm thất bại");
      console.error("Error deleting product:", error);
    } finally {
      setIsDeleting(false);
    }
  };

  return (
    <>
      <Button
        variant="outline"
        color="red"
        onClick={open}
        leftSection={<IconTrash size={16} />}
      >
        Xóa
      </Button>

      <Modal
        opened={opened}
        onClose={close}
        title="Xác nhận xóa sản phẩm"
        centered
      >
        <LoadingOverlay visible={isDeleting} />
        <Text mb="md">
          Bạn có chắc chắn muốn xóa sản phẩm này? Hành động này không thể hoàn
          tác.
        </Text>

        <Group justify="flex-end">
          <Button variant="default" onClick={close}>
            Hủy bỏ
          </Button>
          <Button color="red" onClick={handleDelete} loading={isDeleting}>
            Xác nhận xóa
          </Button>
        </Group>
      </Modal>
    </>
  );
};

export default DeleteDepositProduct;
