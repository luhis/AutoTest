import { parseIsoOrThrow } from "ts-date";

import { Override, EventNotification } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { extract, getHeaders, throwIfNotOk } from "./api";

export const getNotifications = async (
    eventId: number
): Promise<ApiResponse<readonly EventNotification[], number>> =>
    toApiResponse(async () => {
        const response = await fetch(`/api/notifications/${eventId}`);
        type ApiNotification = Override<
            EventNotification,
            { readonly created: string }
        >;
        const events = await extract<readonly ApiNotification[]>(response);

        return events.map(({ created, ...rest }) => ({
            ...rest,
            created: parseIsoOrThrow(created),
        }));
    }, eventId);

export const addNotification = async (
    notification: EventNotification,
    token: string | undefined
): Promise<void> => {
    const { notificationId, eventId, ...rest } = notification;
    const response = await fetch(
        `/api/notifications/${eventId}/${notificationId}`,
        {
            headers: getHeaders(token),
            method: "PUT",
            body: JSON.stringify(rest),
        }
    );
    throwIfNotOk(response);
};
