"use client";

import * as z from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { signIn, useSession } from "next-auth/react";
import { useRouter } from "next/navigation";
import { useEffect } from "react";
import { toast } from "react-toastify";
import Image from "next/image";
import Link from "next/link";

import { Card, CardContent, CardHeader } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import fpt from "@/assets/tech-x.png";

const formSchema = z.object({
  email: z.string().min(1, "Bạn chưa nhập email").email("Hãy nhập đúng email"),
  password: z.string().min(1, "Bạn chưa nhập mật khẩu"),
});

const Home = () => {
  const { data: session, update } = useSession();
  const router = useRouter();

  useEffect(() => {
    if (session?.user?.role) {
      router.replace(`/dashboard/${session.user.role.toLowerCase()}`);
    }
  }, [session, router]);

  const form = useForm({
    resolver: zodResolver(formSchema),
    defaultValues: { email: "", password: "" },
  });

  const handleSubmit = async (data: z.infer<typeof formSchema>) => {
    try {
      const result = await signIn("credentials", {
        email: data.email,
        password: data.password,
        redirect: false,
      });

      if (result?.error)
        return toast.error("Đăng nhập thất bại! Vui lòng thử lại.");

      const updatedSession = await update();
      router.replace(`/dashboard/${updatedSession?.user?.role.toLowerCase()}`);
    } catch (error) {
      console.error("🔴 Lỗi khi đăng nhập:", error);
      toast.error("Đăng nhập thất bại! Vui lòng thử lại.");
    }
  };

  return (
    <div className="relative flex min-h-screen flex-col items-center justify-center p-6 md:p-10">
      <video autoPlay loop muted className="absolute top-0 left-0 w-full h-full object-cover z-0">
        <source src="/background1.mp4" type="video/mp4" />
      </video>
      {/* <Image
        src={"/blue-1295919_1280.png"}
        alt="bg"
        className="absolute top-0 left-0 w-full h-full object-cover z-0"
        width={0}
        height={0}
      /> */}
      <div className="relative z-10 w-full max-w-sm">
        <Card className="bg-white/10 backdrop-blur-md shadow-xl border border-white/10 rounded-lg">
          <CardHeader className="flex items-center justify-center">
            <Image
              src={fpt}
              alt="FPT Logo"
              width={150}
              height={150}
              className="rounded-md drop-shadow-lg"
            />
          </CardHeader>
          <CardContent>
            <Form {...form}>
              <form
                onSubmit={form.handleSubmit(handleSubmit)}
                className="space-y-4"
              >
                <FormField
                  control={form.control}
                  name="email"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel className="text-white font-medium">
                        Email
                      </FormLabel>
                      <FormControl>
                        <Input
                          type="email"
                          placeholder="Enter your email"
                          className="bg-white/90 text-gray-900 placeholder-gray-500 focus-visible:ring-2 focus-visible:ring-blue-500"
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name="password"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel className="text-white font-medium">
                        Password
                      </FormLabel>
                      <FormControl>
                        <Input
                          type="password"
                          placeholder="Enter your password"
                          className="bg-white/90 text-gray-900 placeholder-gray-500 focus-visible:ring-2 focus-visible:ring-blue-500"
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <Link
                  href="/forget-password"
                  className="ml-auto inline-block text-sm text-blue-200 hover:text-blue-100 hover:underline"
                >
                  Forgot your password?
                </Link>

                <Button
                  type="submit"
                  className="w-full font-semibold bg-blue-600 hover:bg-blue-700 text-white transition-colors"
                >
                  Login
                </Button>
              </form>

              <div className="relative flex items-center my-4">
                <div className="flex-1 border-t border-white/40"></div>
                <span className="px-3 text-sm text-white/80">
                  Or continue with
                </span>
                <div className="flex-1 border-t border-white/40"></div>
              </div>
              <Button
                variant="outline"
                className="w-full flex items-center gap-2 border-white/50 hover:bg-white/20"
                onClick={() => signIn("google", { redirectTo: "/" })}
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  viewBox="0 0 24 24"
                  className="w-5 h-5"
                  fill="currentColor"
                >
                  <path d="M12.48 10.92v3.28h7.84c-.24 1.84-.853 3.187-1.787 4.133-1.147 1.147-2.933 2.4-6.053 2.4-4.827 0-8.6-3.893-8.6-8.72s3.773-8.72 8.6-8.72c2.6 0 4.507 1.027 5.907 2.347l2.307-2.307C18.747 1.44 16.133 0 12.48 0 5.867 0 .307 5.387.307 12s5.56 12 12.173 12c3.573 0 6.267-1.173 8.373-3.36 2.16-2.16 2.84-5.213 2.84-7.667 0-.76-.053-1.467-.173-2.053H12.48z" />
                </svg>
                Login with Google
              </Button>
            </Form>
          </CardContent>
        </Card>
      </div>
      <footer className="absolute bottom-0 left-0 right-0 text-center text-sm text-black bg-white/20">
        <Link href="/privacy" className="hover:underline mx-2">
          Privacy Policy
        </Link>
        <Link href="/about" className="hover:underline">
          About Us
        </Link>
      </footer>
    </div>
  );
};

export default Home;
