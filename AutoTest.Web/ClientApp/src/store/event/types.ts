import {
  Event,
  EventNotification,
  Marshal,
  PublicEntrant,
  PublicMarshal,
  Payment,
  EventStatus,
} from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import { AddEvent } from "./actions";
import { SharedActionTypes } from "../shared/types";

export interface EventState {
  readonly events: LoadingState<readonly Event[]>;
  readonly entrants: LoadingState<readonly PublicEntrant[], number>;
  readonly marshals: LoadingState<readonly PublicMarshal[], number>;
  readonly notifications: LoadingState<readonly EventNotification[], number>;
}

interface GetEntrants {
  readonly type: "GET_ENTRANTS";
  readonly payload: LoadingState<readonly PublicEntrant[], number>;
}

interface AddEntrant {
  readonly type: "ADD_ENTRANT";
  readonly payload: PublicEntrant;
}

interface DeleteEntrant {
  readonly type: "DELETE_ENTRANT";
  readonly payload: { readonly entrantId: number };
}

interface SetPaid {
  readonly type: "SET_PAID";
  readonly payload: {
    readonly entrantId: number;
    readonly payment: Payment | null;
  };
}

interface GetMarshals {
  readonly type: "GET_MARSHALS";
  readonly payload: LoadingState<readonly PublicMarshal[], number>;
}

interface AddMarshal {
  readonly type: "ADD_MARSHAL";
  readonly payload: Marshal;
}

interface DeleteMarshal {
  readonly type: "DELETE_MARSHAL";
  readonly payload: { readonly marshalId: number };
}

interface GetEvents {
  readonly type: "GET_EVENTS";
  readonly payload: LoadingState<readonly Event[]>;
}

interface DeleteEvent {
  readonly type: "DELETE_EVENT";
  readonly payload: { readonly eventId: number };
}

interface AddEvent {
  readonly type: "ADD_EVENT";
  readonly payload: { readonly event: Event };
}

interface SetEventStatus {
  readonly type: "SET_EVENT_STATUS";
  readonly payload: {
    readonly eventId: number;
    readonly eventStatus: EventStatus;
  };
}

interface GetNotifications {
  readonly type: "GET_NOTIFICATIONS";
  readonly payload: LoadingState<readonly EventNotification[], number>;
}

interface AddNotification {
  readonly type: "ADD_NOTIFICATION";
  readonly payload: EventNotification;
}

export type EventActionTypes =
  | GetEntrants
  | AddEntrant
  | SetPaid
  | DeleteEntrant
  | AddEvent
  | SetEventStatus
  | GetEvents
  | DeleteEvent
  | GetNotifications
  | AddNotification
  | GetMarshals
  | AddMarshal
  | DeleteMarshal
  | SharedActionTypes;
