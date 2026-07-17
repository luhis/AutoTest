import { EventState, EventActionTypes } from "./types";
import { Payment, PublicEntrant } from "../../types/models";
import { ifLoaded, LoadingState } from "../../types/loadingState";
import { neverReached } from "../../types/shared";

const initialState: EventState = {
  entrants: { tag: "Idle" },
  marshals: { tag: "Idle" },
  events: { tag: "Idle" },
  notifications: { tag: "Idle" },
};

const setPaid = (
  entrants: LoadingState<readonly PublicEntrant[], number>,
  entrantId: number,
  payment: Payment | null,
): LoadingState<readonly PublicEntrant[], number> => {
  return ifLoaded(entrants, (v) =>
    v.map((e) => (e.entrantId === entrantId ? { ...e, payment } : e)),
  );
};

export const eventReducer = (
  state = initialState,
  action: EventActionTypes,
): EventState => {
  switch (action.type) {
    case "CLEAR_CACHE":
      return {
        ...state,
        entrants: { tag: "Idle" },
        marshals: { tag: "Idle" },
        events: { tag: "Idle" },
        notifications: { tag: "Idle" },
      };
    case "GET_ENTRANTS":
      return {
        ...state,
        entrants: action.payload,
      };
    case "ADD_ENTRANT":
      return {
        ...state,
        entrants: ifLoaded(state.entrants, (c) =>
          c
            .filter(({ entrantId }) => entrantId !== action.payload.entrantId)
            .concat(action.payload),
        ),
      };
    case "SET_PAID":
      return {
        ...state,
        entrants: setPaid(
          state.entrants,
          action.payload.entrantId,
          action.payload.payment,
        ),
      };
    case "DELETE_ENTRANT":
      return {
        ...state,
        entrants: ifLoaded(state.entrants, (v) =>
          v.filter(({ entrantId }) => entrantId !== action.payload.entrantId),
        ),
      };
    case "GET_MARSHALS":
      return {
        ...state,
        marshals: action.payload,
      };
    case "ADD_MARSHAL":
      return {
        ...state,
        marshals: ifLoaded(state.marshals, (c) =>
          c
            .filter(({ marshalId }) => marshalId !== action.payload.marshalId)
            .concat(action.payload),
        ),
      };
    case "DELETE_MARSHAL":
      return {
        ...state,
        marshals: ifLoaded(state.marshals, (v) =>
          v.filter(({ marshalId }) => marshalId !== action.payload.marshalId),
        ),
      };
    case "SET_EVENT_STATUS":
      return {
        ...state,
        events: ifLoaded(state.events, (e) =>
          e.map((x) =>
            x.eventId === action.payload.eventId
              ? { ...x, eventStatus: action.payload.eventStatus }
              : x,
          ),
        ),
      };
    case "ADD_EVENT":
      return {
        ...state,
        events: ifLoaded(state.events, (e) =>
          e
            .filter(({ eventId }) => eventId !== action.payload.event.eventId)
            .concat(action.payload.event),
        ),
      };
    case "DELETE_EVENT":
      return {
        ...state,
        events: ifLoaded(state.events, (v) =>
          v.filter(({ eventId }) => eventId !== action.payload.eventId),
        ),
      };
    case "GET_EVENTS":
      return {
        ...state,
        events: action.payload,
      };
    case "GET_NOTIFICATIONS":
      return {
        ...state,
        notifications: action.payload,
      };
    case "ADD_NOTIFICATION":
      return {
        ...state,
        notifications: ifLoaded(state.notifications, (a) =>
          [action.payload].concat(a),
        ),
      };
    default: {
      neverReached(action);
      return state;
    }
  }
};
