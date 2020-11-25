import {
    EventState,
    EventActionTypes,
    GET_ENTRANTS,
    ADD_TEST_RUN,
    UPDATE_TEST_RUN_STATE,
    GET_EVENTS,
    GET_CLUBS,
    SET_PAID,
    DELETE_ENTRANT,
} from "./types";
import { Entrant, TestRunUploadState } from "../../types/models";
import { LoadingState } from "src/types/loadingState";

const initialState: EventState = {
    entrants: { tag: "Idle" },
    testRuns: [],
    testRunsFromServer: { tag: "Idle" },
    events: { tag: "Idle" },
    clubs: { tag: "Idle" },
};

const ifLoaded = (
    entrants: LoadingState<readonly Entrant[]>,
    f: (_: readonly Entrant[]) => readonly Entrant[]
) => {
    if (entrants.tag === "Loaded") {
        return {
            tag: "Loaded",
            value: f(entrants.value),
        } as LoadingState<readonly Entrant[]>;
    } else {
        return entrants;
    }
};

const setPaid = (
    entrants: LoadingState<readonly Entrant[]>,
    entrantId: number,
    isPaid: boolean
): LoadingState<readonly Entrant[]> => {
    return ifLoaded(entrants, (v) =>
        v.map((e) => (e.entrantId === entrantId ? { ...e, isPaid: isPaid } : e))
    );
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
    }
    return state;
};
