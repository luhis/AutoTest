import {
  Entrant,
  Event,
  TestRunFromServer,
  TestRunUploadState,
  EventNotification,
  Marshal,
  TestRunFromClient,
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
  readonly testRuns: readonly TestRunFromClient[];
  readonly testRunsFromServer: LoadingState<
    readonly TestRunFromServer[],
    { readonly eventId: number; readonly ordinal: number }
  >;
  readonly notifications: LoadingState<readonly EventNotification[], number>;
}

interface GetEntrants {
  readonly type: "GET_ENTRANTS";
  readonly payload: LoadingState<readonly PublicEntrant[], number>;
}

interface AddEntrant {
  readonly type: "ADD_ENTRANT";
  readonly payload: Entrant;
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

interface GetTestRuns {
  readonly type: "GET_TEST_RUNS";
  readonly payload: LoadingState<
    readonly TestRunFromServer[],
    { readonly eventId: number; readonly ordinal: number }
  >;
}

interface AddTestRun {
  readonly type: "ADD_TEST_RUN";
  readonly payload: TestRunFromClient;
}

interface UpdateTestRun {
  readonly type: "UPDATE_TEST_RUN";
  readonly payload: TestRunFromServer;
}

interface UpdateTestRunState {
  readonly type: "UPDATE_TEST_RUN_STATE";
  readonly payload: {
    readonly testRunId: number;
    readonly state: TestRunUploadState;
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
  | GetTestRuns
  | GetEvents
  | DeleteEvent
  | AddTestRun
  | UpdateTestRun
  | UpdateTestRunState
  | GetNotifications
  | AddNotification
  | GetMarshals
  | AddMarshal
  | DeleteMarshal
  | SharedActionTypes;
