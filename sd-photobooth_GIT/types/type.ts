import { NumberInputFactory } from "@mantine/core";

export type Booth = {
  id: number;
  boothName: string;
  status: boolean;
  createdAt: string;
  description: string;
  // location: {
  //   locationName: string;
  //   address: string;
  // };
  location?: Location;
};

export type Coupon = {
  id: number;
  name: string;
  description: string;
  code: string;
  discount: number | null;
  discountPercent: number | null;
  startDate: string;
  endDate: string;
  maxUse: number;
  maxDiscount: number;
  minOrder: number;
  isActive: boolean;
  usedAmount: number;
  createdAt: string;
  lastModified: string;
  isDeleted: boolean;
  createdById: string | null;
  lastModifiedById: string | null;
};

export type Frame = {
  id: number;
  name: string;
  frameUrl: string;
  frameStyleName: string;
  forMobile: boolean;
  slotCount: number;
  createdAt: string;
  lastModified: string;
  createdById: string | null;
  lastModifiedById: string | null;
  frameStyleId: number;
  coordinates: Coordinate[];
};

export type FrameStyle = {
  id: number;
  name: string;
  description: string;
  imageUrl: string;
  createdAt: string;
  lastModified: string;
  createdById: string | null;
  lastModifiedById: string | null;
};

export type LevelMembership = {
  id: number;
  name: string;
  description: string;
  point: number;
  isActive: boolean;
  discountPercent: number;
  maxDiscount: number;
  minOrder: number;
  nextLevelId: number | null;
};

export type MembershipCard = {
  id: number;
  points: number;
  description: string;
  isActive: boolean;
  createdAt: string;
  lastModified: string;
  discountPercent: number;
  maxDiscount: number;
  minOrder: number;
  createdById: string | null;
  lastModifiedById: string | null;
  levelMemberShip: {
    id: number;
    name: string;
    point: number;
  };
  customer: {
    id: string;
    fullName: string | null;
    email: string | null;
    phoneNumber: number;
  };
};

export type Sticker = {
  id: number;
  name: string;
  stickerUrl: string;
  stickerStyleName: string;
  StickerStyleId: number;
  createdAt: string;
  lastModified: string;
  createdById: number | null;
  lastModifiedById: number | null;
};

export type StickerStyle = {
  id: number;
  stickerStyleName: string;
  description: string;
  createdAt: string;
  lastModified: string;
  createdById: string;
  lastModifiedById: string;
};

export type Location = {
  id: number;
  locationName: string;
  address: string;
  createdAt: string;
  lastModified: string;
  createdById: string | null;
  lastModifiedById: string | null;
};

export type TypeSession = {
  id: number;
  name: string;
  description: string;
  duration: number;
  price: number;
  forMobile: boolean;
  isPrinting: boolean;
  ableTakenNumber: number;
  createdAt: string;
  lastModified: string;
  createdById: string | null;
  lastModifiedById: string | null;
};

export type PaymentMethod = {
  id: number;
  methodName: string;
  description: string;
  isActive: boolean;
  createdAt: string;
  lastModified: string;
  createdById: string | null;
  lastModifiedById: string | null;
  forMobile: boolean;
  isOnline: boolean;
};

export type Session = {
  id: number;
  code: string;
  expired: string | null;
  isActive: boolean;
  orderId: number;
  createdAt: string;
  lastModified: string;
  isDeleted: boolean;
  ableTakenNumber: number;
};

export type PhotoStyle = {
  id: number;
  name: string;
  description: string;
  imageUrl: string;
  prompt: string;
  negativePrompt: string;
  controlnets: string;
  numImagesPerGen: number;
  backgroundColor: string;
  height: number;
  width: number;
  mode: number;
  numInferenceSteps: number;
  guidanceScale: number;
  strength: number;
  faceImage: boolean;
  backgroundRemover: boolean;
  createdAt: string;
  lastModified: string;
  isDeleted: boolean;
  createdById: string;
  lastModifiedById: string;
};

export type Product = {
  id: number;
  name: string;
  createdAt: string;
  lastModified: string;
  createdById: number | null;
  lastModifiedById: number | null;
  productId: string;
  levelMembershipId: number;
  depositId: number;
  couponId: number | null;
  levelMembership: string | null;
  deposit: any | null;
  coupon: string | null;
  discount: number | null;
  discountPercent: number | null;
};

export type Transaction = {
  id: number;
  paymentId: number;
  paymentMethodName: string;
  type: number;
  amount: number;
  createdAt: string;
  orderId: number;
  depositId: number | null;
  description: string | null;
  accountNumber: string | null;
  reference: string | null;
  transactionDateTime: string | null;
};

export type User = {
  id: string;
  fullName: string | null;
  userName: string;
  email: string;
  phoneNumber: string;
  gender: number;
  birthDate: string;
  avatar: string | null;
  role: string;
  isBanned: boolean;
  membershipCard: {
    id: number;
    currentLevel: string;
  };
  location: Location | null;
};

export type Order = {
  id: number;
  code: number;
  status: number;
  amount: number;
  email: string;
  phone: string;
  createdAt: string;

  sessionCode: string;
  paymentMethodName: string;
  couponCode: string;
  locationName: string;
  boothName: string;
  typeSessionName: string;
  user: {
    id: string;
    email: string;
    phoneNumber: string;
  };
};

export type DepositProduct = {
  id: number;
  name: string;
  description: string;
  price: number;
  amountAdd: number;
  createdAt: string;
  lastModified: string;

  productId: string;
};

export type TypeSessionProduct = {
  id: number;
  name: string;
  createdAt: string;
  lastModified: string;
  createdById: string | null;
  lastModifiedById: string | null;
  productId: string;
  levelMembershipId: number | null;
  typeSessionId: number | null;
  couponId: number | null;
  levelMembership: LevelMembership;
  coupon: Coupon;
  typeSession: TypeSession;
  discount: number | null;
};

export type PhotoHistory = {
  id: number;
  url: string;
  photoStyleName: string;
};

export type Coordinate = {
  id: number;
  frameId: number;
  x: number;
  y: number;
  width: number;
  height: number;
};

export type Payment = {
  id: number;
  code: number;
  orderId: number | null;
  depositId: number | null;
  paymentMethodId: number;
  status: number;
  amount: number;
  paymentMethodName: string;
  orderCode: string | null;
  createdAt: string;
  lastModified: string;
};
