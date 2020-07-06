import { Dispatch } from "redux";

import {
    GET_ENTRANTS,
    ADD_TEST_RUN,
    EventActionTypes,
    UPDATE_TEST_RUN_STATE,
    GET_TESTS,
} from "./types";
import { TestRun, TestRunUploadState } from "../../types/models";
import { getEntrants } from "../../api/entrants";
import { AppState } from "..";
import { addTestRun } from "../../api/testRuns";
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

export const AddTestRun = (testRun: TestRun): EventActionTypes => ({
    type: ADD_TEST_RUN,
    payload: testRun,
});

export const UpdateTestRunState = (
    testRunId: number,
    state: TestRunUploadState
): EventActionTypes => ({
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
