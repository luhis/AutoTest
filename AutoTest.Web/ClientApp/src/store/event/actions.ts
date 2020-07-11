import { Dispatch, ActionCreator } from "redux";

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
import { requiresLoading, idsMatch } from "../../types/loadingState";

export const GetEntrants = (
    eventId: number,
    token: string | undefined
) => async (dispatch: Dispatch<EventActionTypes>, getState: () => AppState) => {
    const entrants = getState().event.entrants;
    if (requiresLoading(entrants.tag) || !idsMatch(entrants, eventId)) {
        dispatch({
            type: GET_ENTRANTS,
            payload: { tag: "Loading", id: eventId },
        });
        const entrants = await getEntrants(eventId, token);
        dispatch({
            type: GET_ENTRANTS,
            payload: entrants,
        });
    }
};

export const GetTests = (eventId: number, token: string | undefined) => async (
    dispatch: Dispatch<EventActionTypes>,
    getState: () => AppState
) => {
    const tests = getState().event.tests;
    if (requiresLoading(tests.tag) || !idsMatch(tests, eventId)) {
        dispatch({
            type: GET_TESTS,
            payload: { tag: "Loading", id: eventId },
        });
        const tests = await getTests(eventId, token);
        dispatch({
            type: GET_TESTS,
            payload: tests,
        });
    }
};

export const GetTestRuns = (
    eventId: number,
    token: string | undefined
) => async (dispatch: Dispatch<EventActionTypes>, getState: () => AppState) => {
    const tests = getState().event.testRunsFromServer;
    if (requiresLoading(tests.tag) || !idsMatch(tests, eventId)) {
        dispatch({
            type: GET_TEST_RUNS,
            payload: { tag: "Loading", id: eventId },
        });
        const tests = await getTestRuns(eventId, token);
        dispatch({
            type: GET_TEST_RUNS,
            payload: tests,
        });
    }
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
