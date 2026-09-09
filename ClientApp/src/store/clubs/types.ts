import type { LoadingState } from "src/types/loadingState";
import type { Club } from "src/types/models";
import type { SharedActionTypes } from "../shared/types";

export interface ClubsState {
  readonly clubs: LoadingState<readonly Club[]>;
}

interface GetClubs {
  readonly type: "GET_CLUBS";
  readonly payload: LoadingState<readonly Club[]>;
}

interface AddClub {
  readonly type: "ADD_CLUB";
  readonly payload: Club;
}

export type ClubsActionTypes = GetClubs | AddClub | SharedActionTypes;
