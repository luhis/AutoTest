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

export const addEvent = async (
    event: Event,
    token: string | undefined
): Promise<void> => {
    const { eventId, ...rest } = event;
    const response = await fetch(`/api/events/${eventId}`, {
        headers: {
            "Content-Type": "application/json",
            Authorization: token ? `Bearer ${token}` : "",
        },
        method: "PUT",
        body: JSON.stringify(rest),
    });
    throwIfNotOk(response);
};
