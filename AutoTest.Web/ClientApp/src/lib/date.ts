import { ValidDate } from "ts-date";

export const getDateTimeString = (d: ValidDate) =>
    d.toISOString().substring(0, 16);

export const getDateString = (d: ValidDate) => d.toISOString().substring(0, 10);
