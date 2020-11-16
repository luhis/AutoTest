import { ValidDate } from "ts-date/locale/en";

export interface Access {
    readonly isLoggedIn: boolean;
    readonly canViewClubs: boolean;
    readonly canViewProfile: boolean;
}

export interface AuthorisationEmail {
    readonly email: string;
}

export interface Club {
    readonly clubId: number;
    readonly clubName: string;
    readonly clubPaymentAddress: string;
    readonly website: string;
    readonly adminEmails: readonly AuthorisationEmail[];
}

export type EditingClub = Club & { readonly isNew: boolean };

export interface Test {
    readonly ordinal: number;
    readonly mapLocation: string;
}

export interface TestRun {
    readonly testRunId: number;
    readonly ordinal: number;
    readonly timeInMS: number;
    readonly entrantId: number;
    readonly penalties: readonly Penalty[];
}

export type TestRunTemp = TestRun & { readonly eventId: number };

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

export interface Event {
    readonly eventId: number;
    readonly clubId: number;
    readonly location: string;
    readonly startTime: ValidDate;
    readonly testCount: number;
    readonly maxAttemptsPerTest: number;
    readonly marshalEmails: readonly AuthorisationEmail[];
    readonly tests: readonly Test[];
}

export type EditingEvent = Override<
    Event,
    { readonly clubId: number | undefined }
> & {
    readonly isNew: boolean;
    readonly isClubEditable: boolean;
};

export interface Profile {
    readonly profileId: number;
    readonly givenName: string;
    readonly familyName: string;
    readonly msaLicense: string;
    readonly vehicle: Vehicle;
    readonly emergencyContact: EmergencyContact;
}

export interface Entrant {
    readonly entrantId: number;
    readonly eventId: number;
    readonly driverNumber: number;
    readonly class: string;
    readonly givenName: string;
    readonly familyName: string;
    readonly msaLicense: string;
    readonly vehicle: Vehicle;
    readonly emergencyContact: EmergencyContact;
}

export type EditingEntrant = Omit<Entrant, "driverNumber"> & {
    readonly isNew: boolean;
};

export interface EmergencyContact {
    readonly name: string;
    readonly phone: string;
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

export interface EntrantTime {
    readonly entrant: Entrant;
    readonly totalTime: number;
    readonly times: readonly TestTime[];
}

interface TestTime {
    readonly ordinal: number;
    readonly testRuns: readonly TestRun[];
}

export type Override<T, P> = P & Omit<T, keyof P>;
