import { newValidDate } from "ts-date";

import {
  EntrantStatus,
  EventType,
  Payment,
  PaymentMethod,
  PublicEntrant,
} from "../../types/models";
import { ClearCache } from "../../store/event/actions";
import { eventReducer } from "../../store/event/reducers";
import { EventState } from "../../store/event/types";
import { Age } from "../../types/profileModels";
import { InductionTypes } from "../../types/shared";
import { mapOrDefault } from "../../types/loadingState";

const populatedState: EventState = {
  entrants: { tag: "Error", value: "Fail" },
  marshals: { tag: "Error", value: "Fail" },
  events: { tag: "Error", value: "Fail" },
  notifications: { tag: "Error", value: "Fail" },
};

const entrant = (payment: Payment): PublicEntrant => ({
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
  entrantStatus: EntrantStatus.Live,
});

describe("Event Reducer", () => {
  test("Clear Cache", () => {
    const finalState = eventReducer(populatedState, ClearCache());
    expect(finalState.entrants.tag).toBe("Idle");
    expect(finalState.marshals.tag).toBe("Idle");
    expect(finalState.events.tag).toBe("Idle");
    expect(finalState.notifications.tag).toBe("Idle");
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
        value: [entrant(payment)],
        id: 1,
        loaded: newValidDate(),
      },
    };
    const finalState = eventReducer(stateWithEntrant, {
      type: "SET_PAID",
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
      createdBy: "test@test.com",
    };
    const stateWithEntrant: EventState = {
      ...populatedState,
      entrants: {
        tag: "Loaded",
        value: [entrant(payment)],
        id: 1,
        loaded: newValidDate(),
      },
    };
    const finalState = eventReducer(stateWithEntrant, {
      type: "SET_PAID",
      payload: { entrantId: 2, payment: payment },
    });
    expect(finalState.entrants.tag).toBe("Loaded");
    expect(mapOrDefault(finalState.entrants, (a) => a[0].payment, null)).toBe(
      payment,
    );
  });
});
