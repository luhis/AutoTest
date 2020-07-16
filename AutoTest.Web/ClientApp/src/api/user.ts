import { User } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { throwIfNotOk } from "./api";

export const getUsers = async (
    token: string | undefined
): Promise<ApiResponse<readonly User[]>> =>
    toApiResponse(async () => {
        const response = await fetch("/api/user", {
            headers: { Authorization: token ? `Bearer ${token}` : "" },
        });
        throwIfNotOk(response);
        return (await response.json()) as User[];
    });
