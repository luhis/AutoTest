import { ValidDate } from "ts-date";
import { Age } from "./profileModels";

import { EmergencyContact, MsaMembership, Vehicle } from "./shared";

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

export interface TestRunFromServer {
    readonly eventId: number;
    readonly testRunId: number;
    readonly ordinal: number;
    readonly timeInMS: number;
    readonly entrantId: number;
    readonly marshalId: number;
    readonly created: ValidDate;
    readonly penalties: readonly Penalty[];
}

export type TestRunFromClient = Omit<TestRunFromServer, "marshalId"> & {
    readonly state: TestRunUploadState;
};

export enum PenaltyType {
    Late = 0,
    NoAttendance = 1,
    WrongTest = 2,
    HitBarrier = 3,
    FailToStop = 4,
}

export enum EventType {
    AutoTest = 0,
    AutoSolo = 1,
    PCA = 2,
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
    Partial<Omit<TestRunFromClient, "created">>,
    {
        readonly testRunId: number;
        readonly ordinal: number;
        readonly penalties: readonly Penalty[];
        readonly state: TestRunUploadState;
    }
>;

export interface Event {
    readonly eventId: number;
    readonly clubId: number;
    readonly location: string;
    readonly startTime: ValidDate;
    readonly testCount: number;
    readonly maxAttemptsPerTest: number;
    readonly tests: readonly Test[];
    readonly regulations: string | null;
    readonly maps: string | null;
    readonly eventType: EventType;
}

export type EditingEvent = Override<
    Event,
    { readonly clubId: number | undefined }
> & {
    readonly isNew: boolean;
    readonly isClubEditable: boolean;
};

export interface PublicEntrant {
    readonly entrantId: number;
    readonly eventId: number;
    readonly class: string;
    readonly givenName: string;
    readonly familyName: string;
    readonly age: Age;
    readonly vehicle: Vehicle;
    readonly driverNumber: number;
    readonly club: string;
    readonly isPaid: boolean;
}

export type Entrant = {
    readonly msaMembership: MsaMembership;
    readonly emergencyContact: EmergencyContact;
    readonly clubNumber: number;
    readonly email: string;
} & PublicEntrant;

export type EditingEntrant = Override<
    Omit<Entrant, "driverNumber">,
    {
        readonly isNew: boolean;
    }
>;

export interface PublicMarshal {
    readonly marshalId: number;
    readonly eventId: number;
    readonly givenName: string;
    readonly familyName: string;
    readonly role: string;
}

export type Marshal = {
    readonly emergencyContact: EmergencyContact;
    readonly registrationNumber: number;
    readonly email: string;
} & PublicMarshal;

export type EditingMarshal = Override<
    Marshal,
    {
        readonly isNew: boolean;
    }
>;

export interface Result {
    readonly class: string;
    readonly entrantTimes: readonly EntrantTime[];
}

export interface EntrantTime {
    readonly entrant: Entrant;
    readonly totalTime: number;
    readonly times: readonly TestTime[];
    readonly position: number;
    readonly classPosition: number;
}

export interface EventNotification {
    readonly notificationId: number;
    readonly eventId: number;
    readonly message: string;
    readonly created: ValidDate;
}

interface TestTime {
    readonly ordinal: number;
    readonly testRuns: readonly TestRunFromServer[];
}

export type Override<T, P> = P & Omit<T, keyof P>;

export interface MakeAndModel {
    readonly make: string;
    readonly model: string;
}
