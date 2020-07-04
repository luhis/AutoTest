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

export interface Test {
    readonly testId: number;
    readonly ordinal: number;
}

export interface TestRun {
    readonly testRunId: number;
    readonly testId: number;
    readonly timeInMS: number;
    readonly entrantId: number;
    readonly penalties: readonly Penalty[];
}

export enum PenaltyType {
    Late = 0,
    NoAttendance = 1,
    WrongTest = 2,
    HitBarrier = 3,
    FailToStop = 4,
}

export interface Penalty {
    readonly penaltyType: PenaltyType;
    readonly instanceCount: number;
}

export type EditableTestRun = Override<
    Partial<TestRun>,
    { readonly testRunId: number; readonly penalties: readonly Penalty[] }
>;

export interface User {
    readonly userId: number;
}

export interface Event {
    readonly eventId: number;
    readonly clubId: number;
    readonly location: string;
    readonly startTime: ValidDate;
    readonly testCount: number;
    readonly maxAttemptsPerTest: number;
}

export interface Entrant {
    readonly entrantId: number;
    readonly eventId: number;
    readonly class: string;
    readonly givenName: string;
    readonly familyName: string;
    readonly vehicle: Vehicle;
}

export interface Vehicle {
    readonly make: string;
    readonly model: string;
    readonly year: number;
    readonly registration: string;
    readonly displacement: number;
}

export interface Result {
    readonly totalTime: number;
}

export type Override<T, P> = P & Omit<T, keyof P>;
