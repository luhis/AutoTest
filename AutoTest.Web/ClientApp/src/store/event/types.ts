import { Entrant, TestRun, TestRunUploadState, Test } from "../../types/models";
import { LoadingState } from "../../types/loadingState";

export interface EventState {
    readonly entrants: LoadingState<readonly Entrant[]>;
    readonly testRuns: readonly (TestRun & { state: TestRunUploadState })[];
    readonly tests: LoadingState<readonly Test[]>;
}

export const GET_ENTRANTS = "GET_ENTRANTS";
export const GET_TESTS = "GET_TESTS";
export const ADD_TEST_RUN = "ADD_TEST_RUN";
export const UPDATE_TEST_RUN_STATE = "UPDATE_TEST_RUN_STATE";

interface GetEntrants {
    type: typeof GET_ENTRANTS;
    payload: LoadingState<readonly Entrant[]>;
}

interface GetTests {
    type: typeof GET_TESTS;
    payload: LoadingState<readonly Test[]>;
}

interface AddTestRun {
    type: typeof ADD_TEST_RUN;
    payload: TestRun;
}

interface UpdateTestRunState {
    type: typeof UPDATE_TEST_RUN_STATE;
    payload: { testRunId: number; state: TestRunUploadState };
}

export type EventActionTypes =
    | GetEntrants
    | GetTests
    | AddTestRun
    | UpdateTestRunState;
