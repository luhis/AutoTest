import {
    EventState,
    EventActionTypes,
    GET_ENTRANTS,
    ADD_TEST_RUN,
    UPDATE_TEST_RUN_STATE,
    GET_EVENTS,
    GET_CLUBS,
} from "./types";
import { TestRunUploadState } from "../../types/models";

const initialState: EventState = {
    entrants: { tag: "Idle" },
    testRuns: [],
    testRunsFromServer: { tag: "Idle" },
    events: { tag: "Idle" },
    clubs: { tag: "Idle" },
};

export const eventReducer = (
    state = initialState,
    action: EventActionTypes
): EventState => {
    switch (action.type) {
        case GET_CLUBS:
            return {
                ...state,
                clubs: action.payload,
            };
        case GET_ENTRANTS:
            return {
                ...state,
                entrants: action.payload,
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
        default:
            return state;
    }
};
