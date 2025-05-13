interface OrderRequest {
  email: string;
  phone: string;
  typeSessionId: number;
  paymentMethodId: number;
  coupon: string;
}

interface OrderResponse {
  paymentLink: string;
  status:number;
  code:number;
}
