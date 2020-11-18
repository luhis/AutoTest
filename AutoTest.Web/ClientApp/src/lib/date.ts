import { newValidDate, ValidDate } from "ts-date";

export const getNow = () => newValidDate();

export const getDateTimeString = (d: ValidDate) =>
    d.toISOString().substring(0, 16);

export const getDateString = (d: ValidDate) => d.toISOString().substring(0, 10);
