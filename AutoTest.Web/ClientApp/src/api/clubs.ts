import { Club, ApiResponse, toApiResponse } from "../types/models";
import { throwIfNotOk } from "./api";

export const getClubs = async (
    token: string | undefined
): Promise<ApiResponse<readonly Club[]>> =>
    toApiResponse(async () => {
        const response = await fetch("api/clubs", {
            headers: { Authorization: token ? `Bearer ${token}` : "" },
        });
        throwIfNotOk(response);
        return (await response.json()) as Club[];
    });
