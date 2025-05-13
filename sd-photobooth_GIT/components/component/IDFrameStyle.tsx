"use client"
import { useEffect, useState } from "react"
import { FrameStyle } from "@/types/type"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import AxiosAPI from "@/configs/axios"
import { Loader2 } from "lucide-react"
import Image from "next/image"


const ViewDetailFrameStyle = ({ id }: { id: number }) => {
     const [frameStyle, setFrameStyle] = useState<FrameStyle | null>(null)
     const [open, setOpen] = useState(false)
     const [loading, setLoading] = useState(false)

     const fetchDetail = async () => {
          try {
               setLoading(true)
               const response = await AxiosAPI.get<FrameStyle>(`/api/FrameStyle/${id}`)
               setFrameStyle(response.data)
          } catch (error) {
               console.error("Lỗi khi lấy dữ liệu chi tiết", error)
               setFrameStyle(null)
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
                         <DialogTitle>Frame Style</DialogTitle>
                    </DialogHeader>

                    {loading ? (
                         <div className="flex items-center space-x-2 text-sm text-muted-foreground">
                              <Loader2 className="animate-spin h-4 w-4" /> <span>Đang tải...</span>
                         </div>
                    ) : frameStyle ? (
                         <div className="space-y-2 text-sm">
                              <p><strong>ID:</strong> {frameStyle.id}</p>
                              <p><strong>Method Name:</strong> {frameStyle.name}</p>
                              <Image src={frameStyle.imageUrl} alt="frame style url" width={100} height={100} />
                              {/* <p><strong>URL:</strong> {frame.frameUrl}</p> */}
                              <p><strong>Style Name:</strong> {frameStyle.description}</p>


                              <p><strong>Created At:</strong> {new Date(frameStyle.createdAt).toLocaleString()}</p>
                         </div>
                    ) : (
                         <div className="text-sm text-red-500">Không thể tải dữ liệu chi tiết</div>
                    )}
               </DialogContent>
          </Dialog>
     )
}

export default ViewDetailFrameStyle
