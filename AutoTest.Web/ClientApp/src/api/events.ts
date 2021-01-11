import { parseIsoOrThrow } from "ts-date";

import { Event, Override } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { extract, getHeaders, throwIfNotOk } from "./api";

export const getEvents = async (): Promise<ApiResponse<readonly Event[]>> =>
    toApiResponse(async () => {
        const response = await fetch("/api/events");
        type ApiEvent = Override<Event, { readonly startTime: string }>;
        const events = await extract<readonly ApiEvent[]>(response);

        return events.map(({ startTime, ...rest }) => ({
            ...rest,
            startTime: parseIsoOrThrow(startTime),
        }));
    }, undefined);

export const addEvent = async (
    event: Event,
    token: string | undefined
): Promise<void> => {
    const { eventId, ...rest } = event;
    const response = await fetch(`/api/events/${eventId}`, {
        headers: getHeaders(token),
        method: "PUT",
        body: JSON.stringify(rest),
    });
    throwIfNotOk(response);
};

export const deleteEvent = async (
    eventId: number,
    token: string | undefined
): Promise<void> => {
    const response = await fetch(`/api/events/${eventId}/`, {
        headers: getHeaders(token),
        method: "DELETE",
    });
    throwIfNotOk(response);
};
