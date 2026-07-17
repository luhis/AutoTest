import { LoadingState } from "../../types/loadingState";
import {
  TestRunFromClient,
  TestRunFromServer,
  TestRunUploadState,
} from "../../types/models";
import { SharedActionTypes } from "../shared/types";

export interface RunState {
  readonly testRuns: readonly TestRunFromClient[];
  readonly testRunsFromServer: LoadingState<
    readonly TestRunFromServer[],
    { readonly eventId: number; readonly ordinal: number }
  >;
}

interface GetTestRuns {
  readonly type: "GET_TEST_RUNS";
  readonly payload: LoadingState<
    readonly TestRunFromServer[],
    { readonly eventId: number; readonly ordinal: number }
  >;
}

interface AddTestRun {
  readonly type: "ADD_TEST_RUN";
  readonly payload: TestRunFromClient;
}

interface UpdateTestRun {
  readonly type: "UPDATE_TEST_RUN";
  readonly payload: TestRunFromServer;
}

interface UpdateTestRunState {
  readonly type: "UPDATE_TEST_RUN_STATE";
  readonly payload: {
    readonly testRunId: number;
    readonly state: TestRunUploadState;
  };
}

export type RunActionTypes =
  | GetTestRuns
  | AddTestRun
  | UpdateTestRun
  | UpdateTestRunState
  | SharedActionTypes;
