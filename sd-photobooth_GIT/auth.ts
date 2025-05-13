import NextAuth from "next-auth";
import Google from "next-auth/providers/google";
// import Credentials from "next-auth/providers/credentials"
import AxiosAPI from "./configs/axios";
import {
  DecodedJWT,
  ErrorResponse,
  TokenResponse,
  LoginRequest,
} from "./types/token";
// import { encrypt } from "./app/helpers/dataEncryption";
import { cookies } from "next/headers";
import { jwtDecode } from "jwt-decode";

import Credentials from "next-auth/providers/credentials";
async function sendTokenToBackend(idToken: string) {
  process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
  try {
    const response = await AxiosAPI.post<{ Token: string }>(
      "api/Auth/google/callback",
      {
        Token: idToken,
      }
    );

    if (response.status >= 200 && response.status < 300 && response.data) {
      const dataReceive = response.data as unknown as TokenResponse;
      return dataReceive.token;
    }

    throw new Error("Failed to retrieve token");
  } catch (error) {
    console.error("An error occurred:", error);
    throw error;
  }
}
export const { handlers, signIn, signOut, auth } = NextAuth({
  providers: [
    Google({
      clientId: process.env.GOOGLE_CLIENT_ID,
      clientSecret: process.env.GOOGLE_CLIENT_SECRET,
    }),

    Credentials({
      credentials: {
        email: {},
        password: {},
      },
      async authorize(credentials) {
        try {
          const loginData: LoginRequest = {
            email: credentials.email as unknown as string,
            password: credentials.password as unknown as string,
          };
          const res = await AxiosAPI.post<TokenResponse>(
            "api/Identity/login",
            loginData as any
          );

          if (!res.data || !res.data.token) {
            throw new Error("Invalid email or password");
          }

          const cookieStore = await cookies();
          const decodedToken = jwtDecode<DecodedJWT>(res.data.token);

          const role =
            decodedToken[
              "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
            ] || "guest";
          if (role !== "Staff" && role !== "Admin" && role !== "Manager") {
            throw new Error("Invalid token");
          }

          cookieStore.set("AccessToken", res.data.token, { path: "/" });

          return {
            id:
              decodedToken[
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
              ] || "",
            name:
              decodedToken[
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
              ] || "",
            email:
              decodedToken[
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/email"
              ] || "",
            role:
              decodedToken[
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
              ] || "guest",
            emailVerified: decodedToken.email_verified ?? null,
            token: res.data.token,
          };
        } catch (error) {
          console.error("Login error:", error);
          return null;
        }
      },
    }),
  ],
  callbacks: {
    async jwt({ token, account, session, user }) {
      if (user) {
        token.id = user.id;
        token.name = user.name;
        token.email = user.email;
        token.role = user.role;
        token.emailVerified = (user as any).emailVerified ?? null;
        token.accessToken = (user as any).token;
      }

      if (account?.id_token) {
        token.accessToken = account.access_token;

        try {
          const access_token = await sendTokenToBackend(account.id_token);

          (await cookies()).set("AccessToken", access_token);

          const decodedJWT = jwtDecode<DecodedJWT>(access_token);

          if (decodedJWT) {
            token.id =
              decodedJWT[
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
              ] || "";
            token.name =
              decodedJWT[
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
              ] || "";
            token.email =
              decodedJWT[
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/email"
              ] || "";
            token.role =
              decodedJWT[
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
              ] || "guest";
            token.emailVerified = decodedJWT["email_verified"] ?? false;
          }
        } catch (error) {
          console.error("An error occurred: ", error);
        }
      }

      return token;
    },

    async session({ session, trigger, token, user }) {
      // console.log("SESSION callback called", { session, trigger, token, user });

      session.user = {
        id: (token.id as string) ?? null,
        name: token.name,
        email: (token.email as string) ?? null,
        role: (token.role as string) ?? null,
        emailVerified: (token.emailVerified as Date) ?? null,
      };

      // console.log("xxx");

      if (trigger === "update") {
        // console.log("xxx");
        if (session) {
          return session;
        }
      }
      return session;
    },
  },
  secret: process.env.NEXTAUTH_SECRET,
});
