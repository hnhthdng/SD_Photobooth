import { DeleteItem } from "@/components/component/DeleteGeneric";
import DeletePayment from "@/components/component/DeletePayment";
import UpdateStickerStyle from "@/components/component/GPStickerStyle";
import ViewDetailStickerStyle from "@/components/component/IDStickerStyle";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { deleteStickerStyle } from "@/services/sticker-style";
import { StickerStyle } from "@/types/type";
import { ColumnDef } from "@tanstack/react-table";
import { MoreHorizontal } from "lucide-react";

const DateCell = ({ value }: { value: string }) => {
  const date = new Date(value);
  return <div>{date.toLocaleDateString()}</div>;
};

export const columns: ColumnDef<StickerStyle>[] = [
    {
      accessorKey: "id",
      header: () => <div className="text-center">ID</div>,
      cell: ({ row }) => <div className="text-center">{row.getValue("id")}</div>,
    },
    {
      accessorKey: "stickerStyleName",
      header: () => <div className="text-center">Sticker Style Name</div>,
      cell: ({ row }) => <div className="text-center">{row.getValue("stickerStyleName")}</div>,
    },
    {
      accessorKey: "description",
      header: () => <div className="text-center">Description</div>,
      cell: ({ row }) => <div className="text-center">{row.getValue("description")}</div>,
    },
    {
      accessorKey: "createdAt",
      header: () => <div className="text-center">Created At</div>,
      cell: ({ row }) => (
        <div className="text-center">
          <DateCell value={row.getValue("createdAt")} />
        </div>
      ),
    },
    {
      accessorKey: "lastModified",
      header: () => <div className="text-center">Last Modified</div>,
      cell: ({ row }) => (
        <div className="text-center">
          <DateCell value={row.getValue("lastModified")} />
        </div>
      ),
    },

    {
      id: "edit",
      header: () => <div className="text-center">Edit</div>,
      cell: ({ row }) => {
        const id = row.original.id;
        // return <UpdateStickerStyle id={id} onUpdateSuccess={refetchData} />;
        return <DeleteItem  id={id} deleteFn={deleteStickerStyle} queryKey="stickerStyles"
       />
      },
    },
    // {
    //   id: "actions",
    //   enableHiding: false,
    //   cell: ({ row }) => {
    //     const id = row.original.id;
    //     const sticker = row.original;
    //     return (
    //       <DropdownMenu>
    //         <DropdownMenuTrigger asChild>
    //           <Button variant="ghost" className="h-8 w-8 p-0">
    //             <MoreHorizontal />
    //           </Button>
    //         </DropdownMenuTrigger>
    //         <DropdownMenuContent align="center">
    //           <DropdownMenuLabel>Actions</DropdownMenuLabel>
    //           {/* <DropdownMenuItem onClick={() => navigator.clipboard.writeText(sticker.id.toString())}>
    //                                Copy Sticker ID
    //                           </DropdownMenuItem> */}
    //           <DropdownMenuSeparator />
    //           <DropdownMenuItem asChild>
    //             <DeletePayment id={id} onDelete={onDelete} />
    //           </DropdownMenuItem>
    //           <DropdownMenuItem asChild>
    //             <ViewDetailStickerStyle id={id} />
    //           </DropdownMenuItem>
    //         </DropdownMenuContent>
    //       </DropdownMenu>
    //     );
    //   },
    // },
  ];
