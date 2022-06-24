import { newValidDate } from "ts-date";

import { TestRunUploadState } from "../types/models";
import { ClearCache } from "../store/event/actions";
import { eventReducer } from "../store/event/reducers";
import { EventState, EventActionTypes } from "../store/event/types";

const populatedState: EventState = {
    entrants: { tag: "Error", value: "Fail" },
    marshals: { tag: "Error", value: "Fail" },
    testRuns: [
        {
            created: newValidDate(),
            entrantId: 1,
            eventId: 1,
            ordinal: 1,
            penalties: [],
            state: TestRunUploadState.NotSent,
            testRunId: 1,
            timeInMS: 1000,
        },
    ],
    testRunsFromServer: { tag: "Error", value: "Fail" },
    events: { tag: "Error", value: "Fail" },
    notifications: { tag: "Error", value: "Fail" },
};

describe("Event Reducer", () => {
    test("Clear Cache", () => {
        const finalState = eventReducer(
            populatedState,
            ClearCache() as EventActionTypes
        );
        expect(finalState.entrants.tag).toBe("Idle");
        expect(finalState.marshals.tag).toBe("Idle");
        expect(finalState.testRunsFromServer.tag).toBe("Idle");
        expect(finalState.events.tag).toBe("Idle");
        expect(finalState.notifications.tag).toBe("Idle");
        expect(finalState.testRuns.length).toBe(1);
    });
});
