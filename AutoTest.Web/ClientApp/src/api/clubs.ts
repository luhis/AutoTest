import { Club } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { throwIfNotOk } from "./api";
import { getBearerHeader, getHeaders } from "./headers";

export const getClubs = async (
    token: string | undefined
): Promise<ApiResponse<readonly Club[]>> =>
    toApiResponse(async () => {
        const response = await fetch("/api/clubs", {
            headers: getBearerHeader(token),
        });
        throwIfNotOk(response);
        return (await response.json()) as readonly Club[];
    });

export const addClub = async (
    club: Club,
    token: string | undefined
): Promise<void> => {
    const { clubId, ...rest } = club;
    const response = await fetch(`/api/clubs/${clubId}`, {
        headers: getHeaders(token),
        method: "PUT",
        body: JSON.stringify(rest),
    });
    throwIfNotOk(response);
};
