import { Result } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { extract, getBearerHeader } from "./api";

export const getResults = async (
    eventId: number,
    token: string | undefined
): Promise<ApiResponse<readonly Result[], number>> =>
    toApiResponse(async () => {
        const response = await fetch(`/api/results/${eventId}`, {
            headers: getBearerHeader(token),
        });
        return await extract(response);
    }, eventId);
