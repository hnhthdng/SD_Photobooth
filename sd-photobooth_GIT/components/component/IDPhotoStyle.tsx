import { useEffect, useState } from "react"
import { PhotoStyle } from "@/types/type"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import AxiosAPI from "@/configs/axios"
import { Loader2 } from "lucide-react"
import Image from "next/image"

const ViewDetailPhotoStyle = ({ id }: { id: number }) => {
  const [photoStyle, setPhotoStyle] = useState<PhotoStyle | null>(null)
  const [open, setOpen] = useState(false)
  const [loading, setLoading] = useState(false)

  const fetchDetail = async () => {
    try {
      setLoading(true)
      const response = await AxiosAPI.get<PhotoStyle>(`/api/PhotoStyle/${id}`)
      setPhotoStyle(response.data)
    } catch (error) {
      console.error("Lỗi khi lấy dữ liệu chi tiết", error)
      setPhotoStyle(null)
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
      <DialogContent className="sm:max-w-[425px] max-h-[80vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>Chi tiết </DialogTitle>
        </DialogHeader>

        {loading ? (
          <div className="flex items-center space-x-2 text-sm text-muted-foreground">
            <Loader2 className="animate-spin h-4 w-4" /> <span>Đang tải...</span>
          </div>
        ) : photoStyle ? (
          <div className="space-y-2 text-sm">
            <p><strong>ID:</strong> {photoStyle.id}</p>
            <p><strong>Method Name:</strong> {photoStyle.name}</p>
            <Image src={photoStyle.imageUrl} alt={photoStyle.name} width={200} height={200} />
            <p><strong>Description:</strong> {photoStyle.description}</p>
            
            <p><strong>prompt:</strong> {photoStyle.prompt}</p>
            <p><strong>negativePrompt:</strong> {photoStyle.negativePrompt}</p>
            <p><strong>numImagesPerGen:</strong> {photoStyle.numImagesPerGen}</p>
            <p><strong>width:</strong> {photoStyle.width}</p>
            <p><strong>height:</strong> {photoStyle.height}</p>
         
            
            <p><strong>Controlnets:</strong> {photoStyle.controlnets}</p>
            <p><strong>Is Active:</strong> {photoStyle.faceImage ? "Yes" : "No"}</p>
            <p><strong>Is Online:</strong> {photoStyle.backgroundRemover ? "Yes" : "No"}</p>
           
            <p><strong>Created At:</strong> {new Date(photoStyle.createdAt).toLocaleString()}</p>
          </div>
        ) : (
          <div className="text-sm text-red-500">Không thể tải dữ liệu chi tiết</div>
        )}
      </DialogContent>
    </Dialog>
  )
}

export default ViewDetailPhotoStyle
