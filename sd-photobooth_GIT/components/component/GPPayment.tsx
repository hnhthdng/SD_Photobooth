import { useEffect, useState } from "react"
import { Modal, Button, TextInput, Checkbox, Stack, Group, Text, LoadingOverlay } from "@mantine/core"
import { useDisclosure } from "@mantine/hooks"
import { PaymentMethod } from "@/types/type"
import AxiosAPI from "@/configs/axios"
import { toast } from "react-toastify"
import { BookUser } from "lucide-react"

const EditPaymentMethod = ({ id, onUpdated }: { id: number; onUpdated?: () => void }) => {
  const [opened, { open, close }] = useDisclosure(false)
  const [loading, setLoading] = useState(false)

  const [form, setForm] = useState({
    methodName: "",
    description: "",
    isActive: false,
    isOnline: false,
    forMobile: false,
  })

  useEffect(() => {
    if (!opened) return

    const fetchData = async () => {
      try {
        setLoading(true)
        const res = await AxiosAPI.get<PaymentMethod>(`/api/PaymentMethod/${id}`)
        const data = res.data
        setForm({
          methodName: data?.methodName || "",
          description: data?.description || "",
          isActive: data?.isActive ?? false,
          isOnline: data?.isOnline ?? false,
          forMobile: data?.forMobile ?? false,
        })
      } catch (err) {
        toast.error("Lỗi khi lấy thông tin phương thức")
        console.error(err)
      } finally {
        setLoading(false)
      }
    }

    fetchData()
  }, [opened, id])

  const handleUpdate = async () => {
    try {
      await AxiosAPI.put(`/api/PaymentMethod/${id}`, form)
      toast.success("Updated Successfully!")
      close()
      onUpdated?.()
    } catch (error) {
      toast.error("Cập nhật thất bại")
      console.error(error)
    }
  }

  return (
    <>
      <Button variant="subtle" onClick={open}>
        <BookUser size={18} />
      </Button>

      <Modal opened={opened} onClose={close} title="Sửa phương thức thanh toán" centered>
        <LoadingOverlay visible={loading} overlayProps={{  blur: 2 }} />
        {!loading && (
          <Stack gap="sm">
            <TextInput
              label="Tên phương thức"
              placeholder="Nhập tên phương thức"
              value={form.methodName}
              onChange={(e) => setForm({ ...form, methodName: e.currentTarget.value })}
            />
            <TextInput
              label="Mô tả"
              placeholder="Nhập mô tả"
              value={form.description}
              onChange={(e) => setForm({ ...form, description: e.currentTarget.value })}
            />
            <Checkbox
              label="Kích hoạt"
              checked={form.isActive}
              onChange={(e) => setForm({ ...form, isActive: e.currentTarget.checked })}
            />
            <Checkbox
              label="Trực tuyến"
              checked={form.isOnline}
              onChange={(e) => setForm({ ...form, isOnline: e.currentTarget.checked })}
            />
            <Checkbox
              label="Dành cho di động"
              checked={form.forMobile}
              onChange={(e) => setForm({ ...form, forMobile: e.currentTarget.checked })}
            />
            <Group  mt="md">
              <Button onClick={handleUpdate}>Cập nhật</Button>
            </Group>
          </Stack>
        )}
      </Modal>
    </>
  )
}

export default EditPaymentMethod
