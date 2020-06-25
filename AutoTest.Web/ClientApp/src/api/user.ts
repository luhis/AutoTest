import { User, ApiResponse, toApiResponse } from "../types/models";
import { throwIfNotOk } from "./api";

export const getClubs = async (
    token: string | undefined
): Promise<ApiResponse<readonly User[]>> =>
    toApiResponse(async () => {
        const response = await fetch("/api/user", {
            headers: { Authorization: token ? `Bearer ${token}` : "" },
        });
        throwIfNotOk(response);
        return (await response.json()) as User[];
    });
