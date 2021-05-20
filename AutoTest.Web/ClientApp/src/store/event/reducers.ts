import {
    EventState,
    EventActionTypes,
    GET_ENTRANTS,
    ADD_ENTRANT,
    ADD_TEST_RUN,
    UPDATE_TEST_RUN_STATE,
    GET_EVENTS,
    SET_PAID,
    DELETE_ENTRANT,
    DELETE_EVENT,
    ADD_EVENT,
    GET_TEST_RUNS,
    GET_NOTIFICATIONS,
    ADD_NOTIFICATION,
    GET_MARSHALS,
    ADD_MARSHAL,
    DELETE_MARSHAL,
} from "./types";
import { Entrant, TestRunUploadState } from "../../types/models";
import { ifLoaded, LoadingState } from "../../types/loadingState";
import { neverReached } from "../../types/shared";
import { CLEAR_CACHE } from "../shared/types";

const initialState: EventState = {
    entrants: { tag: "Idle" },
    marshals: { tag: "Idle" },
    testRuns: [],
    testRunsFromServer: { tag: "Idle" },
    events: { tag: "Idle" },
    notifications: { tag: "Idle" },
};

const setPaid = (
    entrants: LoadingState<readonly Entrant[], number>,
    entrantId: number,
    isPaid: boolean
): LoadingState<readonly Entrant[], number> => {
    return ifLoaded(entrants, (v) =>
        v.map((e) => (e.entrantId === entrantId ? { ...e, isPaid } : e))
    );
};

// const createNewTestRuns = (
//     payload: LoadingState<readonly TestRun[], number>
// ) => {
//     if (payload.tag === "Loaded") {
//         return payload.value.map((a) => ({
//             ...a,
//             state: TestRunUploadState.Uploaded,
//             eventId: payload.id,
//         }));
//     } else {
//         return [];
//     }
// };

export const eventReducer = (
    state = initialState,
    action: EventActionTypes
): EventState => {
    switch (action.type) {
        case CLEAR_CACHE:
            return {
                ...state,
                entrants: { tag: "Idle" },
                marshals: { tag: "Idle" },
                testRunsFromServer: { tag: "Idle" },
                events: { tag: "Idle" },
            };
        case GET_ENTRANTS:
            return {
                ...state,
                entrants: action.payload,
            };
        case ADD_ENTRANT:
            return {
                ...state,
                entrants: ifLoaded(state.entrants, (c) =>
                    c
                        .filter(
                            ({ entrantId }) =>
                                entrantId !== action.payload.entrantId
                        )
                        .concat(action.payload)
                ),
            };
        case SET_PAID:
            return {
                ...state,
                entrants: setPaid(
                    state.entrants,
                    action.payload.entrantId,
                    action.payload.isPaid
                ),
            };
        case DELETE_ENTRANT:
            return {
                ...state,
                entrants: ifLoaded(state.entrants, (v) =>
                    v.filter(
                        ({ entrantId }) =>
                            entrantId !== action.payload.entrantId
                    )
                ),
            };
        case GET_MARSHALS:
            return {
                ...state,
                marshals: action.payload,
            };
        case ADD_MARSHAL:
            return {
                ...state,
                marshals: ifLoaded(state.marshals, (c) =>
                    c
                        .filter(
                            ({ marshalId }) =>
                                marshalId !== action.payload.marshalId
                        )
                        .concat(action.payload)
                ),
            };
        case DELETE_MARSHAL:
            return {
                ...state,
                marshals: ifLoaded(state.marshals, (v) =>
                    v.filter(
                        ({ marshalId }) =>
                            marshalId !== action.payload.marshalId
                    )
                ),
            };
        case ADD_EVENT:
            return {
                ...state,
                events: ifLoaded(state.events, (e) =>
                    e
                        .filter(
                            ({ eventId }) =>
                                eventId != action.payload.event.eventId
                        )
                        .concat(action.payload.event)
                ),
            };
        case DELETE_EVENT:
            return {
                ...state,
                events: ifLoaded(state.events, (v) =>
                    v.filter(
                        ({ eventId }) => eventId !== action.payload.eventId
                    )
                ),
            };
        case GET_EVENTS:
            return {
                ...state,
                events: action.payload,
            };
        case ADD_TEST_RUN:
            return {
                ...state,
                testRuns: state.testRuns.concat({
                    ...action.payload,
                    state: TestRunUploadState.NotSent,
                }),
            };
        case UPDATE_TEST_RUN_STATE: {
            const found = state.testRuns.find(
                (a) => a.testRunId === action.payload.testRunId
            );
            return {
                ...state,
                testRuns:
                    found === undefined
                        ? state.testRuns
                        : state.testRuns
                              .filter(
                                  (a) =>
                                      a.testRunId !== action.payload.testRunId
                              )
                              .concat({
                                  ...found,
                                  state: action.payload.state,
                              }),
            };
        }
        case GET_TEST_RUNS:
            return {
                ...state,
                testRunsFromServer: action.payload,
            };
        case GET_NOTIFICATIONS:
            return {
                ...state,
                notifications: action.payload,
            };
        case ADD_NOTIFICATION:
            return {
                ...state,
                notifications: ifLoaded(state.notifications, (a) =>
                    [action.payload].concat(a)
                ),
            };
        default: {
            neverReached(action);
            return state;
        }
    }
};
