import type { AppState } from "..";

export const selectClubs = (a: AppState) => a.clubs.clubs;
