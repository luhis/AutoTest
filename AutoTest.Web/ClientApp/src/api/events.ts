import { parseIsoOrThrow } from "ts-date";

import { Event, ApiResponse, toApiResponse, Override } from "../types/models";
import { throwIfNotOk } from "./api";

export const getEvents = async (): Promise<ApiResponse<readonly Event[]>> =>
    toApiResponse(async () => {
        const response = await fetch("/api/events");
        throwIfNotOk(response);
        type ApiEvent = Override<Event, { startTime: string }>;
        const events = (await response.json()) as ApiEvent[];

        return events.map((a) => ({
            ...a,
            startTime: parseIsoOrThrow(a.startTime),
        }));
    });
