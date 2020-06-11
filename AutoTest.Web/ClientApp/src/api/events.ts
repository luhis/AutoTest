import { parseIsoOrThrow } from "ts-date";

import { Event, ApiResponse, toApiResponse, Override } from "../types/models";
import { throwIfNotOk } from "./api";

type ApiEvent = Override<Event, { startTime: string }>;

export const getEvents = async (): Promise<ApiResponse<readonly Event[]>> =>
    toApiResponse(async () => {
        const response = await fetch("events");
        throwIfNotOk(response);
        const events = (await response.json()) as ApiEvent[];

        return events.map((a) => ({
            ...a,
            startTime: parseIsoOrThrow(a.startTime),
        }));
    });
