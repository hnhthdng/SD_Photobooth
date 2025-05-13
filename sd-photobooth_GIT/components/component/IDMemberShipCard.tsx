import { useEffect, useState } from "react"
import { MembershipCard } from "@/types/type"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import AxiosAPI from "@/configs/axios"
import { Loader2 } from "lucide-react"


const ViewDetailMembershipCard = ({ id }: { id: number }) => {
  const [member, setMember] = useState<MembershipCard | null>(null)
  const [open, setOpen] = useState(false)
  const [loading, setLoading] = useState(false)

  const fetchDetail = async () => {
    try {
      setLoading(true)
      const response = await AxiosAPI.get<MembershipCard>(`/api/MembershipCard/${id}`)
      setMember(response.data)
    } catch (error) {
      console.error("Lỗi khi lấy dữ liệu chi tiết", error)
      setMember(null)
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
        ) : member ? (
          <div className="space-y-2 text-sm">
            <p><strong>ID:</strong> {member.id}</p>
            <p><strong>Current Point:</strong> {member.points}</p>
            <p><strong>Description:</strong> {member.description}</p>
            {/* <p><strong>URL:</strong> {frame.frameUrl}</p> */}
            <p><strong>Name:</strong> {member.customer.fullName}</p>
            <p><strong>email:</strong> {member.customer.email}</p>
            
            <p><strong>Target LevelMembership</strong> {member.levelMemberShip.name}</p>
            <p><strong>Point of LevelMembership:</strong> {member.levelMemberShip.point}</p>
            <p><strong>isActive:</strong> {member.isActive ? "Yes" : "No"}</p>
            <p><strong>Created At:</strong> {new Date(member.createdAt).toLocaleString()}</p>
          </div>
        ) : (
          <div className="text-sm text-red-500">Không thể tải dữ liệu chi tiết</div>
        )}
      </DialogContent>
    </Dialog>
  )
}

export default ViewDetailMembershipCard
