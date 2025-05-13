"use client"

import { Button } from "@/components/ui/button"

import {
     Dialog,
     DialogContent,
     DialogDescription,
     DialogFooter,
     DialogHeader,
     DialogTitle,
     DialogTrigger,
} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import AxiosAPI from "@/configs/axios"
import { Pencil } from "lucide-react"
import { useState } from "react"
import { toast } from "react-toastify"

interface GPAvatarProps {
     onUpdateSuccess: () => void
}

const GPAvatar:React.FC<GPAvatarProps> = ({onUpdateSuccess}) => {
     const [file, setFile] = useState<File | null>(null)
     const [loading, setLoading] = useState(false)
     const [open, setOpen] = useState(false)
     const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
          const selected = e.target.files?.[0] || null
          setFile(selected)
     }

     const handleSubmit = async () => {
          if (!file) return alert("Please select a file")
      
          const formData = new FormData()
          formData.append("avatar", file)
      
          try {
            setLoading(true)
      
            const response = await AxiosAPI.put("/api/Identity/update-avatar", formData, {
              headers: {
                "Content-Type": "multipart/form-data",
              },
            })
      
            
            toast.success("Update success")
            onUpdateSuccess()
            setOpen(false)
          } catch (err) {
            console.error(err)
            toast.error("Something went wrong while updating avatar")
          } finally {
            setLoading(false)
          }
        }
     return (
          <Dialog open={open} onOpenChange={setOpen}>
               <DialogTrigger asChild>
                    <Button variant="outline" size={"lg"} className="flex items-center  ">
                         <Pencil className="w-10 h-10" />

                    </Button>
               </DialogTrigger>
               <DialogContent className="sm:max-w-[425px]">
                    <DialogHeader>
                         <DialogTitle>Edit profile</DialogTitle>
                         <DialogDescription>
                              Make changes to your profile here. Click save when you're done.
                         </DialogDescription>
                    </DialogHeader>

                    <div className="grid w-full max-w-sm items-center gap-1.5">
                         <Label htmlFor="picture">Picture</Label>
                         <Input id="picture" type="file" onChange={handleFileChange} />
                    </div>

                    <DialogFooter>
                         <Button onClick={handleSubmit} disabled={loading}>
                              {loading ? "Saving..." : "Save changes"}
                         </Button>

                    </DialogFooter>
               </DialogContent>
          </Dialog>
     )
}

export default GPAvatar
