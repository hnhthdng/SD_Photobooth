"use client"

import DeletePayment from "@/components/component/DeletePayment"
import EditLevelMembership from "@/components/component/GPLevelMembership"
import ViewDetailLevelMembership from "@/components/component/IDLevelMembership"
import { Button } from "@/components/ui/button"
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuLabel, DropdownMenuSeparator, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { LevelMembership } from "@/types/type"
import { ColumnDef } from "@tanstack/react-table"
import { MoreHorizontal } from "lucide-react"

export const columns = (
     onDelete: (id: number) => Promise<void>,
     refetchData: () => void,
): ColumnDef<LevelMembership>[] => [
          {
               accessorKey: "id",
               header: () => <div className="text-center">ID</div>,
               cell: ({ row }) => <div className="text-center">{row.getValue("id")}</div>
          },
          {
               accessorKey: "name",
               header: () => <div className="text-center">Name</div>,
               cell: ({ row }) => <div className="text-center">{row.getValue("name")}</div>
          },
          {
               accessorKey: "description",
               header: () => <div className="text-center">Description</div>,
               cell: ({ row }) => <div className="text-center">{row.getValue("description")}</div>
          },
          {
               accessorKey: "point",
               header: () => <div className="text-center">Point</div>,
               cell: ({ row }) => <div className="text-center">{row.getValue("point")}</div>
          },
          {
               accessorKey: "discountPercent",
               header: () => <div className="text-center">Discount Percent</div>,
               cell: ({ row }) => <div className="text-center">{row.getValue("discountPercent")}</div>
          },
          {
               accessorKey: "maxDiscount",
               header: () => <div className="text-center">Max Discount</div>,
               cell: ({ row }) => <div className="text-center">{row.getValue("maxDiscount")}</div>
          },
          {
               accessorKey: "minOrder",
               header: () => <div className="text-center">Min Order</div>,
               cell: ({ row }) => <div className="text-center">{row.getValue("minOrder")}</div>
          },
          {
               accessorKey: "nextLevelId",
               header: () => <div className="text-center">nextLevelId</div>,
               cell: ({ row }) => <div className="text-center">{row.getValue("nextLevelId")}</div>
          },
          {
               id: "edit",
               header: () => <div className="text-center">Edit</div>,
               cell: ({ row }) => {
                    const id = row.original.id;
                    return (
                         <EditLevelMembership id={id} onUpdated={refetchData} />
                    )
               }
          },
          {
               id: "actions",
               enableHiding: false,
               cell: ({ row }) => {
                    const id = row.original.id
                    const sticker = row.original;
                    return (
                         <DropdownMenu>
                              <DropdownMenuTrigger asChild>
                                   <Button variant="ghost" className="h-8 w-8 p-0">

                                        <MoreHorizontal />
                                   </Button>
                              </DropdownMenuTrigger>
                              <DropdownMenuContent align="center">
                                   <DropdownMenuLabel>Actions</DropdownMenuLabel>
                                   {/* <DropdownMenuItem onClick={() => navigator.clipboard.writeText(sticker.id.toString())}>
                                   Copy Sticker ID
                              </DropdownMenuItem> */}
                                   <DropdownMenuSeparator />
                                   <DropdownMenuItem asChild>
                                        <DeletePayment id={id} onDelete={onDelete} />
                                   </DropdownMenuItem>


                                   {/* <DropdownMenuSeparator /> */}

                                   {/* <DropdownMenuSeparator /> */}

                                   <DropdownMenuItem asChild>
                                        <ViewDetailLevelMembership id={id} />
                                   </DropdownMenuItem>

                              </DropdownMenuContent>
                         </DropdownMenu>
                    );
               }
          }
     ]
