import { Entrant, LoadingState } from "../../types/models";

export interface EventState {
    readonly entrants: LoadingState<readonly Entrant[]>;
}

export const GET_ENTRANTS = "GET_ENTRANTS";

interface GetEntrants {
    type: typeof GET_ENTRANTS;
}

export type EventActionTypes = GetEntrants;
