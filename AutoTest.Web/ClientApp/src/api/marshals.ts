import { Marshal, PublicMarshal } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { extract, getHeaders, throwIfNotOk } from "./api";

export const getMarshals = async (
    eventId: number,
): Promise<ApiResponse<readonly PublicMarshal[], number>> =>
    toApiResponse(async () => {
        const response = await fetch(`/api/marshals/${eventId}`, {
            headers: getHeaders(undefined),
        });
        return await extract(response);
    }, eventId);

export const getMarshal = async (
    eventId: number,
    marshalId: number,
    token: string | undefined,
): Promise<Marshal> => {
    const response = await fetch(`/api/marshals/${eventId}/${marshalId}`, {
        headers: getHeaders(token),
    });
    return await extract(response);
};

export const addMarshal = async (
    entrant: Marshal,
    token: string | undefined,
): Promise<Marshal> => {
    const { marshalId, eventId, ...rest } = entrant;
    const response = await fetch(`/api/marshals/${eventId}/${marshalId}`, {
        headers: getHeaders(token),
        method: "PUT",
        body: JSON.stringify(rest),
    });
    return await extract(response);
};

export const deleteMarshal = async (
    eventId: number,
    marshalId: number,
    token: string | undefined,
): Promise<void> => {
    const response = await fetch(`/api/marshals/${eventId}/${marshalId}/`, {
        headers: getHeaders(token),
        method: "DELETE",
    });
    throwIfNotOk(response);
};
