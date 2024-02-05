import { ThunkAction } from "redux-thunk";
import { ActionCreator } from "redux";
import { toast } from "bulma-toast";

import { RunActionTypes } from "./types";
import { AppState } from "..";
import { selectTestRuns, selectTestRunsFromServer } from "./selectors";
import {
  TestRunFromClient,
  TestRunFromServer,
  TestRunUploadState,
} from "../../types/models";
import {
  canUpdate,
  idsMatch,
  ifLoaded,
  isStale,
  requiresLoading,
} from "../../types/loadingState";
import { addTestRun, getTestRuns, updateTestRun } from "../../api/testRuns";

export const GetTestRunsIfRequired =
  (
    eventId: number,
    ordinal: number,
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, RunActionTypes> =>
  async (dispatch, getState) => {
    const testRuns = selectTestRunsFromServer(getState());
    const id = { eventId, ordinal };
    const missMatch = !idsMatch(testRuns, id);
    if (missMatch) {
      dispatch({
        type: "GET_TEST_RUNS",
        payload: { tag: "Loading", id: id },
      });
    }
    if (requiresLoading(testRuns.tag) || isStale(testRuns) || missMatch) {
      await GetTestRuns(eventId, ordinal, token)(dispatch, getState, {});
    }
  };

const GetTestRuns =
  (
    eventId: number,
    ordinal: number,
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, RunActionTypes> =>
  async (dispatch, getState) => {
    const testRuns = selectTestRunsFromServer(getState());
    const res = await getTestRuns(eventId, ordinal, token);
    if (canUpdate(testRuns, res)) {
      dispatch({
        type: "GET_TEST_RUNS",
        payload: res,
      });
    }
  };

export const AddTestRun =
  (
    testRun: TestRunFromClient,
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, RunActionTypes> =>
  async (dispatch, getState) => {
    dispatch({
      type: "ADD_TEST_RUN",
      payload: testRun,
    });
    await SyncTestRuns(testRun.eventId, testRun.ordinal, token)(
      dispatch,
      getState,
      {},
    );
  };

export const UpdateTestRun =
  (
    testRun: TestRunFromServer,
    token: string | undefined,
    onSuccess: () => void,
  ): ThunkAction<Promise<void>, AppState, unknown, RunActionTypes> =>
  async (dispatch) => {
    await updateTestRun(testRun, token);
    onSuccess();
    dispatch({
      type: "UPDATE_TEST_RUN",
      payload: testRun,
    });
  };

export const SyncTestRuns =
  (
    eventId: number,
    ordinal: number,
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, RunActionTypes> =>
  async (dispatch, getState) => {
    const runs = selectTestRuns(getState());
    const toUpload = runs.filter(
      (a) => a.state !== TestRunUploadState.Uploaded,
    );
    await Promise.all(
      toUpload.map(async (element) => {
        try {
          const res = await addTestRun(element, token);
          ifLoaded(res, () => {
            dispatch(
              UpdateTestRunState(
                element.testRunId,
                TestRunUploadState.Uploaded,
              ),
            );
          });
        } catch (error) {
          toast({ message: (error as Error).message, type: "is-danger" });
        }
      }),
    );

    await GetTestRuns(eventId, ordinal, token)(dispatch, getState, {});
  };

const UpdateTestRunState: ActionCreator<RunActionTypes> = (
  testRunId: number,
  state: TestRunUploadState,
) => ({
  type: "UPDATE_TEST_RUN_STATE",
  payload: { testRunId, state },
});
