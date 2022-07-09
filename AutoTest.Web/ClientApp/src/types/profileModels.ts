import {
    ClubMembership,
    EmergencyContact,
    MsaMembership,
    Vehicle,
} from "./shared";

export enum Age {
    Junior = 0,
    Senior = 1,
}

export interface Profile {
    readonly emailAddress: string;
    readonly givenName: string;
    readonly familyName: string;
    readonly age: Age;
    readonly vehicle: Vehicle;
    readonly msaMembership: MsaMembership;
    readonly emergencyContact: EmergencyContact;
    readonly clubMemberships: readonly ClubMembership[];
}

export interface Access {
    readonly isLoggedIn: boolean;
    readonly isRootAdmin: boolean;
    readonly canViewClubs: boolean;
    readonly canViewProfile: boolean;
    readonly adminClubs: readonly number[];
    readonly marshalEvents: readonly number[];
    readonly editableEntrants: readonly number[];
    readonly editableMarshals: readonly number[];
}
