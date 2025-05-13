"use client";

import React, { useState, useEffect } from "react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardFooter } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { toast } from "react-toastify";
import AxiosAPI from "@/configs/axios";
import { LocationResponseDTO } from "@/types/user";

const AddUser = () => {
  const [formData, setFormData] = useState({
    role: 3, // Mặc định là Customer
    userName: "",
    email: "",
    phoneNumber: "",
    password: "",
    fullName: "",
    gender: 2, // Mặc định là Other
    birthDate: "",
    locationId: 0,
  });

  const [loading, setLoading] = useState(false);
  const [locations, setLocations] = useState<LocationResponseDTO[]>([]);

  useEffect(() => {
    const fetchLocations = async () => {
      try {
        const response = await AxiosAPI.get("api/Location");
        if (response.status === 200) {
          setLocations(response.data as unknown as LocationResponseDTO[]);
        }
      } catch (error) {
        toast.error("Failed to fetch locations");
      }
    };

    fetchLocations();
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSelectChange = (name: string, value: string | number) => {
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault(); // Chặn reload trang
    setLoading(true);

    try {
      const response = await AxiosAPI.post("/api/User/create", formData);

      if (response.status === 201 || response.status === 200) {
        toast.success("User created successfully!");
        setFormData({
          role: 1,
          userName: "",
          email: "",
          phoneNumber: "",
          password: "",
          fullName: "",
          gender: 2,
          birthDate: "",
          locationId: 0,
        });
      } else {
        throw new Error("Failed to create user");
      }
    } catch (error) {
      toast.error("Error creating user");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Card className="w-full shadow-none border-none max-h-[450px] overflow-y-auto">
      <CardContent>
        <form onSubmit={handleSubmit} className="grid gap-4">
          <div className="flex flex-col space-y-1.5">
            <Label>Role</Label>
            <Select
              onValueChange={(value) =>
                handleSelectChange("role", parseInt(value))
              }
              defaultValue={formData.role.toString()}
            >
              <SelectTrigger>
                <SelectValue placeholder="Select Role" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="1">Manager</SelectItem>
                <SelectItem value="2">Staff</SelectItem>
              </SelectContent>
            </Select>
          </div>
          <div className="flex flex-col space-y-1.5">
            <Label htmlFor="userName">Username</Label>
            <Input
              id="userName"
              name="userName"
              value={formData.userName}
              onChange={handleChange}
              required
            />
          </div>
          <div className="flex flex-col space-y-1.5">
            <Label htmlFor="email">Email</Label>
            <Input
              id="email"
              type="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              required
            />
          </div>
          <div className="flex flex-col space-y-1.5">
            <Label htmlFor="phoneNumber">Phone Number</Label>
            <Input
              id="phoneNumber"
              name="phoneNumber"
              value={formData.phoneNumber}
              onChange={handleChange}
              required
            />
          </div>
          <div className="flex flex-col space-y-1.5">
            <Label htmlFor="password">Password</Label>
            <Input
              id="password"
              type="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              required
            />
          </div>
          <div className="flex flex-col space-y-1.5">
            <Label htmlFor="fullName">Full Name</Label>
            <Input
              id="fullName"
              name="fullName"
              value={formData.fullName}
              onChange={handleChange}
              required
            />
          </div>
          <div className="flex flex-col space-y-1.5">
            <Label>Gender</Label>
            <Select
              onValueChange={(value) =>
                handleSelectChange("gender", parseInt(value))
              }
              defaultValue={formData.gender.toString()}
            >
              <SelectTrigger>
                <SelectValue placeholder="Select Gender" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="0">Male</SelectItem>
                <SelectItem value="1">Female</SelectItem>
                <SelectItem value="2">Other</SelectItem>
              </SelectContent>
            </Select>
          </div>
          <div className="flex flex-col space-y-1.5">
            <Label htmlFor="birthDate">Birth Date</Label>
            <Input
              id="birthDate"
              type="date"
              name="birthDate"
              value={formData.birthDate}
              onChange={handleChange}
              required
            />
          </div>
          <div className="flex flex-col space-y-1.5">
            <Label>Location</Label>
            <Select
              onValueChange={(value) =>
                handleSelectChange("locationId", parseInt(value))
              }
              defaultValue={formData.locationId.toString()}
            >
              <SelectTrigger>
                <SelectValue placeholder="Select Location" />
              </SelectTrigger>
              <SelectContent>
                {locations.map((location) => (
                  <SelectItem key={location.id} value={location.id.toString()}>
                    {location.locationName}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>
        </form>
      </CardContent>
      <CardFooter className="flex justify-between">
        <Button
          variant="outline"
          onClick={() =>
            setFormData({
              role: 3,
              userName: "",
              email: "",
              phoneNumber: "",
              password: "",
              fullName: "",
              gender: 2,
              birthDate: "",
              locationId: 0,
            })
          }
        >
          Cancel
        </Button>
        <Button type="submit" onClick={handleSubmit} disabled={loading}>
          {loading ? "Creating..." : "Create User"}
        </Button>
      </CardFooter>
    </Card>
  );
};

export default AddUser;
