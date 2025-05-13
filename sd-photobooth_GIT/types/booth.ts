export interface Booth {
  id: string;
  name: string;
  location: string;
  createdAt: string;
  updatedAt: string;
}

export interface BoothSearchParams {
  page: number;
  size: number;
  search?: string;
}
