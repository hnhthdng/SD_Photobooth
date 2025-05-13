import { useEffect, useState } from "react"
import { Frame } from "@/types/type"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import AxiosAPI from "@/configs/axios"
import { Loader2 } from "lucide-react"
import Image from "next/image"

const ViewDetailFrame = ({ id }: { id: number }) => {
  const [frame, setFrame] = useState<Frame | null>(null)
  const [open, setOpen] = useState(false)
  const [loading, setLoading] = useState(false)

  const fetchDetail = async () => {
    try {
      setLoading(true)
      const response = await AxiosAPI.get<Frame>(`/api/Frame/${id}`)
      // console.log(response.data)
      setFrame(response.data)
    } catch (error) {
      console.error("Lỗi khi lấy dữ liệu chi tiết", error)
      setFrame(null)
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
          <DialogTitle>Chi tiết</DialogTitle>
        </DialogHeader>

        {loading ? (
          <div className="flex items-center space-x-2 text-sm text-muted-foreground">
            <Loader2 className="animate-spin h-4 w-4" /> <span>Đang tải...</span>
          </div>
        ) : frame ? (
          <div className="space-y-2 text-sm">
            <p><strong>ID:</strong> {frame.id}</p>
            <p><strong>Method Name:</strong> {frame.name}</p>
            <Image src={frame.frameUrl} alt="frame url" width={100} height={100} />
            <p><strong>Style Name:</strong> {frame.frameStyleName}</p>
            <p><strong>Slot Count:</strong> {frame.slotCount}</p>
            <p><strong>For Mobile:</strong> {frame.forMobile ? "Yes" : "No"}</p>
            <p><strong>Created At:</strong> {new Date(frame.createdAt).toLocaleString()}</p>

            {/* Kiểm tra điều kiện trước khi sử dụng .map */}
            {frame.coordinates && frame.coordinates.length > 0 ? (
              <div className="mt-4 grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                {frame.coordinates.map((coordinate) => (
                  <div key={coordinate.id} className="border p-4 rounded-lg shadow-lg">
                    <p><strong>ID:</strong> {coordinate.id}</p>
                    <p><strong>Frame ID:</strong> {coordinate.frameId}</p>
                    <p><strong>X:</strong> {coordinate.x}</p>
                    <p><strong>Y:</strong> {coordinate.y}</p>
                    <p><strong>Width:</strong> {coordinate.width}</p>
                    <p><strong>Height:</strong> {coordinate.height}</p>
                  </div>
                ))}
              </div>
            ) : (
              <div className="text-sm text-red-500">Không có coordinate nào</div>
            )}
          </div>
        ) : (
          <div className="text-sm text-red-500">Không thể tải dữ liệu chi tiết</div>
        )}
      </DialogContent>
    </Dialog>
  )
}

export default ViewDetailFrame;
