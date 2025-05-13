"use client";

import { columns } from "./columns";
import { useEffect, useState } from "react";
import AxiosAPI from "@/configs/axios";
import { usePaginatedQuery } from "@/hooks/usePaginatedQuery";
import { User, Location } from "@/types/type";
import dynamic from "next/dynamic";
import { LoadingSkeleton } from "@/components/layouts/LoadingSkeleton";
import { getStaffList } from "@/services/user";
import { staffSchema } from "@/types/schema/user";
import { toast } from "react-toastify";
import { getAllLocations } from "@/services/location";
import { IconPlus } from "@tabler/icons-react";
import { PlusCircleIcon } from "lucide-react";

const CreateDialogForm = dynamic(
  () =>
    import("@/components/layouts/CreateDialog").then(
      (mod) => mod.MemoizedCreateDialogForm
    ),
  {
    loading: () => <LoadingSkeleton />,
    ssr: false,
  }
);

const CrudPageWrapper = dynamic(
  () =>
    import("@/components/layouts/SharedPage").then(
      (mod) => mod.MemoizedCrudPage
    ),
  {
    loading: () => <LoadingSkeleton />,
    ssr: false,
  }
);

export default function AccountStaffPage() {
  const [pageSize, setPageSize] = useState(5);
  const [pageIndex, setPageIndex] = useState(0);
  const [locations, setLocations] = useState<Location[]>([]);

  const { data, totalItems, isLoading, refetch } = usePaginatedQuery<User>({
    queryKey: "booths",
    pageIndex,
    pageSize,
    queryFn: async ({ page, size, search }) => {
      return await getStaffList(page, size);
    },
  });

  useEffect(() => {
    const fetchLocations = async () => {
      try {
        const res = await getAllLocations();
        const validLocations = res.filter(
          (l): l is Location => !!l?.locationName && !!l?.id
        );
        setLocations(validLocations);
      } catch (error) {
        console.error("Failed to fetch locations", error);
        toast.error("Failed to load locations");
      }
    };

    fetchLocations();
  }, []);

  return (
    <CrudPageWrapper
      title="Account Staff Management"
      isSearchable={false}
      createButton={
        <CreateDialogForm
          title="Add Staff"
          triggerText=""
          triggerIcon={<PlusCircleIcon className="w-10 h-10" />}
          schema={staffSchema}
          fields={[
            {
              type: "text",
              name: "userName",
              label: "Username",
            },
            {
              type: "text",
              name: "email",
              label: "Email",
            },
            {
              type: "text",
              name: "phoneNumber",
              label: "Phone Number",
            },
            {
              type: "text",
              name: "password",
              label: "Password",
            },
            {
              type: "text",
              name: "fullName",
              label: "Full Name",
            },
            {
              type: "select",
              name: "gender",
              label: "Gender",
              options: [
                { label: "Male", value: "0" },
                { label: "Female", value: "1" },
                { label: "Other", value: "2" },
              ],
            },
            {
              type: "date",
              name: "birthDate",
              label: "Birth Date",
            },
            {
              type: "select",
              name: "locationId",
              label: "Location",
              options: locations.map((l) => ({
                label: l.locationName,
                value: l.id,
              })),
            },
          ]}
          onSubmit={async (values) => {
            await AxiosAPI.post("/api/User/create", {
              ...values,
            });
            refetch();
          }}
          
        />
      }
      data={data}
      columns={columns()}
      isLoading={isLoading}
      pageCount={Math.ceil(totalItems / pageSize)}
      pageIndex={pageIndex}
      pageSize={pageSize}
      onPageChange={setPageIndex}
      onPageSizeChange={setPageSize}
    />
  );
}
