import { z } from "zod";

export const customerSchema = z.object({
  role: z.number().default(3),
  userName: z.string().min(1, "Username is required"),
  email: z.string().email("Invalid email"),
  phoneNumber: z.string().min(1, "Phone number is required"),
  password: z.string().min(6, "Password must be at least 6 characters"),
  fullName: z.string().min(1, "Full name is required"),
  gender: z.coerce.number().min(0).max(2),
  birthDate: z.string().min(1, "Birth date is required"),
});

export const staffSchema = z.object({
  role: z.number().default(2),
  userName: z.string().min(1, "Username is required"),
  email: z.string().email("Invalid email"),
  phoneNumber: z.string().min(1, "Phone number is required"),
  password: z.string().min(6, "Password must be at least 6 characters"),
  fullName: z.string().min(1, "Full name is required"),
  gender: z.coerce.number().min(0).max(2),
  birthDate: z.string().min(1, "Birth date is required"),
  locationId: z.coerce.number().min(0, "Location is required"),
});
