
interface LoginRequest {
  email: string;
  password: string;
}
interface TokenResponse {
  message: string;
  token: string;
}

interface ErrorResponse {
  message: string;
}

interface DecodedJWT {
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": string;
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": string;
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/email": string;
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
  email_verified: boolean;
  exp: number;
}

export type { TokenResponse, ErrorResponse, DecodedJWT,LoginRequest };
