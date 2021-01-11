import { TestRun } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { extract, getHeaders, throwIfNotOk } from "./api";

export const getTestRuns = async (
    eventId: number,
    ordinal: number,
    token: string | undefined
): Promise<ApiResponse<readonly TestRun[], number>> =>
    toApiResponse(async () => {
        const response = await fetch(
            `/api/events/${eventId}/tests/${ordinal}/testRuns/`,
            {
                headers: getHeaders(token),
            }
        );
        return await extract(response);
    }, eventId);

export const addTestRun = async (
    eventId: number,
    testRun: TestRun,
    token: string | undefined
): Promise<ApiResponse<void>> =>
    toApiResponse(async () => {
        const { testRunId, ordinal, ...rest } = testRun;
        const response = await fetch(
            `/api/events/${eventId}/tests/${ordinal}/testRuns/${testRunId}`,
            {
                method: "PUT",
                body: JSON.stringify(rest),
                headers: getHeaders(token),
            }
        );
        throwIfNotOk(response);
    }, undefined);
