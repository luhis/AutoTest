import { Entrant } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { throwIfNotOk } from "./api";
import { getBearerHeader, getHeaders } from "./headers";

export const getEntrants = async (
    eventId: number,
    token: string | undefined
): Promise<ApiResponse<readonly Entrant[], number>> =>
    toApiResponse(async () => {
        const response = await fetch(`/api/entrants/${eventId}`, {
            headers: getBearerHeader(token),
        });
        throwIfNotOk(response);
        return (await response.json()) as readonly Entrant[];
    }, eventId);

export const addEntrant = async (
    entrant: Entrant,
    token: string | undefined
): Promise<void> => {
    const { entrantId, eventId, ...rest } = entrant;
    const response = await fetch(`/api/entrants/${eventId}/${entrantId}`, {
        headers: getHeaders(token),
        method: "PUT",
        body: JSON.stringify(rest),
    });
    throwIfNotOk(response);
};

export const markPaid = async (
    eventId: number,
    entrantId: number,
    isPaid: boolean,
    token: string | undefined
): Promise<void> => {
    const response = await fetch(
        `/api/entrants/${eventId}/${entrantId}/markPaid`,
        {
            headers: getHeaders(token),
            method: "PUT",
            body: JSON.stringify(isPaid),
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
        headers: getBearerHeader(token),
        method: "DELETE",
    });
    throwIfNotOk(response);
};
