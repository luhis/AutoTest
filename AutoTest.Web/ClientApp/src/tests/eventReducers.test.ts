import { newValidDate } from "ts-date";

import { EventType, PaymentMethod, TestRunUploadState } from "../types/models";
import { ClearCache } from "../store/event/actions";
import { eventReducer } from "../store/event/reducers";
import {
  EventState,
  EventActionTypes,
  SET_PAID,
  UPDATE_TEST_RUN_STATE,
} from "../store/event/types";
import { Age } from "../types/profileModels";
import { InductionTypes } from "../types/shared";
import { mapOrDefault } from "../types/loadingState";

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
      ClearCache() as EventActionTypes,
    );
    expect(finalState.entrants.tag).toBe("Idle");
    expect(finalState.marshals.tag).toBe("Idle");
    expect(finalState.testRunsFromServer.tag).toBe("Idle");
    expect(finalState.events.tag).toBe("Idle");
    expect(finalState.notifications.tag).toBe("Idle");
    expect(finalState.testRuns.length).toBe(1);
  });

  test("Set Paid unpaid", () => {
    const payment = {
      method: PaymentMethod.PayPal,
      paidAt: newValidDate(),
      timestamp: newValidDate(),
    };
    const stateWithEntrant: EventState = {
      ...populatedState,
      entrants: {
        tag: "Loaded",
        value: [
          {
            entrantId: 2,
            payment,
            age: Age.Senior,
            eventType: EventType.AutoTest,
            class: "A",
            club: "BRMC",
            driverNumber: 1,
            eventId: 2,
            familyName: "Family Name",
            givenName: "Given Name",
            vehicle: {
              displacement: 1364,
              induction: InductionTypes.NA,
              make: "",
              model: "",
              registration: "",
              year: 2006,
            },
          },
        ],
        id: 1,
        loaded: newValidDate(),
      },
    };
    const finalState = eventReducer(stateWithEntrant, {
      type: SET_PAID,
      payload: { entrantId: 2, payment: null },
    });
    expect(finalState.entrants.tag).toBe("Loaded");
    expect(
      mapOrDefault(finalState.entrants, (a) => a[0].payment, payment),
    ).toBeNull();
  });
  test("Set Paid Bacs", () => {
    const payment = {
      method: PaymentMethod.Bacs,
      paidAt: newValidDate(),
      timestamp: newValidDate(),
    };
    const stateWithEntrant: EventState = {
      ...populatedState,
      entrants: {
        tag: "Loaded",
        value: [
          {
            entrantId: 2,
            payment: null,
            age: Age.Senior,
            eventType: EventType.AutoTest,
            class: "A",
            club: "BRMC",
            driverNumber: 1,
            eventId: 2,
            familyName: "Family Name",
            givenName: "Given Name",
            vehicle: {
              displacement: 1364,
              induction: InductionTypes.NA,
              make: "",
              model: "",
              registration: "",
              year: 2006,
            },
          },
        ],
        id: 1,
        loaded: newValidDate(),
      },
    };
    const finalState = eventReducer(stateWithEntrant, {
      type: SET_PAID,
      payload: { entrantId: 2, payment: payment },
    });
    expect(finalState.entrants.tag).toBe("Loaded");
    expect(mapOrDefault(finalState.entrants, (a) => a[0].payment, null)).toBe(
      payment,
    );
  });
  test("Update test run state", () => {
    const stateWithEntrant: EventState = {
      ...populatedState,
      testRuns: [
        {
          state: TestRunUploadState.NotSent,
          timeInMS: 10000,
          created: newValidDate(),
          entrantId: 1,
          eventId: 1,
          testRunId: 2,
          ordinal: 1,
          penalties: [],
        },
      ],
    };
    const finalState = eventReducer(stateWithEntrant, {
      type: UPDATE_TEST_RUN_STATE,
      payload: { testRunId: 2, state: TestRunUploadState.Uploaded },
    });
    expect(finalState.testRuns[0].state).toBe(TestRunUploadState.Uploaded);
  });
});
