import { Dispatch, ActionCreator } from "redux";

import {
    GET_ENTRANTS,
    ADD_ENTRANT,
    ADD_TEST_RUN,
    EventActionTypes,
    UPDATE_TEST_RUN_STATE,
    GET_TEST_RUNS,
    GET_EVENTS,
    SET_PAID,
    DELETE_ENTRANT,
    DELETE_EVENT,
    ADD_EVENT,
    GET_NOTIFICATIONS,
    ADD_NOTIFICATION,
    ADD_MARSHAL,
    DELETE_MARSHAL,
    GET_MARSHALS,
} from "./types";
import {
    TestRunUploadState,
    TestRunTemp,
    Entrant,
    Event,
    Notification,
    Marshal,
} from "../../types/models";
import {
    addEntrant,
    deleteEntrant,
    getEntrants,
    markPaid,
} from "../../api/entrants";
import { addMarshal, deleteMarshal, getMarshals } from "../../api/marshals";
import { AppState } from "..";
import { addTestRun, getTestRuns } from "../../api/testRuns";
import {
    requiresLoading,
    idsMatch,
    isStale,
    ifLoaded,
    canUpdate,
} from "../../types/loadingState";
import { addEvent, deleteEvent, getEvents } from "../../api/events";
import { addNotification, getNotifications } from "../../api/notifications";
import {
    selectEntrants,
    selectEvents,
    selectMarshals,
    selectTestRuns,
    selectTestRunsFromServer,
} from "./selectors";
import { CLEAR_CACHE } from "../shared/types";

export const GetMarshalsIfRequired =
    (eventId: number, token: string | undefined) =>
    async (dispatch: Dispatch<EventActionTypes>, getState: () => AppState) => {
        const clubs = selectMarshals(getState());
        if (requiresLoading(clubs.tag) || isStale(clubs)) {
            if (clubs.tag === "Idle") {
                dispatch({
                    type: GET_MARSHALS,
                    payload: { tag: "Loading", id: eventId },
                });
            }
            const res = await getMarshals(eventId, token);
            if (canUpdate(clubs, res)) {
                dispatch({
                    type: GET_MARSHALS,
                    payload: res,
                });
            }
        }
    };

export const ClearCache = () => ({
    type: CLEAR_CACHE,
});

export const GetEntrantsIfRequired =
    (eventId: number, token: string | undefined) =>
    async (dispatch: Dispatch<EventActionTypes>, getState: () => AppState) => {
        const entrants = selectEntrants(getState());
        if (!idsMatch(entrants, eventId)) {
            dispatch({
                type: GET_ENTRANTS,
                payload: { tag: "Loading", id: eventId },
            });
        }
        if (requiresLoading(entrants.tag) || isStale(entrants)) {
            const res = await getEntrants(eventId, token);
            if (canUpdate(entrants, res)) {
                dispatch({
                    type: GET_ENTRANTS,
                    payload: res,
                });
            }
        }
    };

export const AddEntrant =
    (entrant: Entrant, token: string | undefined, onSuccess: () => void) =>
    async (dispatch: Dispatch<EventActionTypes>) => {
        const newEntrant = await addEntrant(entrant, token);
        dispatch({
            type: ADD_ENTRANT,
            payload: newEntrant,
        });
        onSuccess();
    };

export const AddMarshal =
    (marshal: Marshal, token: string | undefined, onSuccess: () => void) =>
    async (dispatch: Dispatch<EventActionTypes>) => {
        const newEntrant = await addMarshal(marshal, token);
        dispatch({
            type: ADD_MARSHAL,
            payload: newEntrant,
        });
        onSuccess();
    };

export const GetEventsIfRequired =
    () =>
    async (dispatch: Dispatch<EventActionTypes>, getState: () => AppState) => {
        const events = selectEvents(getState());
        if (requiresLoading(events.tag) || isStale(events)) {
            if (events.tag === "Idle") {
                dispatch({
                    type: GET_EVENTS,
                    payload: { tag: "Loading", id: undefined },
                });
            }
            const res = await getEvents();
            if (canUpdate(events, res)) {
                dispatch({
                    type: GET_EVENTS,
                    payload: res,
                });
            }
        }
    };

export const GetNotifications =
    (eventId: number) => async (dispatch: Dispatch<EventActionTypes>) => {
        dispatch({
            type: GET_NOTIFICATIONS,
            payload: { tag: "Loading", id: eventId },
        });
        dispatch({
            type: GET_NOTIFICATIONS,
            payload: await getNotifications(eventId),
        });
    };

export const AddNotification = (notification: Notification) => ({
    type: ADD_NOTIFICATION,
    payload: notification,
});

export const CreateNotification =
    (notification: Notification, token: string | undefined) =>
    async (dispatch: Dispatch<EventActionTypes>) => {
        await addNotification(notification, token);
        dispatch({
            type: ADD_NOTIFICATION,
            payload: notification,
        });
    };

export const AddEvent =
    (event: Event, token: string | undefined, onSuccess: () => void) =>
    async (dispatch: Dispatch<EventActionTypes>) => {
        await addEvent(event, token);
        dispatch({
            type: ADD_EVENT,
            payload: { event },
        });
        onSuccess();
    };

export const DeleteEvent =
    (eventId: number, token: string | undefined) =>
    async (dispatch: Dispatch<EventActionTypes>) => {
        await deleteEvent(eventId, token);
        dispatch({
            type: DELETE_EVENT,
            payload: { eventId },
        });
    };

export const GetTestRunsIfRequired =
    (eventId: number, ordinal: number, token: string | undefined) =>
    async (dispatch: Dispatch<EventActionTypes>, getState: () => AppState) => {
        const testRuns = selectTestRunsFromServer(getState());
        const id = { eventId, ordinal };
        const missMatch = !idsMatch(testRuns, id);
        if (missMatch) {
            dispatch({
                type: GET_TEST_RUNS,
                payload: { tag: "Loading", id: id },
            });
        }
        if (requiresLoading(testRuns.tag) || isStale(testRuns) || missMatch) {
            await GetTestRuns(eventId, ordinal, token)(dispatch, getState);
        }
    };

const GetTestRuns =
    (eventId: number, ordinal: number, token: string | undefined) =>
    async (dispatch: Dispatch<EventActionTypes>, getState: () => AppState) => {
        const testRuns = selectTestRunsFromServer(getState());
        const res = await getTestRuns(eventId, ordinal, token);
        if (canUpdate(testRuns, res)) {
            dispatch({
                type: GET_TEST_RUNS,
                payload: res,
            });
        }
    };

export const AddTestRun =
    (testRun: TestRunTemp, token: string | undefined) =>
    async (dispatch: Dispatch<EventActionTypes>, getState: () => AppState) => {
        dispatch({
            type: ADD_TEST_RUN,
            payload: testRun,
        });
        await SyncTestRuns(
            testRun.eventId,
            testRun.ordinal,
            token
        )(dispatch, getState);
    };

export const SetPaid =
    (
        { eventId, entrantId }: Entrant,
        isPaid: boolean,
        token: string | undefined
    ) =>
    async (dispatch: Dispatch<EventActionTypes>) => {
        await markPaid(eventId, entrantId, isPaid, token);
        dispatch({
            type: SET_PAID,
            payload: { entrantId, isPaid },
        });
    };

export const DeleteEntrant =
    ({ eventId, entrantId }: Entrant, token: string | undefined) =>
    async (dispatch: Dispatch<EventActionTypes>) => {
        await deleteEntrant(eventId, entrantId, token);
        dispatch({
            type: DELETE_ENTRANT,
            payload: { entrantId },
        });
    };

export const DeleteMarshal =
    ({ eventId, marshalId }: Marshal, token: string | undefined) =>
    async (dispatch: Dispatch<EventActionTypes>) => {
        await deleteMarshal(eventId, marshalId, token);
        dispatch({
            type: DELETE_MARSHAL,
            payload: { marshalId },
        });
    };

const UpdateTestRunState: ActionCreator<EventActionTypes> = (
    testRunId: number,
    state: TestRunUploadState
) => ({
    type: UPDATE_TEST_RUN_STATE,
    payload: { testRunId, state },
});

export const SyncTestRuns =
    (eventId: number, ordinal: number, token: string | undefined) =>
    async (dispatch: Dispatch<EventActionTypes>, getState: () => AppState) => {
        const runs = selectTestRuns(getState());
        const toUpload = runs.filter(
            (a) => a.state !== TestRunUploadState.Uploaded
        );
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

        await GetTestRuns(eventId, ordinal, token)(dispatch, getState);
    };
