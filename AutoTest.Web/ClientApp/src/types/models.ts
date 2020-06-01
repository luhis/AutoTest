import { GoogleUser } from "react-use-googlelogin/dist/types";

export type ApiResponse<T> =
    | { tag: "Loaded"; value: T }
    | { tag: "Error"; value: string };

export type LoadingState<T> = ApiResponse<T> | { tag: "Loading" };

export interface GoogleAuth {
    signIn: () => Promise<GoogleUser>;
    googleUser: GoogleUser | null;
}

export interface Access {
    canViewClubs: boolean;
}
