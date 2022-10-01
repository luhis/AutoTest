import { ValidDate } from "ts-date";

export const getDateTimeString = (d: ValidDate) =>
    `${d.getFullYear()}-${`${d.getMonth() + 1}`.padStart(2, "0")}-${`${
        d.getDate() + 1
    }`.padStart(2, "0")}T${`${d.getHours()}`.padStart(
        2,
        "0"
    )}:${`${d.getMinutes()}`.padStart(2, "0")}`;

export const getDateString = (d: ValidDate | Date) =>
    d.toISOString().substring(0, 10);
