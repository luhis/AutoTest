import { ValidDate } from "ts-date";

import { EmergencyContact, Vehicle } from "./shared";

export interface Profile {
    readonly profileId: number;
    readonly givenName: string;
    readonly familyName: string;
    readonly msaLicense: string;
    readonly vehicle: Vehicle;
    readonly emergencyContact: EmergencyContact;
    readonly clubMemberships: readonly ClubMembership[];
}

export interface ClubMembership {
    readonly clubName: string;
    readonly membershipNumber: string;
    readonly expiry: ValidDate;
}

export interface Access {
    readonly isLoggedIn: boolean;
    readonly canViewClubs: boolean;
    readonly canViewProfile: boolean;
}
