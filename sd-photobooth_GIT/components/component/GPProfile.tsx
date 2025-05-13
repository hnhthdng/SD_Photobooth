"use client"

import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import AxiosAPI from "@/configs/axios"
import { Pencil } from "lucide-react"
import { useState } from "react"
import { toast } from "react-toastify"

const GPProfile: React.FC<{ onUpdateSuccess: () => void }> = ({ onUpdateSuccess }) => {
  const [form, setForm] = useState({
    fullName: "",
    email: "",
    phoneNumber: "",
    gender: 0,
    birthDate: new Date().toISOString().slice(0, 10),
  })
  const [loading, setLoading] = useState(false)
  const [open, setOpen] = useState(false)

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value })
  }

  const handleGenderChange = (value: string) => {
    setForm({ ...form, gender: Number(value) })
  }

  const handleSubmit = async () => {
    try {
      setLoading(true)
      await AxiosAPI.patch("/api/Identity/update-profile", form)
      toast.success("Cập nhật thông tin thành công")
      onUpdateSuccess()
      setOpen(false)
    } catch (err) {
      console.error(err)
      toast.error("Có lỗi xảy ra khi cập nhật thông tin")
    } finally {
      setLoading(false)
    }
  }

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button variant="outline" size="lg" className="flex items-center">
          <Pencil className="w-10 h-10" />
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle>Edit Profile</DialogTitle>
          <DialogDescription>
            Cập nhật thông tin cá nhân tại đây.
          </DialogDescription>
        </DialogHeader>

        <div className="grid gap-4 py-4">
          <div className="grid gap-2">
            <Label htmlFor="fullName">Full Name</Label>
            <Input name="fullName" value={form.fullName} onChange={handleChange} />
          </div>
          <div className="grid gap-2">
            <Label htmlFor="email">Email</Label>
            <Input name="email" value={form.email} onChange={handleChange} />
          </div>
          <div className="grid gap-2">
            <Label htmlFor="phoneNumber">Phone Number</Label>
            <Input name="phoneNumber" value={form.phoneNumber} onChange={handleChange} />
          </div>
          <div className="grid gap-2">
            <Label htmlFor="gender">Gender</Label>
            <Select value={form.gender.toString()} onValueChange={handleGenderChange}>
              <SelectTrigger>
                <SelectValue placeholder="Chọn giới tính" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="0">Nam</SelectItem>
                <SelectItem value="1">Nữ</SelectItem>
                <SelectItem value="2">Khác</SelectItem>
              </SelectContent>
            </Select>
          </div>
          <div className="grid gap-2">
            <Label htmlFor="birthDate">Birth Date</Label>
            <Input
              name="birthDate"
              type="date"
              value={form.birthDate}
              onChange={handleChange}
            />
          </div>
        </div>

        <DialogFooter>
          <Button onClick={handleSubmit} disabled={loading}>
            {loading ? "Đang lưu..." : "Lưu thay đổi"}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}

export default GPProfile
