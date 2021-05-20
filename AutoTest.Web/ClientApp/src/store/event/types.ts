import {
    Entrant,
    Event,
    TestRun,
    TestRunUploadState,
    TestRunTemp,
    Notification,
    Marshal,
} from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import { AddEvent } from "./actions";
import { SharedActionTypes } from "../shared/types";

export interface EventState {
    readonly events: LoadingState<readonly Event[]>;
    readonly entrants: LoadingState<readonly Entrant[], number>;
    readonly marshals: LoadingState<readonly Marshal[], number>;
    readonly testRuns: readonly (TestRun & {
        readonly state: TestRunUploadState;
        readonly eventId: number;
    })[];
    readonly testRunsFromServer: LoadingState<readonly TestRun[], number>;
    readonly notifications: LoadingState<readonly Notification[], number>;
}

export const GET_ENTRANTS = "GET_ENTRANTS";
export const ADD_ENTRANT = "ADD_ENTRANT";
export const ADD_MARSHAL = "ADD_MARSHAL";
export const DELETE_MARSHAL = "DELETE_MARSHAL";
export const GET_MARSHALS = "GET_MARSHALS";
export const SET_PAID = "SET_PAID";
export const DELETE_ENTRANT = "DELETE_ENTRANT";
export const GET_EVENTS = "GET_EVENTS";
export const DELETE_EVENT = "DELETE_EVENT";
export const ADD_EVENT = "ADD_EVENT";
export const GET_TEST_RUNS = "GET_TEST_RUNS";
export const ADD_TEST_RUN = "ADD_TEST_RUN";
export const UPDATE_TEST_RUN_STATE = "UPDATE_TEST_RUN_STATE";
export const GET_NOTIFICATIONS = "GET_NOTIFICATIONS";
export const ADD_NOTIFICATION = "ADD_NOTIFICATION";

interface GetEntrants {
    readonly type: typeof GET_ENTRANTS;
    readonly payload: LoadingState<readonly Entrant[], number>;
}

interface AddEntrant {
    readonly type: typeof ADD_ENTRANT;
    readonly payload: Entrant;
}

interface DeleteEntrant {
    readonly type: typeof DELETE_ENTRANT;
    readonly payload: { readonly entrantId: number };
}

interface SetPaid {
    readonly type: typeof SET_PAID;
    readonly payload: { readonly entrantId: number; readonly isPaid: boolean };
}

interface GetMarshals {
    readonly type: typeof GET_MARSHALS;
    readonly payload: LoadingState<readonly Marshal[], number>;
}

interface AddMarshal {
    readonly type: typeof ADD_MARSHAL;
    readonly payload: Marshal;
}

interface DeleteMarshal {
    readonly type: typeof DELETE_MARSHAL;
    readonly payload: { readonly marshalId: number };
}

interface GetEvents {
    readonly type: typeof GET_EVENTS;
    readonly payload: LoadingState<readonly Event[]>;
}

interface DeleteEvent {
    readonly type: typeof DELETE_EVENT;
    readonly payload: { readonly eventId: number };
}

interface AddEvent {
    readonly type: typeof ADD_EVENT;
    readonly payload: { readonly event: Event };
}

interface GetTestRuns {
    readonly type: typeof GET_TEST_RUNS;
    readonly payload: LoadingState<readonly TestRun[], number>;
}

interface AddTestRun {
    readonly type: typeof ADD_TEST_RUN;
    readonly payload: TestRunTemp;
}

interface UpdateTestRunState {
    readonly type: typeof UPDATE_TEST_RUN_STATE;
    readonly payload: {
        readonly testRunId: number;
        readonly state: TestRunUploadState;
    };
}

interface GetNotifications {
    readonly type: typeof GET_NOTIFICATIONS;
    readonly payload: LoadingState<readonly Notification[], number>;
}

interface AddNotification {
    readonly type: typeof ADD_NOTIFICATION;
    readonly payload: Notification;
}

export type EventActionTypes =
    | GetEntrants
    | AddEntrant
    | SetPaid
    | DeleteEntrant
    | AddEvent
    | GetTestRuns
    | GetEvents
    | DeleteEvent
    | AddTestRun
    | UpdateTestRunState
    | GetNotifications
    | AddNotification
    | GetMarshals
    | AddMarshal
    | DeleteMarshal
    | SharedActionTypes;
