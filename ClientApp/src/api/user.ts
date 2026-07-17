import { formatDateIso, parseIsoOrThrow } from "ts-date";

import { Profile } from "../types/profileModels";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { ClubMembership } from "../types/shared";
import { throwIfNotOk, getHeaders, extract } from "./api";
import { Override } from "../types/models";

export const getProfile = async (
  token: string | undefined,
): Promise<ApiResponse<Profile>> =>
  toApiResponse(async () => {
    const response = await fetch("/api/profile", {
      headers: getHeaders(token),
    });
    await throwIfNotOk(response);
    type ApiProfile = Override<
      Profile,
      {
        readonly clubMemberships: readonly Override<
          ClubMembership,
          { readonly expiry: string }
        >[];
      }
    >;
    const received = await extract<ApiProfile>(response);
    return {
      ...received,
      clubMemberships: received.clubMemberships.map(({ expiry, ...rest }) => ({
        ...rest,
        expiry: parseIsoOrThrow(expiry),
      })),
    };
  }, undefined);

export const saveProfile = async (
  profile: Profile,
  token: string | undefined,
): Promise<void> => {
  const saveableProfile = {
    ...profile,
    clubMemberships: profile.clubMemberships.map((membership) => ({
      ...membership,
      expiry: formatDateIso(membership.expiry),
    })),
  };
  const response = await fetch(`/api/profile/`, {
    headers: getHeaders(token),
    method: "PUT",
    body: JSON.stringify(saveableProfile),
  });
  await throwIfNotOk(response);
};
