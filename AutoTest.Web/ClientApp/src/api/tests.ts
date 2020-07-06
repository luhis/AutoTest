import { Test } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { throwIfNotOk } from "./api";

export const getTests = async (
    eventId: number,
    token: string | undefined
): Promise<ApiResponse<readonly Test[]>> =>
    toApiResponse(async () => {
        const response = await fetch(`/api/tests/${eventId}`, {
            headers: { Authorization: token ? `Bearer ${token}` : "" },
        });
        throwIfNotOk(response);
        return (await response.json()) as Test[];
    });
