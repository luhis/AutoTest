import { ClearCache } from "../store/event/actions";
import { eventReducer } from "../store/event/reducers";
import { EventState, EventActionTypes } from "../store/event/types";

const populatedState: EventState = {
    entrants: { tag: "Error", value: "Fail" },
    marshals: { tag: "Idle" },
    testRuns: [],
    testRunsFromServer: { tag: "Idle" },
    events: { tag: "Idle" },
    notifications: { tag: "Idle" },
};

describe("Event Reducer", () => {
    test("Clear Cache", () => {
        const finalState = eventReducer(
            populatedState,
            ClearCache() as EventActionTypes
        );
        expect(finalState.entrants.tag).toBe("Idle");
    });
});
