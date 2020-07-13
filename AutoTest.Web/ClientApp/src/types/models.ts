import { GoogleUser } from "react-use-googlelogin/dist/types";
import { ValidDate } from "ts-date/locale/en";

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
    readonly adminEmails: readonly string[];
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

export enum TestRunUploadState {
    NotSent,
    Error,
    Uploaded,
}

export interface Penalty {
    readonly penaltyType: PenaltyType;
    readonly instanceCount: number;
}

export type EditableTestRun = Override<
    Partial<Omit<TestRun, "created">>,
    {
        readonly testRunId: number;
        readonly penalties: readonly Penalty[];
    }
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
    readonly marshalEmails: readonly string[];
}

export interface Entrant {
    readonly entrantId: number;
    readonly eventId: number;
    readonly driverNumber: number;
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
    readonly class: string;
    readonly entrantTimes: readonly EntrantTime[];
}

interface EntrantTime {
    readonly entrant: Entrant;
    readonly totalTime: number;
    readonly times: readonly TestTime[];
}

interface TestTime {
    readonly ordinal: number;
    readonly timesInMs: readonly number[];
}

export type Override<T, P> = P & Omit<T, keyof P>;
