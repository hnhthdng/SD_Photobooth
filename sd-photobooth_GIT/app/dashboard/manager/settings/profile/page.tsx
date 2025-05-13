"use client";
import React, { useEffect, useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Text } from "@mantine/core";


import GPAvatar from "@/components/component/GPAvatar";
import GPProfile from "@/components/component/GPProfile";

import { useUserStore } from "@/hooks/userStore";


const ProfilePage = () => {
  
  const { user, fetchUser } = useUserStore();

  useEffect(() => {
    fetchUser();
  }, []);
  return (
    <Card>
      <CardHeader>
        <CardTitle>Profile</CardTitle>
      </CardHeader>
      <CardContent>
        {user && (
          <>
            <Card className="flex items-center justify-between p-5">
              <div className="flex items-center gap-3">
                <Avatar className="w-20 h-20">
                  <AvatarImage src={user.avatar ?? undefined} alt="@shadcn" />
                  <AvatarFallback>CN</AvatarFallback>
                </Avatar>

                <div>
                  <Text className="font-sans font-semibold">
                    {" "}
                    {user.userName}
                  </Text>
                  <Text className="font-sans font-normal text-slate-400">
                    {" "}
                    {user.role} | {user.email}
                  </Text>
                </div>
              </div>

              <GPAvatar onUpdateSuccess={fetchUser} />
            </Card>

            <Card className=" my-5">
              <CardHeader>
                {/* <CardTitle>Personal Information</CardTitle> */}
                <GPProfile onUpdateSuccess={fetchUser} />
              </CardHeader>
              <CardContent>
                <div className="flex items-end justify-between ">
                  <div className="flex items-center  gap-20">
                    <div>
                      <Text
                        className="font-sans font-semibold text-gray-500"
                        size="lg"
                      >
                        Full Name
                      </Text>
                      <Text className="font-sans font-medium" size="sm">
                        {user.fullName || "N/A"}
                      </Text>
                    </div>

                    <div>
                      <Text
                        className="font-sans font-semibold text-gray-500"
                        size="lg"
                      >
                        Phone Number
                      </Text>
                      <Text className="font-sans font-medium" size="sm">
                        {user.phoneNumber || "N/A"}
                      </Text>
                    </div>
                  </div>
                </div>
                <div className="py-10">
                  <Text
                    className="font-sans font-semibold text-gray-500"
                    size="lg"
                  >
                    Gender
                  </Text>
                  <Text className="font-sans font-medium" size="sm">
                    {" "}
                    {user.gender === 0
                      ? "Nam"
                      : user.gender === 1
                      ? "Nữ"
                      : "Other"}
                  </Text>
                </div>
                <div className="pt-2">
                  <Text
                    className="font-sans font-semibold text-gray-500"
                    size="lg"
                  >
                    Birth Date
                  </Text>
                  <Text className="font-sans font-medium" size="sm">
                    {user.birthDate
                      ? new Date(user.birthDate).toLocaleDateString()
                      : "N/A"}
                  </Text>
                </div>
              </CardContent>
            </Card>
          </>
        )}
      </CardContent>
    </Card>
  );
};

export default ProfilePage;
