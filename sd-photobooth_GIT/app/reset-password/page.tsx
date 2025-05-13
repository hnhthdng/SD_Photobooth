"use client";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { GalleryVerticalEnd } from "lucide-react";
import Link from "next/link";
import React, { useState } from "react";
import { useSearchParams, useRouter } from "next/navigation";
import { toast } from "react-toastify";
import AxiosAPI from "@/configs/axios";

const ResetPasswordPage = () => {
  const searchParams = useSearchParams();
  const router = useRouter();

  const rawToken = searchParams.get("token") || "";
  const email = decodeURIComponent(searchParams.get("email") || "");

  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [loading, setLoading] = useState(false);

  const restoreSpecialChars = (encodedToken: string) => {
    const decodedToken = decodeURIComponent(encodedToken);
    const specialChars: Record<string, string> = {
      "%20": " ", "%2B": "+", "%3D": "=", "%2F": "/", 
      "%40": "@", "%3F": "?", "%26": "&",
    };
    let restoredToken = decodedToken;
    for (const [encoded, original] of Object.entries(specialChars)) {
      restoredToken = restoredToken.replace(new RegExp(encoded, "g"), original);
    }
    return restoredToken;
  };

  // Token cuối cùng
  const finalToken = restoreSpecialChars(rawToken);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    

    if (password !== confirmPassword) {
      toast.error("Passwords do not match!");
      return;
    }

    try {
      setLoading(true);

      const res = await AxiosAPI.post("/api/Identity/reset-password", {
        email,
        token:finalToken,
        newPassword: password,
      });
      // console.log(res);
      if (res.status === 200) {
        toast.success("Password updated successfully!");
        router.replace("/");
      } else {
        toast.error("Failed to update password.");
      }
    } catch (error: any) {
      console.error("Error resetting password:", error);
      toast.error(error.response?.data?.message || "Something went wrong!");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex min-h-screen flex-col items-center justify-center gap-6 bg-muted p-6 md:p-10">
      <div className="flex w-full max-w-sm flex-col gap-6">
        <a href="#" className="flex items-center gap-2 self-center font-medium">
          <div className="flex h-6 w-6 items-center justify-center rounded-md bg-primary text-primary-foreground">
            <GalleryVerticalEnd className="size-4" />
          </div>
          Acme Inc.
        </a>
        <Card>
          <CardHeader className="text-center">
            <CardTitle className="text-xl">Reset Password</CardTitle>
            <CardDescription>Enter your new password below.</CardDescription>
          </CardHeader>
          <CardContent>
            <form onSubmit={handleSubmit}>
              <div className="grid gap-4">
                <div className="grid gap-2">
                  <Label htmlFor="new-password">New Password</Label>
                  <Input
                    id="new-password"
                    className="password-input"
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                  />
                </div>
                <div className="grid gap-2">
                  <Label htmlFor="confirm-password">Confirm Password</Label>
                  <Input
                    id="confirm-password"
                    className="password-input"
                    type="password"
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    required
                  />
                </div>
                <Button type="submit" className="w-full" disabled={loading}>
                  {loading ? "Updating..." : "Update Password"}
                </Button>
              </div>
              <div className="text-center text-sm mt-4">
                You already have an account?{" "}
                <Link href="/" className="underline underline-offset-4">
                  Sign In
                </Link>
              </div>
            </form>
          </CardContent>
        </Card>
      </div>
    </div>
  );
};

export default ResetPasswordPage;
