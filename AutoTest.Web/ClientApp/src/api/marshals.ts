import { Marshal } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { extract, getHeaders, throwIfNotOk } from "./api";

export const getMarshals = async (
    eventId: number,
    token: string | undefined
): Promise<ApiResponse<readonly Marshal[], number>> =>
    toApiResponse(async () => {
        const response = await fetch(`/api/marshals/${eventId}`, {
            headers: getHeaders(token),
        });
        return await extract(response);
    }, eventId);

export const getMarshal = async (
    eventId: number,
    marshalId: number,
    token: string | undefined
): Promise<ApiResponse<readonly Marshal[], number>> =>
    toApiResponse(async () => {
        const response = await fetch(`/api/marshal/${eventId}/${marshalId}`, {
            headers: getHeaders(token),
        });
        return await extract(response);
    }, eventId);

export const addMarshal = async (
    entrant: Marshal,
    token: string | undefined
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
    token: string | undefined
): Promise<void> => {
    const response = await fetch(`/api/marshals/${eventId}/${marshalId}/`, {
        headers: getHeaders(token),
        method: "DELETE",
    });
    throwIfNotOk(response);
};
