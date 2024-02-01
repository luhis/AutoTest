import { ActionCreator } from "redux";
import { ThunkAction } from "redux-thunk";

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
  UPDATE_TEST_RUN,
  SET_EVENT_STATUS,
} from "./types";
import {
  TestRunUploadState,
  Entrant,
  Event,
  EventNotification,
  Marshal,
  TestRunFromServer,
  TestRunFromClient,
  PublicMarshal,
  PublicEntrant,
  Payment,
  EventStatus,
} from "../../types/models";
import {
  addEntrant,
  deleteEntrant,
  getEntrants,
  markPaid,
} from "../../api/entrants";
import { addMarshal, deleteMarshal, getMarshals } from "../../api/marshals";
import { AppState } from "..";
import { addTestRun, getTestRuns, updateTestRun } from "../../api/testRuns";
import {
  requiresLoading,
  idsMatch,
  isStale,
  ifLoaded,
  canUpdate,
} from "../../types/loadingState";
import {
  addEvent,
  deleteEvent,
  getEvents,
  setEventStatus,
} from "../../api/events";
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
  (
    eventId: number,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch, getState) => {
    const clubs = selectMarshals(getState());
    if (requiresLoading(clubs.tag) || isStale(clubs)) {
      if (clubs.tag === "Idle") {
        dispatch({
          type: GET_MARSHALS,
          payload: { tag: "Loading", id: eventId },
        });
      }
      const res = await getMarshals(eventId);
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
  (
    eventId: number,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch, getState) => {
    const entrants = selectEntrants(getState());
    if (!idsMatch(entrants, eventId)) {
      dispatch({
        type: GET_ENTRANTS,
        payload: { tag: "Loading", id: eventId },
      });
    }
    if (requiresLoading(entrants.tag) || isStale(entrants)) {
      const res = await getEntrants(eventId);
      if (canUpdate(entrants, res)) {
        dispatch({
          type: GET_ENTRANTS,
          payload: res,
        });
      }
    }
  };

export const AddEntrant =
  (
    entrant: Entrant,
    token: string | undefined,
    onSuccess: () => void,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch) => {
    const newEntrant = await addEntrant(entrant, token);
    dispatch({
      type: ADD_ENTRANT,
      payload: newEntrant,
    });
    onSuccess();
  };

export const AddMarshal =
  (
    marshal: Marshal,
    token: string | undefined,
    onSuccess: () => void,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch) => {
    const newEntrant = await addMarshal(marshal, token);
    dispatch({
      type: ADD_MARSHAL,
      payload: newEntrant,
    });
    onSuccess();
  };

export const GetEventsIfRequired =
  (): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch, getState) => {
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
  (
    eventId: number,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch) => {
    dispatch({
      type: GET_NOTIFICATIONS,
      payload: { tag: "Loading", id: eventId },
    });
    dispatch({
      type: GET_NOTIFICATIONS,
      payload: await getNotifications(eventId),
    });
  };

export const AddNotification = (notification: EventNotification) => ({
  type: ADD_NOTIFICATION,
  payload: notification,
});

export const CreateNotification =
  (
    notification: EventNotification,
    token: string | undefined,
  ): ThunkAction<Promise<boolean>, AppState, unknown, EventActionTypes> =>
  async (dispatch) => {
    try {
      await addNotification(notification, token);
      dispatch({
        type: ADD_NOTIFICATION,
        payload: notification,
      });
      return true;
    } catch (e) {
      return false;
    }
  };

export const SetEventStatus =
  (
    eventId: number,
    eventStatus: EventStatus,
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch) => {
    await setEventStatus(eventId, eventStatus, token);
    dispatch({
      type: SET_EVENT_STATUS,
      payload: { eventId, eventStatus },
    });
  };

export const AddEvent =
  (
    event: Event,
    token: string | undefined,
    onSuccess: () => void,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch) => {
    await addEvent(event, token);
    dispatch({
      type: ADD_EVENT,
      payload: { event },
    });
    onSuccess();
  };

export const DeleteEvent =
  (
    eventId: number,
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch) => {
    await deleteEvent(eventId, token);
    dispatch({
      type: DELETE_EVENT,
      payload: { eventId },
    });
  };

export const GetTestRunsIfRequired =
  (
    eventId: number,
    ordinal: number,
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch, getState) => {
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
      await GetTestRuns(eventId, ordinal, token)(dispatch, getState, {});
    }
  };

const GetTestRuns =
  (
    eventId: number,
    ordinal: number,
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch, getState) => {
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
  (
    testRun: TestRunFromClient,
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch, getState) => {
    dispatch({
      type: ADD_TEST_RUN,
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
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch) => {
    await updateTestRun(testRun, token);
    onSuccess();
    dispatch({
      type: UPDATE_TEST_RUN,
      payload: testRun,
    });
  };

export const SetPaid =
  (
    { eventId, entrantId }: PublicEntrant,
    payment: Payment | null,
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch) => {
    await markPaid(eventId, entrantId, payment, token);
    dispatch({
      type: SET_PAID,
      payload: { entrantId, payment },
    });
  };

export const DeleteEntrant =
  (
    { eventId, entrantId }: PublicEntrant,
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch) => {
    await deleteEntrant(eventId, entrantId, token);
    dispatch({
      type: DELETE_ENTRANT,
      payload: { entrantId },
    });
  };

export const DeleteMarshal =
  (
    { eventId, marshalId }: PublicMarshal,
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch) => {
    await deleteMarshal(eventId, marshalId, token);
    dispatch({
      type: DELETE_MARSHAL,
      payload: { marshalId },
    });
  };

const UpdateTestRunState: ActionCreator<EventActionTypes> = (
  testRunId: number,
  state: TestRunUploadState,
) => ({
  type: UPDATE_TEST_RUN_STATE,
  payload: { testRunId, state },
});

export const SyncTestRuns =
  (
    eventId: number,
    ordinal: number,
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch, getState) => {
    const runs = selectTestRuns(getState());
    const toUpload = runs.filter(
      (a) => a.state !== TestRunUploadState.Uploaded,
    );
    await Promise.all(
      toUpload.map(async (element) => {
        const res = await addTestRun(element, token);
        ifLoaded(res, () => {
          dispatch(
            UpdateTestRunState(element.testRunId, TestRunUploadState.Uploaded),
          );
        });
      }),
    );

    await GetTestRuns(eventId, ordinal, token)(dispatch, getState, {});
  };
