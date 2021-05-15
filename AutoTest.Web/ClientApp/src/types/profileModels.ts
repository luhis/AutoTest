import {
    ClubMembership,
    EmergencyContact,
    MsaMembership,
    Vehicle,
} from "./shared";

export interface Profile {
    readonly profileId: number;
    readonly givenName: string;
    readonly familyName: string;
    readonly vehicle: Vehicle;
    readonly msaMembership: MsaMembership;
    readonly emergencyContact: EmergencyContact;
    readonly clubMemberships: readonly ClubMembership[];
}

export interface Access {
    readonly isLoggedIn: boolean;
    readonly canViewClubs: boolean;
    readonly canViewProfile: boolean;
}
