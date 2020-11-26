import {
    Entrant,
    Event,
    TestRun,
    TestRunUploadState,
    TestRunTemp,
    Club,
} from "../../types/models";
import { LoadingState } from "../../types/loadingState";

export interface EventState {
    readonly events: LoadingState<readonly Event[]>;
    readonly entrants: LoadingState<readonly Entrant[], number>;
    readonly testRuns: readonly (TestRun & {
        readonly state: TestRunUploadState;
        readonly eventId: number;
    })[];
    readonly testRunsFromServer: LoadingState<readonly TestRun[]>;
    readonly clubs: LoadingState<readonly Club[]>;
}

export const GET_CLUBS = "GET_CLUBS";
export const GET_ENTRANTS = "GET_ENTRANTS";
export const SET_PAID = "SET_PAID";
export const DELETE_ENTRANT = "DELETE_ENTRANT";
export const GET_EVENTS = "GET_EVENTS";
export const GET_TEST_RUNS = "GET_TEST_RUNS";
export const ADD_TEST_RUN = "ADD_TEST_RUN";
export const UPDATE_TEST_RUN_STATE = "UPDATE_TEST_RUN_STATE";

interface GetClubs {
    readonly type: typeof GET_CLUBS;
    readonly payload: LoadingState<readonly Club[]>;
}

interface GetEntrants {
    readonly type: typeof GET_ENTRANTS;
    readonly payload: LoadingState<readonly Entrant[], number>;
}

interface SetPaid {
    readonly type: typeof SET_PAID;
    readonly payload: { readonly entrantId: number; readonly isPaid: boolean };
}

interface DeleteEntrant {
    readonly type: typeof DELETE_ENTRANT;
    readonly payload: { readonly entrantId: number };
}

interface GetEvents {
    readonly type: typeof GET_EVENTS;
    readonly payload: LoadingState<readonly Event[]>;
}

interface GetTestRuns {
    readonly type: typeof GET_TEST_RUNS;
    readonly payload: LoadingState<readonly TestRun[], number>;
}

interface AddTestRun {
    readonly type: typeof ADD_TEST_RUN;
    readonly payload: TestRunTemp;
}

interface UpdateTestRunState {
    readonly type: typeof UPDATE_TEST_RUN_STATE;
    readonly payload: {
        readonly testRunId: number;
        readonly state: TestRunUploadState;
    };
}

export type EventActionTypes =
    | GetClubs
    | GetEntrants
    | SetPaid
    | DeleteEntrant
    | GetTestRuns
    | GetEvents
    | AddTestRun
    | UpdateTestRunState;
