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

export interface Course {
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
  HitBarrier = 0,
  FailToStop = 1,
  WrongTest = 2,
  Late = 3,
  NoAttendance = 4,
}

export enum EventType {
  AutoTest = 0,
  PCA = 1,
  AutoSolo = 2,
  Trial = 3,
  Sprint = 4,
}

export enum EntrantStatus {
  Entered = 0,
  Withdrawn = 1,
  Reserve = 2,
}

export enum EventStatus {
  Open = 0,
  Running = 1,
  Provisional = 2,
  Finalised = 3,
}

export enum TestRunUploadState {
  NotSent,
  Error,
  Uploaded,
}

export enum PaymentMethod {
  Bacs = 0,
  PayPal = 1,
  Complementary = 2,
}

export enum TimingSystem {
  StopWatch = 0,
  App = 1,
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
    readonly timeInMS: string;
  }
>;

export interface Event {
  readonly eventId: number;
  readonly clubId: number;
  readonly location: string;
  readonly startTime: ValidDate;
  readonly courseCount: number;
  readonly maxAttemptsPerCourse: number;
  readonly maxEntrants: number;
  readonly courses: readonly Course[];
  readonly regulations: string | null;
  readonly maps: string | null;
  readonly eventTypes: readonly EventType[];
  readonly entryOpenDate: ValidDate;
  readonly entryCloseDate: ValidDate;
  readonly timingSystem: TimingSystem;
  readonly eventStatus: EventStatus;
  readonly created: ValidDate;
}

export type EditingEvent = Override<
  Omit<Event, "created">,
  {
    readonly clubId: number | undefined;
    readonly startTime: string;
    readonly entryOpenDate: string;
    readonly entryCloseDate: string;
  }
> & {
  readonly isNew: boolean;
  readonly isClubEditable: boolean;
};

export interface Payment {
  readonly paidAt: ValidDate;
  readonly method: PaymentMethod;
  readonly timestamp: ValidDate;
}

type EntrantClub = { readonly club: string; readonly clubNumber: string };

export interface PublicEntrant {
  readonly entrantId: number;
  readonly eventId: number;
  readonly class: string;
  readonly givenName: string;
  readonly familyName: string;
  readonly age: Age;
  readonly vehicle: Vehicle;
  readonly driverNumber: number;
  readonly entrantClub: Omit<EntrantClub, "clubNumber">;
  readonly payment: Payment | null;
  readonly entrantStatus: EntrantStatus;
}

export type AcceptDeclaration = {
  readonly isAccepted: boolean;
  readonly email: string;
  readonly timeStamp: ValidDate;
};

export type Entrant = {
  readonly entrantClub: EntrantClub;
  readonly msaMembership: MsaMembership;
  readonly emergencyContact: EmergencyContact;
  readonly email: string;
  readonly acceptDeclaration: AcceptDeclaration | null;
  readonly isLady: boolean;
  readonly doubleDrivenWith: number | null;
} & Omit<PublicEntrant, "club">;

export type SaveEntrant = Omit<Entrant, "entrantStatus">; // todo can i remove driver number?

export type EditingEntrant = Override<
  Omit<Entrant, "driverNumber" | "entrantStatus">,
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
  readonly acceptDeclaration: AcceptDeclaration | null;
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
