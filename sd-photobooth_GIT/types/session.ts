export interface Session {
  id: string;
  name: string;
  description: string;
  duration: number;
  price: number;
  isPrinting: boolean;
  ableTakenNumber: number;
  createdAt: string;
  lastModified: string;
  createdById: string;
  lastModifiedById: string;
}
