import { Override, TestRun } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { extract, getHeaders, throwIfNotOk } from "./api";
import { parseIsoOrThrow } from "ts-date";

type TestRunApi = Override<TestRun, { readonly created: string }>;

export const getTestRuns = async (
    eventId: number,
    ordinal: number,
    token: string | undefined
): Promise<
    ApiResponse<
        readonly TestRun[],
        { readonly eventId: number; readonly ordinal: number }
    >
> =>
    toApiResponse(
        async () => {
            const response = await fetch(
                `/api/events/${eventId}/tests/${ordinal}/testRuns/`,
                {
                    headers: getHeaders(token),
                }
            );
            const runs = await extract<readonly TestRunApi[]>(response);
            return runs.map(({ created, ...rest }) => ({
                ...rest,
                created: parseIsoOrThrow(created),
            }));
        },
        { eventId, ordinal }
    );

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
