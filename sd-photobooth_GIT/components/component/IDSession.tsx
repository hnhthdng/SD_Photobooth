import { useEffect, useState } from "react"
import { Session } from "@/types/type"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import AxiosAPI from "@/configs/axios"
import { Loader2 } from "lucide-react"
import { FaEye } from "react-icons/fa"



const ViewDetailSession = ({ id }: { id: number }) => {

     const [session, setSession] = useState<Session | null>(null)
     const [open, setOpen] = useState(false)
     const [loading, setLoading] = useState(false)

     const fetchDetail = async () => {
          try {
               setLoading(true)
               const response = await AxiosAPI.get<Session>(`/api/Session/${id}`)
               setSession(response.data)
          } catch (error) {
               console.error("Lỗi khi lấy dữ liệu chi tiết", error)
               setSession(null)
          } finally {
               setLoading(false)
          }
     }
     const formatDate = (date: any) => {
          return date ? new Date(date).toLocaleString() : "Chưa xác định";
     };

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
                    ) : session ? (
                         <div className="space-y-2 text-sm">
                              <p><strong>ID:</strong> {session.id}</p>
                              <p><strong>Method Name:</strong> {session.code}</p>

                              <p><strong>	Expired:</strong> {formatDate(session.expired)}</p>
                              <p><strong>orderId:</strong> {session.orderId}</p>
                              <p><strong>isActive:</strong> {session.isActive ? "Yes" : "No"}</p>
                              <p><strong>Created At:</strong> {new Date(session.createdAt).toLocaleString()}</p>
                         </div>
                    ) : (
                         <div className="text-sm text-red-500">Không thể tải dữ liệu chi tiết</div>
                    )}
               </DialogContent>
          </Dialog>
     )
}

export default ViewDetailSession
