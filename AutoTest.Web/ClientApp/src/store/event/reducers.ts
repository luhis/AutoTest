import { EventState, EventActionTypes, GET_ENTRANTS } from "./types";

const initialState: EventState = {
    entrants: { tag: "Idle" },
};

export function eventReducer(
    state = initialState,
    action: EventActionTypes
): EventState {
    switch (action.type) {
        case GET_ENTRANTS:
            return {
                ...state,
                entrants: action.payload,
            };
        default:
            return state;
    }
}
