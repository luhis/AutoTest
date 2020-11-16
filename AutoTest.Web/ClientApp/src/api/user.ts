import { Profile } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { throwIfNotOk } from "./api";

export const getProfile = async (
    token: string | undefined
): Promise<ApiResponse<Profile>> =>
    toApiResponse(async () => {
        const response = await fetch("/api/profile", {
            headers: { Authorization: token ? `Bearer ${token}` : "" },
        });
        throwIfNotOk(response);
        return (await response.json()) as Profile;
    });

export const saveProfile = async (
    profile: Profile,
    token: string | undefined
): Promise<void> => {
    const response = await fetch(`/api/profile/`, {
        headers: {
            "Content-Type": "application/json",
            Authorization: token ? `Bearer ${token}` : "",
        },
        method: "PUT",
        body: JSON.stringify(profile),
    });
    throwIfNotOk(response);
};
