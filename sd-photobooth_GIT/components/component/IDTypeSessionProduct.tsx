import { useEffect, useState } from "react"
import { TypeSessionProduct } from "@/types/type"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import AxiosAPI from "@/configs/axios"
import { Loader2 } from "lucide-react"
import { FaEye } from "react-icons/fa"

const ViewDetailTypeSessionProduct = ({ id }: { id: number }) => {
     const [type, setType] = useState<TypeSessionProduct | null>(null)
     const [open, setOpen] = useState(false)
     const [loading, setLoading] = useState(false)

     const fetchDetail = async () => {
          try {
               setLoading(true)
               const response = await AxiosAPI.get<TypeSessionProduct>(`/api/TypeSessionProduct/${id}`)
               setType(response.data)
          } catch (error) {
               console.error("Lỗi khi lấy dữ liệu chi tiết", error)
               setType(null)
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
                    ) : type ? (
                         <div className="space-y-2 text-sm">
                              <p><strong>ID:</strong> {type.id}</p>
                              <p><strong>Method Name:</strong> {type.name}</p>
                              <p><strong>productId:</strong> {type.productId}</p>
                              <p><strong>typeSessionId:</strong> {type.typeSessionId}</p>
                              <p><strong>levelMembershipId:</strong> {type.levelMembershipId}</p>
                              <p><strong>couponId:</strong> {type.couponId}</p>

                              <p><strong>Created At:</strong> {new Date(type.createdAt).toLocaleString()}</p>
                         </div>
                    ) : (
                         <div className="text-sm text-red-500">Không thể tải dữ liệu chi tiết</div>
                    )}
               </DialogContent>
          </Dialog>
     )
}

export default ViewDetailTypeSessionProduct
