import { ValidDate } from "ts-date";

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

export interface ClubMembership {
    readonly clubName: string;
    readonly membershipNumber: string;
    readonly expiry: ValidDate;
}

export const neverReached = (_: never) => undefined;
