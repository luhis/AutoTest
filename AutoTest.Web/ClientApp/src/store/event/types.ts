import { Entrant, LoadingState } from "../../types/models";

export interface EventState {
    readonly entrants: LoadingState<readonly Entrant[]>;
}

export const GET_ENTRANTS = "GET_ENTRANTS";
export const ADD_TEST_RUN = "ADD_TEST_RUN";

interface GetEntrants {
    type: typeof GET_ENTRANTS;
    payload: LoadingState<readonly Entrant[]>;
}

export type EventActionTypes = GetEntrants;
