import {
    Entrant,
    Event,
    TestRun,
    TestRunUploadState,
    Test,
    TestRunTemp,
} from "../../types/models";
import { LoadingState } from "../../types/loadingState";

export interface EventState {
    readonly events: LoadingState<readonly Event[]>;
    readonly entrants: LoadingState<readonly Entrant[]>;
    readonly testRuns: readonly (TestRun & {
        state: TestRunUploadState;
        eventId: number;
    })[];
    readonly tests: LoadingState<readonly Test[]>;
    readonly testRunsFromServer: LoadingState<readonly TestRun[]>;
}

export const GET_ENTRANTS = "GET_ENTRANTS";
export const GET_EVENTS = "GET_EVENTS";
export const GET_TESTS = "GET_TESTS";
export const GET_TEST_RUNS = "GET_TEST_RUNS";
export const ADD_TEST_RUN = "ADD_TEST_RUN";
export const UPDATE_TEST_RUN_STATE = "UPDATE_TEST_RUN_STATE";

interface GetEntrants {
    type: typeof GET_ENTRANTS;
    payload: LoadingState<readonly Entrant[]>;
}

interface GetEvents {
    type: typeof GET_EVENTS;
    payload: LoadingState<readonly Event[]>;
}

interface GetTests {
    type: typeof GET_TESTS;
    payload: LoadingState<readonly Test[]>;
}

interface GetTestRuns {
    type: typeof GET_TEST_RUNS;
    payload: LoadingState<readonly TestRun[]>;
}

interface AddTestRun {
    type: typeof ADD_TEST_RUN;
    payload: TestRunTemp;
}

interface UpdateTestRunState {
    type: typeof UPDATE_TEST_RUN_STATE;
    payload: { testRunId: number; state: TestRunUploadState };
}

export type EventActionTypes =
    | GetEntrants
    | GetTests
    | GetTestRuns
    | GetEvents
    | AddTestRun
    | UpdateTestRunState;
