export interface CustomAxiosResponse<T> {
  data: T | null;
  status: number;
  statusText: string;
}
