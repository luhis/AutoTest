import { Entrant, Payment, PublicEntrant } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { extract, getHeaders, throwIfNotOk } from "./api";

export const getEntrants = async (
    eventId: number
): Promise<ApiResponse<readonly PublicEntrant[], number>> =>
    toApiResponse(async () => {
        const response = await fetch(`/api/entrants/${eventId}`, {
            headers: getHeaders(undefined),
        });
        return await extract(response);
    }, eventId);

export const getEntrant = async (
    eventId: number,
    entrantId: number,
    token: string | undefined
): Promise<Entrant> => {
    const response = await fetch(`/api/entrants/${eventId}/${entrantId}`, {
        headers: getHeaders(token),
    });
    return await extract(response);
};

export const addEntrant = async (
    entrant: Entrant,
    token: string | undefined
): Promise<Entrant> => {
    const { entrantId, eventId, ...rest } = entrant;
    const response = await fetch(`/api/entrants/${eventId}/${entrantId}`, {
        headers: getHeaders(token),
        method: "PUT",
        body: JSON.stringify(rest),
    });
    return await extract(response);
};

export const markPaid = async (
    eventId: number,
    entrantId: number,
    payment: Payment | undefined,
    token: string | undefined
): Promise<void> => {
    const response = await fetch(
        `/api/entrants/${eventId}/${entrantId}/markPaid`,
        {
            headers: getHeaders(token),
            method: "PUT",
            body: JSON.stringify(payment),
        }
    );
    throwIfNotOk(response);
};

export const deleteEntrant = async (
    eventId: number,
    entrantId: number,
    token: string | undefined
): Promise<void> => {
    const response = await fetch(`/api/entrants/${eventId}/${entrantId}/`, {
        headers: getHeaders(token),
        method: "DELETE",
    });
    throwIfNotOk(response);
};
