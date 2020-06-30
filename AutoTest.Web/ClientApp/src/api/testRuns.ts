import { ApiResponse, toApiResponse, TestRun } from "../types/models";
import { throwIfNotOk } from "./api";

export const getTestRuns = async (
    testId: number,
    token: string | undefined
): Promise<ApiResponse<readonly TestRun[]>> =>
    toApiResponse(async () => {
        const response = await fetch(`/api/testRuns/${testId}`, {
            headers: { Authorization: token ? `Bearer ${token}` : "" },
        });
        throwIfNotOk(response);
        return (await response.json()) as TestRun[];
    });

export const addTestRun = async (
    testRun: TestRun,
    token: string | undefined
): Promise<ApiResponse<readonly TestRun[]>> =>
    toApiResponse(async () => {
        const { testRunId, ...rest } = testRun;
        const response = await fetch(`/api/testRuns/${testRunId}`, {
            method: "PUT",
            body: JSON.stringify(rest),
            headers: { Authorization: token ? `Bearer ${token}` : "" },
        });
        throwIfNotOk(response);
        return (await response.json()) as TestRun[];
    });
