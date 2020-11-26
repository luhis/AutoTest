import { TestRun } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { throwIfNotOk } from "./api";
import { getBearerHeader, getHeaders } from "./headers";

export const getTestRuns = async (
    testId: number,
    token: string | undefined
): Promise<ApiResponse<readonly TestRun[], number>> =>
    toApiResponse(async () => {
        const response = await fetch(`/api/testRuns/${testId}`, {
            headers: getBearerHeader(token),
        });
        throwIfNotOk(response);
        return (await response.json()) as readonly TestRun[];
    }, testId);

export const addTestRun = async (
    eventId: number,
    testRun: TestRun,
    token: string | undefined
): Promise<ApiResponse<undefined>> =>
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
        return undefined;
    }, undefined);
