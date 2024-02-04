import { parseIsoOrThrow } from "ts-date";

import { Event, EventStatus, Override } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { extract, getHeaders, throwIfNotOk } from "./api";

type ApiEvent = Override<
  Event,
  {
    readonly startTime: string;
    readonly entryCloseDate: string;
    readonly entryOpenDate: string;
  }
>;

export const getEvents = async (): Promise<ApiResponse<readonly Event[]>> =>
  toApiResponse(async () => {
    const response = await fetch("/api/events");
    const events = await extract<readonly ApiEvent[]>(response);

    return events.map(
      ({ startTime, entryCloseDate, entryOpenDate, ...rest }) => ({
        ...rest,
        startTime: parseIsoOrThrow(startTime),
        entryCloseDate: parseIsoOrThrow(entryCloseDate),
        entryOpenDate: parseIsoOrThrow(entryOpenDate),
      }),
    );
  }, undefined);

export const addEvent = async (
  event: Event,
  token: string | undefined,
): Promise<void> => {
  const { eventId, ...rest } = event;
  const response = await fetch(`/api/events/${eventId}`, {
    headers: getHeaders(token),
    method: "PUT",
    body: JSON.stringify(rest),
  });
  throwIfNotOk(response);
};

export const setEventStatus = async (
  eventId: number,
  eventStatus: EventStatus,
  token: string | undefined,
): Promise<void> => {
  const response = await fetch(`/api/events/setEventStatus/${eventId}`, {
    headers: getHeaders(token),
    method: "PUT",
    body: eventStatus.toString(),
  });
  throwIfNotOk(response);
};

export const deleteEvent = async (
  eventId: number,
  token: string | undefined,
): Promise<void> => {
  const response = await fetch(`/api/events/${eventId}/`, {
    headers: getHeaders(token),
    method: "DELETE",
  });
  throwIfNotOk(response);
};
