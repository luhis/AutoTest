import { parseIsoOrThrow } from "ts-date";

import { Event, Override } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { throwIfNotOk } from "./api";

export const getEvents = async (): Promise<ApiResponse<readonly Event[]>> =>
    toApiResponse(async () => {
        const response = await fetch("/api/events");
        throwIfNotOk(response);
        type ApiEvent = Override<Event, { readonly startTime: string }>;
        const events = (await response.json()) as readonly ApiEvent[];

        return events.map(({ startTime, ...rest }) => ({
            ...rest,
            startTime: parseIsoOrThrow(startTime),
        }));
    });

export const addEvent = async (
    event: Event,
    token: string | undefined
): Promise<void> => {
    const { eventId, ...rest } = event;
    debugger;
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
