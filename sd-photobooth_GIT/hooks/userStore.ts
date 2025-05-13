import { create } from "zustand";
import AxiosAPI from "@/configs/axios";
import { User } from "@/types/type";

type UserStore = {
  user: User | null;
  setUser: (user: User) => void;
  fetchUser: () => Promise<void>;
};

export const useUserStore = create<UserStore>((set) => ({
  user: null,
  setUser: (user) => set({ user }),
  fetchUser: async () => {
    try {
      const res = await AxiosAPI.get<User>("/api/Identity/profile");
      set({ user: res.data });
    } catch (error) {
      console.error("Lỗi khi fetch user:", error);
    }
  },
}));
