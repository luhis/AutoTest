import { AppState } from "..";

export const selectProfile = (a: AppState) => a.profile.profile;

export const selectAccess = (a: AppState) => a.profile.access;
