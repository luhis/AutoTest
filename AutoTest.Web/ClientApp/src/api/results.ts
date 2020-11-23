import { Result } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { throwIfNotOk } from "./api";
import { getBearerHeader } from "./headers";

export const getResults = async (
    eventId: number,
    token: string | undefined
): Promise<ApiResponse<readonly Result[]>> =>
    toApiResponse(async () => {
        const response = await fetch(`/api/results/${eventId}`, {
            headers: getBearerHeader(token),
        });
        throwIfNotOk(response);
        return (await response.json()) as readonly Result[];
    }, eventId);
