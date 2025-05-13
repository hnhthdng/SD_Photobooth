import { useEffect, useState } from "react";
import {
  Modal,
  Button,
  TextInput,
  NumberInput,
  Stack,
  Group,
  LoadingOverlay,
  Checkbox,
  ColorInput,
  FileInput,
  Image,
} from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import AxiosAPI from "@/configs/axios";
import { toast } from "react-toastify";
import { CiEdit } from "react-icons/ci";
import { PhotoStyle } from "@/types/type";

const EditPhotoStyle = ({
  id,
  onUpdateSuccess,
}: {
  id: number;
  onUpdateSuccess: () => void;
}) => {
  const [opened, { open, close }] = useDisclosure(false);
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState<Partial<PhotoStyle & { imageFile?: File | null }>>({});
  const [previewImage, setPreviewImage] = useState<string | null>(null);

  useEffect(() => {
    if (!opened) return;

    const fetchPhotoStyle = async () => {
      try {
        setLoading(true);
        const res = await AxiosAPI.get<PhotoStyle>(`/api/PhotoStyle/${id}`);
        setFormData(res.data as any);
      } catch (err) {
        toast.error("Không thể tải thông tin Photo Style");
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchPhotoStyle();
  }, [opened, id]);

  const handleFileChange = (file: File | null) => {
    if (file) {
      const previewUrl = URL.createObjectURL(file);
      setPreviewImage(previewUrl);
      setFormData((prev) => ({ ...prev, imageFile: file }));
    } else {
      setPreviewImage(null);
      setFormData((prev) => ({ ...prev, imageFile: null }));
    }
  };

  const handleChange = <K extends keyof PhotoStyle>(
    field: K,
    value: PhotoStyle[K]
  ) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = async () => {
    try {
      setLoading(true);
      const formDataToSend = new FormData();

      if (formData.imageFile instanceof File) {
        formDataToSend.append("ImageUrl", formData.imageFile); // 👈 key đúng với backend
      }

      formDataToSend.append("Name", formData.name ?? "");
      formDataToSend.append("Description", formData.description ?? "");
      formDataToSend.append("Prompt", formData.prompt ?? "");
      formDataToSend.append("NegativePrompt", formData.negativePrompt ?? "");
      formDataToSend.append("Controlnets", formData.controlnets ?? "");
      formDataToSend.append("NumImagesPerGen", String(formData.numImagesPerGen ?? 1));
      formDataToSend.append("BackgroundColor", formData.backgroundColor ?? "#ffffff");
      formDataToSend.append("Height", String(formData.height ?? 0));
      formDataToSend.append("Width", String(formData.width ?? 0));
      formDataToSend.append("Mode", String(formData.mode ?? 0));
      formDataToSend.append("NumInferenceSteps", String(formData.numInferenceSteps ?? 0));
      formDataToSend.append("GuidanceScale", String(formData.guidanceScale ?? 0));
      formDataToSend.append("Strength", String(formData.strength ?? 0));
      formDataToSend.append("FaceImage", String(formData.faceImage ?? false));
      formDataToSend.append("BackgroundRemover", String(formData.backgroundRemover ?? false));

      await AxiosAPI.put(`/api/PhotoStyle/${id}`, formDataToSend, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });

      toast.success("Cập nhật Photo Style thành công!");
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

      <Modal opened={opened} onClose={close} title="Chỉnh sửa Photo Style" centered size="lg">
        <LoadingOverlay visible={loading} overlayProps={{ blur: 2 }} />
        <Stack gap="sm">
          {(previewImage || formData.imageUrl) && (
            <Image
              src={previewImage || formData.imageUrl!}
              height={200}
              fit="contain"
              alt="Current style image"
            />
          )}
          <FileInput
            placeholder="Nhập hình ảnh"
            label="Ảnh Style mới"
            accept="image/*"
            onChange={handleFileChange}
          />
          <TextInput
            label="Tên Style"
            value={formData.name || ""}
            onChange={(e) => handleChange("name", e.currentTarget.value)}
            required
          />
          <TextInput
            label="Mô tả"
            value={formData.description || ""}
            onChange={(e) => handleChange("description", e.currentTarget.value)}
          />
          <TextInput
            label="Prompt"
            value={formData.prompt || ""}
            onChange={(e) => handleChange("prompt", e.currentTarget.value)}
          />
          <TextInput
            label="Negative Prompt"
            value={formData.negativePrompt || ""}
            onChange={(e) => handleChange("negativePrompt", e.currentTarget.value)}
          />
          <TextInput
            label="Controlnets"
            value={formData.controlnets || ""}
            onChange={(e) => handleChange("controlnets", e.currentTarget.value)}
          />
          <NumberInput
            label="Số ảnh mỗi lần tạo"
            value={formData.numImagesPerGen || 0}
            onChange={(v) => handleChange("numImagesPerGen", Number(v))}
            min={1}
          />
          <ColorInput
            label="Màu nền"
            value={formData.backgroundColor || "#ffffff"}
            onChange={(v) => handleChange("backgroundColor", v)}
          />
          <Group grow>
            <NumberInput
              label="Chiều cao"
              value={formData.height || 0}
              onChange={(v) => handleChange("height", Number(v))}
              min={0}
            />
            <NumberInput
              label="Chiều rộng"
              value={formData.width || 0}
              onChange={(v) => handleChange("width", Number(v))}
              min={0}
            />
          </Group>
          <NumberInput
            label="Chế độ"
            value={formData.mode || 0}
            onChange={(v) => handleChange("mode", Number(v))}
          />
          <NumberInput
            label="Số bước inference"
            value={formData.numInferenceSteps || 0}
            onChange={(v) => handleChange("numInferenceSteps", Number(v))}
            min={0}
          />
          <NumberInput
            label="Guidance Scale"
            value={formData.guidanceScale || 0}
            onChange={(v) => handleChange("guidanceScale", Number(v))}
            min={0}
            step={0.1}
          />
          <NumberInput
            label="Strength"
            value={formData.strength || 0}
            onChange={(v) => handleChange("strength", Number(v))}
            min={0}
            max={1}
            step={0.1}
          />
          <Checkbox
            label="Ảnh khuôn mặt"
            checked={formData.faceImage || false}
            onChange={(e) => handleChange("faceImage", e.currentTarget.checked)}
          />
          <Checkbox
            label="Xóa nền"
            checked={formData.backgroundRemover || false}
            onChange={(e) => handleChange("backgroundRemover", e.currentTarget.checked)}
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

export default EditPhotoStyle;
