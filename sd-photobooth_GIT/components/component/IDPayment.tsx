import { useEffect, useState } from "react"
import { PaymentMethod } from "@/types/type"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import AxiosAPI from "@/configs/axios"
import { Loader2 } from "lucide-react"

const ViewDetail = ({ id }: { id: number }) => {
  const [payment, setPayment] = useState<PaymentMethod | null>(null)
  const [open, setOpen] = useState(false)
  const [loading, setLoading] = useState(false)

  const fetchDetail = async () => {
    try {
      setLoading(true)
      const response = await AxiosAPI.get<PaymentMethod>(`/api/PaymentMethod/${id}`)
      setPayment(response.data)
    } catch (error) {
      console.error("Lỗi khi lấy dữ liệu chi tiết", error)
      setPayment(null)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    if (open) fetchDetail()
  }, [open])

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button variant="outline">Xem</Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Chi tiết phương thức thanh toán</DialogTitle>
        </DialogHeader>

        {loading ? (
          <div className="flex items-center space-x-2 text-sm text-muted-foreground">
            <Loader2 className="animate-spin h-4 w-4" /> <span>Đang tải...</span>
          </div>
        ) : payment ? (
          <div className="space-y-2 text-sm">
            <p><strong>ID:</strong> {payment.id}</p>
            <p><strong>Method Name:</strong> {payment.methodName}</p>
            <p><strong>Description:</strong> {payment.description}</p>
            <p><strong>Is Active:</strong> {payment.isActive ? "Yes" : "No"}</p>
            <p><strong>Is Online:</strong> {payment.isOnline ? "Yes" : "No"}</p>
            <p><strong>For Mobile:</strong> {payment.forMobile ? "Yes" : "No"}</p>
            <p><strong>Created At:</strong> {new Date(payment.createdAt).toLocaleString()}</p>
          </div>
        ) : (
          <div className="text-sm text-red-500">Không thể tải dữ liệu chi tiết</div>
        )}
      </DialogContent>
    </Dialog>
  )
}

export default ViewDetail
