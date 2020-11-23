import { parseIsoOrThrow } from "ts-date";

import { Profile } from "../types/profileModels";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { ClubMembership } from "../types/shared";
import { throwIfNotOk } from "./api";
import { Override } from "../types/models";
import { getBearerHeader, getHeaders } from "./headers";

export const getProfile = async (
    token: string | undefined
): Promise<ApiResponse<Profile>> =>
    toApiResponse(async () => {
        const response = await fetch("/api/profile", {
            headers: getBearerHeader(token),
        });
        throwIfNotOk(response);
        type ApiProfile = Override<
            Profile,
            {
                readonly clubMemberships: readonly Override<
                    ClubMembership,
                    { readonly expiry: string }
                >[];
            }
        >;
        const received = (await response.json()) as ApiProfile;
        return {
            ...received,
            clubMemberships: received.clubMemberships.map(
                ({ expiry, ...rest }) => ({
                    ...rest,
                    expiry: parseIsoOrThrow(expiry),
                })
            ),
        };
    });

export const saveProfile = async (
    profile: Profile,
    token: string | undefined
): Promise<void> => {
    const response = await fetch(`/api/profile/`, {
        headers: getHeaders(token),
        method: "PUT",
        body: JSON.stringify(profile),
    });
    throwIfNotOk(response);
};
