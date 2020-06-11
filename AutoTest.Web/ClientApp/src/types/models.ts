import { GoogleUser } from "react-use-googlelogin/dist/types";
import { ValidDate } from "ts-date/locale/en";

export type ApiResponse<T> =
    | { readonly tag: "Loaded"; readonly value: T }
    | { readonly tag: "Error"; readonly value: string };

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
    readonly signIn: () => Promise<GoogleUser>;
    readonly googleUser: GoogleUser | null;
}

export interface Access {
    readonly canViewClubs: boolean;
}

export interface Club {
    readonly clubId: number;
    readonly clubName: string;
    readonly clubPaymentAddress: string;
    readonly website: string;
}

export interface Event {
    readonly eventId: number;
    readonly clubId: number;
    readonly location: string;
    readonly startTime: ValidDate;
}

export type EditableClub = Override<
    Club,
    { readonly clubId: number | undefined }
>;

export type Override<T, P> = P & Omit<T, keyof P>;
