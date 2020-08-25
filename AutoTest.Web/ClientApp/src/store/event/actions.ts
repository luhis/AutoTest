import { Dispatch, ActionCreator } from "redux";

import {
    GET_ENTRANTS,
    ADD_TEST_RUN,
    EventActionTypes,
    UPDATE_TEST_RUN_STATE,
    GET_TEST_RUNS,
    GET_EVENTS,
    GET_CLUBS,
} from "./types";
import { TestRunUploadState, TestRunTemp } from "../../types/models";
import { getEntrants } from "../../api/entrants";
import { AppState } from "..";
import { addTestRun, getTestRuns } from "../../api/testRuns";
import { requiresLoading, idsMatch, isStale } from "../../types/loadingState";
import { getEvents } from "../../api/events";
import { getClubs } from "../../api/clubs";
import { distinct } from "../../lib/array";

export const GetClubsIfRequired = (token: string | undefined) => async (
    dispatch: Dispatch<EventActionTypes>,
    getState: () => AppState
) => {
    const clubs = getState().event.clubs;
    if (requiresLoading(clubs.tag)) {
        await GetClubs(token)(dispatch);
    }
};

export const GetClubs = (token: string | undefined) => async (
    dispatch: Dispatch<EventActionTypes>
) => {
    dispatch({
        type: GET_CLUBS,
        payload: { tag: "Loading", id: undefined },
    });
    dispatch({
        type: GET_CLUBS,
        payload: await getClubs(token),
    });
};

export const GetEntrantsIfRequired = (
    eventId: number,
    token: string | undefined
) => async (dispatch: Dispatch<EventActionTypes>, getState: () => AppState) => {
    const entrants = getState().event.entrants;
    if (
        requiresLoading(entrants.tag) ||
        !idsMatch(entrants, eventId) ||
        isStale(entrants)
    ) {
        await GetEntrants(eventId, token)(dispatch);
    }
};

export const GetEntrants = (
    eventId: number,
    token: string | undefined
) => async (dispatch: Dispatch<EventActionTypes>) => {
    dispatch({
        type: GET_ENTRANTS,
        payload: { tag: "Loading", id: eventId },
    });
    dispatch({
        type: GET_ENTRANTS,
        payload: await getEntrants(eventId, token),
    });
};

export const GetEventsIfRequired = () => async (
    dispatch: Dispatch<EventActionTypes>,
    getState: () => AppState
) => {
    const events = getState().event.events;
    if (requiresLoading(events.tag) || isStale(events)) {
        await GetEvents()(dispatch);
    }
};

export const GetEvents = () => async (dispatch: Dispatch<EventActionTypes>) => {
    dispatch({
        type: GET_EVENTS,
        payload: { tag: "Loading", id: undefined },
    });
    dispatch({
        type: GET_EVENTS,
        payload: await getEvents(),
    });
};

export const GetTestRunsIfRequired = (
    eventId: number,
    token: string | undefined
) => async (dispatch: Dispatch<EventActionTypes>, getState: () => AppState) => {
    const testRuns = getState().event.testRunsFromServer;
    if (
        requiresLoading(testRuns.tag) ||
        !idsMatch(testRuns, eventId) ||
        isStale(testRuns)
    ) {
        await GetTestRuns(eventId, token)(dispatch);
    }
};

const GetTestRuns = (eventId: number, token: string | undefined) => async (
    dispatch: Dispatch<EventActionTypes>
) => {
    dispatch({
        type: GET_TEST_RUNS,
        payload: { tag: "Loading", id: eventId },
    });
    dispatch({
        type: GET_TEST_RUNS,
        payload: await getTestRuns(eventId, token),
    });
};

export const AddTestRun = (
    testRun: TestRunTemp,
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
    const eventIds = distinct(runs.map((a) => a.eventId));
    await Promise.all(
        toUpload.map(async (element) => {
            const res = await addTestRun(
                element.eventId,
                element.ordinal,
                element,
                token
            );
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
    await Promise.all(eventIds.map((a) => GetTestRuns(a, token)));
};
