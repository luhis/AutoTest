import { ValidDate } from "ts-date";

export interface EmergencyContact {
  readonly name: string;
  readonly phone: string;
}

export enum InductionTypes {
  "NA" = 0,
  "Forced" = 1,
}

export interface Vehicle {
  readonly make: string;
  readonly model: string;
  readonly year: number;
  readonly registration: string;
  readonly displacement: number;
  readonly induction: InductionTypes;
}

export interface ClubMembership {
  readonly clubName: string;
  readonly membershipNumber: string;
  readonly expiry: ValidDate;
}

export interface MsaMembership {
  readonly msaLicense: number;
  readonly msaLicenseType: string;
}

export const neverReached = (_: never) => undefined;
