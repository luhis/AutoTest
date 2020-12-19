import { parseIsoOrThrow } from "ts-date";

import { Override, Notification } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { extract, getHeaders, throwIfNotOk } from "./api";

export const getNotifications = async (
    eventId: number
): Promise<ApiResponse<readonly Notification[], number>> =>
    toApiResponse(async () => {
        const response = await fetch(`/api/notifications/${eventId}`);
        type ApiNotification = Override<
            Notification,
            { readonly created: string }
        >;
        const events = await extract<readonly ApiNotification[]>(response);

        return events.map(({ created, ...rest }) => ({
            ...rest,
            created: parseIsoOrThrow(created),
        }));
    }, eventId);

export const addNotification = async (
    notification: Notification,
    token: string | undefined
): Promise<void> => {
    const { notificationId, ...rest } = notification;
    const response = await fetch(`/api/notifications/${notificationId}`, {
        headers: getHeaders(token),
        method: "PUT",
        body: JSON.stringify(rest),
    });
    throwIfNotOk(response);
};
