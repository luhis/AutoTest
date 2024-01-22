import { Club } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { extract, getHeaders, throwIfNotOk } from "./api";

export const getClubs = async (
  token: string | undefined,
): Promise<ApiResponse<readonly Club[]>> =>
  toApiResponse(async () => {
    const response = await fetch("/api/clubs", {
      headers: getHeaders(token),
    });
    return await extract(response);
  }, undefined);

export const addClub = async (
  club: Club,
  token: string | undefined,
): Promise<void> => {
  const { clubId, ...rest } = club;
  const response = await fetch(`/api/clubs/${clubId}`, {
    headers: getHeaders(token),
    method: "PUT",
    body: JSON.stringify(rest),
  });
  throwIfNotOk(response);
};

export const deleteClub = async (
  clubId: number,
  token: string | undefined,
): Promise<void> => {
  const response = await fetch(`/api/clubs/${clubId}/`, {
    headers: getHeaders(token),
    method: "DELETE",
  });
  throwIfNotOk(response);
};
