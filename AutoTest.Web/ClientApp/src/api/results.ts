import { Result } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { extract, getHeaders } from "./api";

export const getResults = async (
    eventId: number,
    token: string | undefined,
): Promise<ApiResponse<readonly Result[], number>> =>
    toApiResponse(async () => {
        const response = await fetch(`/api/results/${eventId}`, {
            headers: getHeaders(token),
        });
        return await extract(response);
    }, eventId);
