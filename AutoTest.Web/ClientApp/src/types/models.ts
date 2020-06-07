import { GoogleUser } from "react-use-googlelogin/dist/types";

export type ApiResponse<T> =
    | { tag: "Loaded"; value: T }
    | { tag: "Error"; value: string };

export type LoadingState<T> = ApiResponse<T> | { tag: "Loading" };

export const toApiResponse = async <T>(
    f: () => Promise<T>
): Promise<ApiResponse<T>> => {
    try {
        return { tag: "Loaded", value: await f() };
    } catch (e) {
        return { tag: "Error", value: "API error" };
    }
};

export interface GoogleAuth {
    signIn: () => Promise<GoogleUser>;
    googleUser: GoogleUser | null;
}

export interface Access {
    canViewClubs: boolean;
}

export interface Club {
    clubId: number;
    clubName: string;
    clubPaymentAddress: string;
    website: string;
}

export type EditableClub = Override<Club, { clubId: number | undefined }>;

type Override<T, P> = P & Omit<T, keyof P>;
