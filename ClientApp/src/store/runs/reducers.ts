import { TestRunUploadState } from "../../types/models";
import { ifLoaded } from "../../types/loadingState";
import { neverReached } from "../../types/shared";
import { RunActionTypes, RunState } from "./types";

const initialState: RunState = {
  testRuns: [],
  testRunsFromServer: { tag: "Idle" },
};

export const runReducer = (
  state = initialState,
  action: RunActionTypes,
): RunState => {
  switch (action.type) {
    case "CLEAR_CACHE":
      return {
        ...state,
        testRunsFromServer: { tag: "Idle" },
      };
    case "ADD_TEST_RUN":
      return {
        ...state,
        testRuns: state.testRuns.concat({
          ...action.payload,
          state: TestRunUploadState.NotSent,
        }),
      };
    case "UPDATE_TEST_RUN":
      return {
        ...state,
        testRunsFromServer: ifLoaded(state.testRunsFromServer, (runs) =>
          runs.map((a) =>
            a.testRunId === action.payload.testRunId ? action.payload : a,
          ),
        ),
      };
    case "UPDATE_TEST_RUN_STATE": {
      const found = state.testRuns.find(
        (a) => a.testRunId === action.payload.testRunId,
      );
      return {
        ...state,
        testRuns:
          found === undefined
            ? state.testRuns
            : state.testRuns
                .filter((a) => a.testRunId !== action.payload.testRunId)
                .concat({
                  ...found,
                  state: action.payload.state,
                }),
      };
    }
    case "GET_TEST_RUNS":
      return {
        ...state,
        testRunsFromServer: action.payload,
      };
    default: {
      neverReached(action);
      return state;
    }
  }
};
