import { EventState, EventActionTypes, GET_ENTRANTS } from "./types";

const initialState: EventState = {
    entrants: { tag: "Loading" },
};

export function eventReducer(
    state = initialState,
    action: EventActionTypes
): EventState {
    switch (action.type) {
        case GET_ENTRANTS:
            return {
                ...state,
                entrants: { tag: "Loaded", value: [] },
            };
        default:
            return state;
    }
}
