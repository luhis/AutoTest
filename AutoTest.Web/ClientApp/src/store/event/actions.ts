import { Dispatch, ActionCreator } from "redux";

import {
    GET_ENTRANTS,
    ADD_TEST_RUN,
    EventActionTypes,
    UPDATE_TEST_RUN_STATE,
    GET_TEST_RUNS,
    GET_EVENTS,
    GET_CLUBS,
    SET_PAID,
    DELETE_ENTRANT,
} from "./types";
import {
    TestRunUploadState,
    TestRunTemp,
    Entrant,
    EditingClub,
    Event,
} from "../../types/models";
import {
    addEntrant,
    deleteEntrant,
    getEntrants,
    markPaid,
} from "../../api/entrants";
import { AppState } from "..";
import { addTestRun, getTestRuns } from "../../api/testRuns";
import {
    requiresLoading,
    idsMatch,
    isStale,
    ifLoaded,
} from "../../types/loadingState";
import { addEvent, deleteEvent, getEvents } from "../../api/events";
import { getClubs, addClub, deleteClub } from "../../api/clubs";
import { distinct } from "../../lib/array";

export const GetClubsIfRequired = (token: string | undefined) => async (
    dispatch: Dispatch<EventActionTypes>,
    getState: () => AppState
) => {
    const clubs = getState().event.clubs;
    if (requiresLoading(clubs.tag) || isStale(clubs)) {
        await GetClubs(token)(dispatch);
    }
};

export const AddClub = (
    club: EditingClub,
    token: string | undefined,
    onSuccess: () => void
) => async (dispatch: Dispatch<EventActionTypes>) => {
    await addClub(club, token);
    dispatch({
        type: GET_CLUBS,
        payload: { tag: "Loading", id: undefined },
    });
    dispatch({
        type: GET_CLUBS,
        payload: await getClubs(token),
    });
    onSuccess();
};

export const DeleteClub = (clubId: number, token: string | undefined) => async (
    dispatch: Dispatch<EventActionTypes>
) => {
    await deleteClub(clubId, token);
    dispatch({
        type: GET_CLUBS,
        payload: { tag: "Loading", id: undefined },
    });
    dispatch({
        type: GET_CLUBS,
        payload: await getClubs(token),
    });
};

const GetClubs = (token: string | undefined) => async (
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

export const AddEntrant = (
    entrant: Entrant,
    token: string | undefined,
    onSuccess: () => void
) => async (dispatch: Dispatch<EventActionTypes>) => {
    await addEntrant(entrant, token);
    await GetEntrants(entrant.eventId, token)(dispatch);
    onSuccess();
};

const GetEntrants = (eventId: number, token: string | undefined) => async (
    dispatch: Dispatch<EventActionTypes>
) => {
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

export const AddEvent = (
    event: Event,
    token: string | undefined,
    onSuccess: () => void
) => async (dispatch: Dispatch<EventActionTypes>) => {
    await addEvent(event, token);
    await GetEvents()(dispatch);
    onSuccess();
};

export const DeleteEvent = (
    eventId: number,
    token: string | undefined
) => async (dispatch: Dispatch<EventActionTypes>) => {
    await deleteEvent(eventId, token);
    await GetEvents()(dispatch);
};

const GetEvents = () => async (dispatch: Dispatch<EventActionTypes>) => {
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

export const SetPaid = (
    { eventId, entrantId }: Entrant,
    isPaid: boolean,
    token: string | undefined
) => async (dispatch: Dispatch<EventActionTypes>) => {
    await markPaid(eventId, entrantId, isPaid, token);
    dispatch({
        type: SET_PAID,
        payload: { entrantId, isPaid },
    });
};

export const DeleteEntrant = (
    { eventId, entrantId }: Entrant,
    token: string | undefined
) => async (dispatch: Dispatch<EventActionTypes>) => {
    await deleteEntrant(eventId, entrantId, token);
    dispatch({
        type: DELETE_ENTRANT,
        payload: { entrantId },
    });
};

const UpdateTestRunState: ActionCreator<EventActionTypes> = (
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
            const res = await addTestRun(element.eventId, element, token);
            ifLoaded(res, () => {
                dispatch(
                    UpdateTestRunState(
                        element.testRunId,
                        TestRunUploadState.Uploaded
                    )
                );
            });
        })
    );
    await Promise.all(eventIds.map((a) => GetTestRuns(a, token)));
};
