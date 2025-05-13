import { useEffect, useState } from "react";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import AxiosAPI from "@/configs/axios";
import { toast } from "react-toastify";
import { FilePenLine } from "lucide-react";
import { FrameStyle } from "@/types/type";

const UpdateFrameStyle = ({ id, onUpdated }: { id: number; onUpdated?: () => void }) => {
  const [open, setOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const [form, setForm] = useState({
    name: "",
    description: "",
  });
  const [image, setImage] = useState<File | null>(null);

  // Fetch existing FrameStyle data
  useEffect(() => {
    if (!open) return;

    const fetchData = async () => {
      try {
        setLoading(true);
        const res = await AxiosAPI.get<FrameStyle>(`/api/FrameStyle/${id}`);
        const data = res.data;
        setForm({
          name: data?.name || "",
          description: data?.description || "",
        });
      } catch (err) {
        toast.error("Lỗi khi lấy thông tin FrameStyle");
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [open, id]);

  const handleUpdate = async () => {
    try {
      const formData = new FormData();
      formData.append("Name", form.name);
      formData.append("Description", form.description);
      if (image) {
        formData.append("Image", image);
      }

      await AxiosAPI.put(`/api/FrameStyle/${id}`, formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      toast.success("Cập nhật FrameStyle thành công!");
      setOpen(false);
      onUpdated?.();
    } catch (error) {
      toast.error("Cập nhật FrameStyle thất bại");
      console.error(error);
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button variant="outline">
          <FilePenLine />
        </Button>
      </DialogTrigger>
      <DialogContent role="dialog" aria-modal="false">
        <DialogHeader>
          <DialogTitle>Sửa FrameStyle</DialogTitle>
        </DialogHeader>

        {loading ? (
          <p className="text-muted-foreground text-sm">Đang tải dữ liệu...</p>
        ) : (
          <div className="space-y-4">
            <Input
              type="file"
              accept="image/*"
              onChange={(e) => setImage(e.target.files?.[0] || null)}
            />
            <Input
              placeholder="Tên FrameStyle"
              value={form.name}
              onChange={(e) => setForm({ ...form, name: e.target.value })}
            />
            <Input
              placeholder="Mô tả"
              value={form.description}
              onChange={(e) => setForm({ ...form, description: e.target.value })}
            />

            <Button onClick={handleUpdate} className="w-full mt-2">
              Cập nhật
            </Button>
          </div>
        )}
      </DialogContent>
    </Dialog>
  );
};

export default UpdateFrameStyle;
