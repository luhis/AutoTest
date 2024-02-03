import { ThunkAction } from "redux-thunk";

import { EventActionTypes } from "./types";
import {
  Entrant,
  Event,
  EventNotification,
  Marshal,
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
import {
  requiresLoading,
  idsMatch,
  isStale,
  canUpdate,
} from "../../types/loadingState";
import {
  addEvent,
  deleteEvent,
  getEvents,
  setEventStatus,
} from "../../api/events";
import { addNotification, getNotifications } from "../../api/notifications";
import { selectEntrants, selectEvents, selectMarshals } from "./selectors";

export const GetMarshalsIfRequired =
  (
    eventId: number,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch, getState) => {
    const clubs = selectMarshals(getState());
    if (requiresLoading(clubs.tag) || isStale(clubs)) {
      if (clubs.tag === "Idle") {
        dispatch({
          type: "GET_MARSHALS",
          payload: { tag: "Loading", id: eventId },
        });
      }
      const res = await getMarshals(eventId);
      if (canUpdate(clubs, res)) {
        dispatch({
          type: "GET_MARSHALS",
          payload: res,
        });
      }
    }
  };

export const ClearCache = () => ({
  type: "CLEAR_CACHE" as const,
});

export const GetEntrantsIfRequired =
  (
    eventId: number,
  ): ThunkAction<Promise<void>, AppState, unknown, EventActionTypes> =>
  async (dispatch, getState) => {
    const entrants = selectEntrants(getState());
    if (!idsMatch(entrants, eventId)) {
      dispatch({
        type: "GET_ENTRANTS",
        payload: { tag: "Loading", id: eventId },
      });
    }
    if (requiresLoading(entrants.tag) || isStale(entrants)) {
      const res = await getEntrants(eventId);
      if (canUpdate(entrants, res)) {
        dispatch({
          type: "GET_ENTRANTS",
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
      type: "ADD_ENTRANT",
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
      type: "ADD_MARSHAL",
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
          type: "GET_EVENTS",
          payload: { tag: "Loading", id: undefined },
        });
      }
      const res = await getEvents();
      if (canUpdate(events, res)) {
        dispatch({
          type: "GET_EVENTS",
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
      type: "GET_NOTIFICATIONS",
      payload: { tag: "Loading", id: eventId },
    });
    dispatch({
      type: "GET_NOTIFICATIONS",
      payload: await getNotifications(eventId),
    });
  };

export const AddNotification = (notification: EventNotification) => ({
  type: "ADD_NOTIFICATION",
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
        type: "ADD_NOTIFICATION",
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
      type: "SET_EVENT_STATUS",
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
      type: "ADD_EVENT",
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
      type: "DELETE_EVENT",
      payload: { eventId },
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
      type: "SET_PAID",
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
      type: "DELETE_ENTRANT",
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
      type: "DELETE_MARSHAL",
      payload: { marshalId },
    });
  };
