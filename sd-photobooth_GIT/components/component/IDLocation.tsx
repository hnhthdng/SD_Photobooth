import { useEffect, useState } from "react"
import { Location } from "@/types/type"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import AxiosAPI from "@/configs/axios"
import { Loader2 } from "lucide-react"
import { FaEye } from "react-icons/fa"
import { Card, Divider, Group, Skeleton, Text } from "@mantine/core"



const ViewDetailLocation = ({ id }: { id: number }) => {
  const [location, setLocation] = useState<Location | null>(null)
  const [open, setOpen] = useState(false)
  const [loading, setLoading] = useState(false)

  const fetchDetail = async () => {
    try {
      setLoading(true)
      const response = await AxiosAPI.get<Location>(`/api/Location/${id}`)
      setLocation(response.data)
    } catch (error) {
      console.error("Lỗi khi lấy dữ liệu chi tiết", error)
      setLocation(null)
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
          <Skeleton height={8} mt={6} width="70%" radius="xl" />
        ) : location ? (
          <Card
            shadow="sm"
            padding="lg"
            radius="md"
            withBorder
            className="bg-white dark:bg-[#1f1f1f] border border-gray-200 dark:border-[#333] transition-all"
          >
            <Text size="lg" fw={600} className="mb-2 text-gray-900 dark:text-white">
              Thông tin địa điểm
            </Text>

            <Divider className="mb-4 dark:border-[#444]" />

            <Group justify="space-between" className="mb-2">
              <Text size="sm" className="text-gray-500 dark:text-gray-400">ID:</Text>
              <Text size="sm" fw={500} className="text-gray-900 dark:text-gray-100">{location.id}</Text>
            </Group>

            <Group justify="space-between" className="mb-2">
              <Text size="sm" className="text-gray-500 dark:text-gray-400">Location Name:</Text>
              <Text size="sm" fw={500} className="text-gray-900 dark:text-gray-100">{location.locationName}</Text>
            </Group>

            <Group justify="space-between" className="mb-2">
              <Text size="sm" className="text-gray-500 dark:text-gray-400">Address:</Text>
              <Text size="sm" fw={500} className="text-gray-900 dark:text-gray-100">{location.address}</Text>
            </Group>

            <Group justify="space-between">
              <Text size="sm" className="text-gray-500 dark:text-gray-400">Created At:</Text>
              <Text size="sm" fw={500} className="text-gray-900 dark:text-gray-100">
                {new Date(location.createdAt).toLocaleString()}
              </Text>
            </Group>
          </Card>
        ) : (
          <div className="text-sm text-red-500">Không thể tải dữ liệu chi tiết</div>
        )}
      </DialogContent>
    </Dialog>
  )
}

export default ViewDetailLocation
