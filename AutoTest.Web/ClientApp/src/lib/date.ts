import { newValidDateOrThrow } from "ts-date";

export const getNow = () => newValidDateOrThrow(Date.now());
