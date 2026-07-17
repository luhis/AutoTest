import { newValidDateOrThrow } from "ts-date";

import { getDateTimeString, getDateString } from "../lib/date";

describe("getDateTimeString", () => {
  it("formats date correctly without off-by-one error", () => {
    const date = newValidDateOrThrow(2024, 2, 15, 10, 30); // March 15, 2024 10:30
    expect(getDateTimeString(date)).toBe("2024-03-15T10:30");
  });

  it("pads single digit month and day", () => {
    const date = newValidDateOrThrow(2024, 0, 5, 8, 5); // Jan 5, 2024 08:05
    expect(getDateTimeString(date)).toBe("2024-01-05T08:05");
  });

  it("handles end of month correctly", () => {
    const date = newValidDateOrThrow(2024, 1, 29, 23, 59); // Feb 29, 2024 23:59 (leap year)
    expect(getDateTimeString(date)).toBe("2024-02-29T23:59");
  });

  it("handles month boundaries correctly", () => {
    const date = newValidDateOrThrow(2024, 11, 31, 0, 0); // Dec 31, 2024 00:00
    expect(getDateTimeString(date)).toBe("2024-12-31T00:00");
  });

  it("handles day 1 correctly", () => {
    const date = newValidDateOrThrow(2024, 0, 1, 12, 0); // Jan 1, 2024 12:00
    expect(getDateTimeString(date)).toBe("2024-01-01T12:00");
  });
});

describe("getDateString", () => {
  it("returns ISO date string", () => {
    const date = newValidDateOrThrow("2024-03-15T10:30:00Z");
    expect(getDateString(date)).toBe("2024-03-15");
  });

  it("pads single digit month and day", () => {
    const date = newValidDateOrThrow("2024-01-05T08:05:00Z");
    expect(getDateString(date)).toBe("2024-01-05");
  });
});
