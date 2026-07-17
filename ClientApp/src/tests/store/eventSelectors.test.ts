import { newValidDate } from "ts-date";

import { get10LatestEvents } from "../../store/event/selectors";
import { Event, EventStatus, TimingSystem } from "../../types/models";

const event: Event = {
  eventId: 1,
  clubId: 1,
  courseCount: 1,
  courses: [],
  entryCloseDate: newValidDate(),
  entryOpenDate: newValidDate(),
  eventStatus: EventStatus.Open,
  timingSystem: TimingSystem.App,
  location: "",
  eventTypes: [],
  regulations: "",
  maps: null,
  maxAttemptsPerCourse: 2,
  maxEntrants: 40,
  startTime: newValidDate(),
  created: newValidDate("2000-01-01")!,
};
const secondEvent = { ...event, created: newValidDate("2000-01-02")! };

describe("Event Selectors", () => {
  test("get10LatestEvents", () => {
    const sorted = get10LatestEvents([event, secondEvent]);
    expect(sorted).toStrictEqual([secondEvent, event]);
  });
});
