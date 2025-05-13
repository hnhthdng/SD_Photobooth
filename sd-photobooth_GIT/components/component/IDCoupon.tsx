
import { useEffect, useState } from "react"
import { Coupon } from "@/types/type"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import AxiosAPI from "@/configs/axios"
import { Loader2 } from "lucide-react"
import { FaEye } from "react-icons/fa"

const ViewDetailCoupon = ({ id }: { id: number }) => {
     const [coupon, setCoupon] = useState<Coupon | null>(null)
     const [open, setOpen] = useState(false)
     const [loading, setLoading] = useState(false)
   
     const fetchDetail = async () => {
       try {
         setLoading(true)
         const response = await AxiosAPI.get<Coupon>(`/api/Coupon/${id}`)
         setCoupon(response.data)
       } catch (error) {
         console.error("Lỗi khi lấy dữ liệu chi tiết", error)
         setCoupon(null)
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
          ) : coupon ? (
            <div className="space-y-2 text-sm">
              <p><strong>ID:</strong> {coupon.id}</p>
              <p><strong>Code:</strong> {coupon.code}</p>
              <p><strong>discount:</strong> {coupon.discount}</p>
              <p><strong>discountPercent:</strong> {coupon.discountPercent}</p>
             
             
              
              <p><strong>isActive:</strong> {coupon.isActive ? "Yes" : "No"}</p>
              <p><strong>startDate:</strong> {new Date(coupon.startDate).toLocaleString()}</p>
              <p><strong>endDate:</strong> {new Date(coupon.endDate).toLocaleString()}</p>
              <p><strong>Created At:</strong> {new Date(coupon.createdAt).toLocaleString()}</p>
            </div>
          ) : (
            <div className="text-sm text-red-500">Không thể tải dữ liệu chi tiết</div>
          )}
        </DialogContent>
      </Dialog>
  )
}

export default ViewDetailCoupon
