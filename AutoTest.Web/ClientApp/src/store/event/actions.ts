import { Dispatch, ActionCreator } from "redux";
import { ThunkAction } from "redux-thunk";

import {
    GET_ENTRANTS,
    ADD_TEST_RUN,
    EventActionTypes,
    UPDATE_TEST_RUN_STATE,
    GET_TESTS,
    GET_TEST_RUNS,
} from "./types";
import { TestRun, TestRunUploadState } from "../../types/models";
import { getEntrants } from "../../api/entrants";
import { AppState } from "..";
import { addTestRun, getTestRuns } from "../../api/testRuns";
import { getTests } from "../../api/tests";

export const GetEntrants = (
    eventId: number,
    token: string | undefined
) => async (dispatch: Dispatch<EventActionTypes>) => {
    dispatch({
        type: GET_ENTRANTS,
        payload: { tag: "Loading" },
    });
    const entrants = await getEntrants(eventId, token);
    dispatch({
        type: GET_ENTRANTS,
        payload: entrants,
    });
};

export const GetTests = (eventId: number, token: string | undefined) => async (
    dispatch: Dispatch<EventActionTypes>
) => {
    dispatch({
        type: GET_TESTS,
        payload: { tag: "Loading" },
    });
    const tests = await getTests(eventId, token);
    dispatch({
        type: GET_TESTS,
        payload: tests,
    });
};

export const GetTestRuns: ActionCreator<ThunkAction<
    Promise<EventActionTypes>,
    AppState,
    undefined,
    EventActionTypes
>> = (eventId: number, token: string | undefined) => async (dispatch) => {
    dispatch({
        type: GET_TEST_RUNS,
        payload: { tag: "Loading" },
    });
    const tests = await getTestRuns(eventId, token);
    return dispatch({
        type: GET_TEST_RUNS,
        payload: tests,
    });
};

export const SetTestsIdle: ActionCreator<EventActionTypes> = () => ({
    type: GET_TESTS,
    payload: { tag: "Idle" },
});

export const SetEntrantsIdle: ActionCreator<EventActionTypes> = () => ({
    type: GET_ENTRANTS,
    payload: { tag: "Idle" },
});

export const AddTestRun = (
    testRun: TestRun,
    token: string | undefined
) => async (dispatch: Dispatch<EventActionTypes>, getState: () => AppState) => {
    dispatch({
        type: ADD_TEST_RUN,
        payload: testRun,
    });
    await SyncTestRuns(token)(dispatch, getState);
};

export const UpdateTestRunState: ActionCreator<EventActionTypes> = (
    testRunId: number,
    state: TestRunUploadState
) => ({
    type: UPDATE_TEST_RUN_STATE,
    payload: { testRunId, state },
});

export const SyncTestRuns = (token: string | undefined) => async (
    dispatch: Dispatch<EventActionTypes>,
    getState: () => AppState
) => {
    const runs = getState().event.testRuns;
    const toUpload = runs.filter(
        (a) => a.state !== TestRunUploadState.Uploaded
    );
    await Promise.all(
        toUpload.map(async (element) => {
            const res = await addTestRun(element, token);
            if (res.tag === "Loaded") {
                dispatch(
                    UpdateTestRunState(
                        element.testRunId,
                        TestRunUploadState.Uploaded
                    )
                );
            }
        })
    );
};
