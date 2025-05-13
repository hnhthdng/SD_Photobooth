import { useEffect, useState } from "react"
import { Sticker } from "@/types/type"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import AxiosAPI from "@/configs/axios"
import { Loader2 } from "lucide-react"
import Image from "next/image"

const ViewDetailSticker = ({ id }: { id: number }) => {
     const [sticker, setSticker] = useState<Sticker | null>(null)
     const [open, setOpen] = useState(false)
     const [loading, setLoading] = useState(false)

     const fetchDetail = async () => {
          try {
               setLoading(true)
               const response = await AxiosAPI.get<Sticker>(`/api/Sticker/${id}`)
               setSticker(response.data)
          } catch (error) {
               console.error("Lỗi khi lấy dữ liệu chi tiết", error)
               setSticker(null)
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
                    ) : sticker ? (
                         <div className="space-y-2 text-sm">
                              <p><strong>ID:</strong> {sticker.id}</p>
                              <p><strong>Method Name:</strong> {sticker.name}</p>

                              <p><strong>Slot Count:</strong> {sticker.stickerStyleName}</p>
                              <Image src={sticker.stickerUrl} alt="sticker" width={100} height={100} />
                              <p><strong>Created At:</strong> {new Date(sticker.createdAt).toLocaleString()}</p>
                         </div>
                    ) : (
                         <div className="text-sm text-red-500">Không thể tải dữ liệu chi tiết</div>
                    )}
               </DialogContent>
          </Dialog>
     )
}

export default ViewDetailSticker
