import { ApiResponse, toApiResponse, TestRun } from "../types/models";
import { throwIfNotOk } from "./api";

export const getTestRuns = async (
    eventId: number,
    token: string | undefined
): Promise<ApiResponse<readonly TestRun[]>> =>
    toApiResponse(async () => {
        const response = await fetch(`api/testRuns/${eventId}`, {
            headers: { Authorization: token ? `Bearer ${token}` : "" },
        });
        throwIfNotOk(response);
        return (await response.json()) as TestRun[];
    });
