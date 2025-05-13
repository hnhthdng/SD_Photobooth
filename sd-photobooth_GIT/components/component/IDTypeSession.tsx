import { useEffect, useState } from "react"
import { TypeSession } from "@/types/type"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import AxiosAPI from "@/configs/axios"
import { Loader2 } from "lucide-react"


const ViewDetailTypeSession = ({ id }: { id: number }) => {
  const [typeSession, setTypeSession] = useState<TypeSession | null>(null)
  const [open, setOpen] = useState(false)
  const [loading, setLoading] = useState(false)

  const fetchDetail = async () => {
    try {
      setLoading(true)
      const response = await AxiosAPI.get<TypeSession>(`/api/TypeSession/${id}`)
      setTypeSession(response.data)
    } catch (error) {
      console.error("Lỗi khi lấy dữ liệu chi tiết", error)
      setTypeSession(null)
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
          <DialogTitle>Chi tiết </DialogTitle>
        </DialogHeader>

        {loading ? (
          <div className="flex items-center space-x-2 text-sm text-muted-foreground">
            <Loader2 className="animate-spin h-4 w-4" /> <span>Đang tải...</span>
          </div>
        ) : typeSession ? (
          <div className="space-y-2 text-sm">
            <p><strong>ID:</strong> {typeSession.id}</p>
            <p><strong>Method Name:</strong> {typeSession.name}</p>
            
            
            <p><strong>description:</strong> {typeSession.description}</p>
            <p><strong>duration:</strong> {typeSession.duration}</p>
            <p><strong>price:</strong> {typeSession.price}</p>
            <p><strong>ableTakenNumber:</strong> {typeSession.ableTakenNumber}</p>
            <p><strong>forMobile:</strong> {typeSession.forMobile ? "Yes" : "No"}</p>
            <p><strong>Created At:</strong> {new Date(typeSession.createdAt).toLocaleString()}</p>
          </div>
        ) : (
          <div className="text-sm text-red-500">Không thể tải dữ liệu chi tiết</div>
        )}
      </DialogContent>
    </Dialog>
  )
}

export default ViewDetailTypeSession
