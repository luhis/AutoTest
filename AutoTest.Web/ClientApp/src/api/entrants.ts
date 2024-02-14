import { parseIsoOrThrow } from "ts-date";

import {
  Entrant,
  Override,
  Payment,
  PublicEntrant,
  SaveEntrant,
} from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { extract, getHeaders, throwIfNotOk } from "./api";

export const getEntrants = async (
  eventId: number,
): Promise<ApiResponse<readonly PublicEntrant[], number>> =>
  toApiResponse(async () => {
    const response = await fetch(`/api/entrants/${eventId}`, {
      headers: getHeaders(undefined),
    });
    const res = await extract<readonly ApiEntrant[]>(response);
    return res.map(
      ({ payment, ...rest }) =>
        <PublicEntrant>{
          ...rest,
          payment:
            payment !== null
              ? {
                  ...payment,
                  paidAt: parseIsoOrThrow(payment.paidAt),
                  timestamp: parseIsoOrThrow(payment.timestamp),
                }
              : null,
        },
    );
  }, eventId);

type ApiEntrant = Override<
  PublicEntrant,
  {
    readonly payment: Override<
      Payment,
      {
        readonly paidAt: string;
        readonly timestamp: string;
      }
    > | null;
  }
>;

export const getEntrant = async (
  eventId: number,
  entrantId: number,
  token: string | undefined,
): Promise<Entrant> => {
  const response = await fetch(`/api/entrants/${eventId}/${entrantId}`, {
    headers: getHeaders(token),
  });
  return await extract(response);
};

export const addEntrant = async (
  entrant: SaveEntrant,
  token: string | undefined,
): Promise<Entrant> => {
  const { entrantId, eventId, ...rest } = entrant;
  const response = await fetch(`/api/entrants/${eventId}/${entrantId}`, {
    headers: getHeaders(token),
    method: "PUT",
    body: JSON.stringify(rest),
  });
  return await extract(response);
};

export const markPaid = async (
  eventId: number,
  entrantId: number,
  payment: Payment | null,
  token: string | undefined,
): Promise<void> => {
  const response = await fetch(
    `/api/entrants/${eventId}/${entrantId}/markPaid`,
    {
      headers: getHeaders(token),
      method: "PUT",
      body: JSON.stringify(payment),
    },
  );
  throwIfNotOk(response);
};

export const deleteEntrant = async (
  eventId: number,
  entrantId: number,
  token: string | undefined,
): Promise<void> => {
  const response = await fetch(`/api/entrants/${eventId}/${entrantId}/`, {
    headers: getHeaders(token),
    method: "DELETE",
  });
  throwIfNotOk(response);
};
