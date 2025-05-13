import { useEffect, useState } from "react"
import { Order } from "@/types/type"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import AxiosAPI from "@/configs/axios"
import { Loader2 } from "lucide-react"
import { FaEye } from "react-icons/fa"


const ViewDetailOrder = ({ id }: { id: number }) => {
  const [order, setOrder] = useState<Order | null>(null)
  const [open, setOpen] = useState(false)
  const [loading, setLoading] = useState(false)

  const fetchDetail = async () => {
    try {
      setLoading(true)
      const response = await AxiosAPI.get<Order>(`/api/Order/${id}`)
      setOrder(response.data)
    } catch (error) {
      console.error("Lỗi khi lấy dữ liệu chi tiết", error)
      setOrder(null)
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
        <Button variant="outline"><FaEye /></Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Chi tiết </DialogTitle>
        </DialogHeader>

        {loading ? (
          <div className="flex items-center space-x-2 text-sm text-muted-foreground">
            <Loader2 className="animate-spin h-4 w-4" /> <span>Đang tải...</span>
          </div>
        ) : order ? (
          <div className="space-y-2 text-sm">
            <p><strong>ID:</strong> {order.id}</p>
            <p><strong>code:</strong> {order.code}</p>
          
            <p><strong>couponCode:</strong> {order.couponCode}</p>
            <p><strong>Booth Name:</strong> {order.boothName}</p>
            <p><strong>phone:</strong> {order.phone}</p>
            <p><strong>sessionCode:</strong> {order.sessionCode}</p>
            <p><strong>Created At:</strong> {new Date(order.createdAt).toLocaleString()}</p>
          </div>
        ) : (
          <div className="text-sm text-red-500">Không thể tải dữ liệu chi tiết</div>
        )}
      </DialogContent>
    </Dialog>
  )
}

export default ViewDetailOrder
