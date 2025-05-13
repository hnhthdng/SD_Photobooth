import { User } from "./type";

export interface UserInfo {
  id: string;
  name: string;
  role: string;
  exp: number;
}

export interface CustomerResponse {
  data: User[];
  totalCount: number;
}
