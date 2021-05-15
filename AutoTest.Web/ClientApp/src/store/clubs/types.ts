import { LoadingState } from "src/types/loadingState";
import { Club } from "src/types/models";
import { SharedActionTypes } from "../shared/types";

export interface ClubsState {
    readonly clubs: LoadingState<readonly Club[]>;
}

export const GET_CLUBS = "GET_CLUBS";
export const ADD_CLUB = "ADD_CLUB";

interface GetClubs {
    readonly type: typeof GET_CLUBS;
    readonly payload: LoadingState<readonly Club[]>;
}

interface AddClub {
    readonly type: typeof ADD_CLUB;
    readonly payload: Club;
}

export type ClubsActionTypes = GetClubs | AddClub | SharedActionTypes;
