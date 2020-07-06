import { Entrant } from "../types/models";
import { ApiResponse, toApiResponse } from "../types/loadingState";
import { throwIfNotOk } from "./api";

export const getEntrants = async (
    eventId: number,
    token: string | undefined
): Promise<ApiResponse<readonly Entrant[]>> =>
    toApiResponse(async () => {
        const response = await fetch(`/api/entrants/${eventId}`, {
            headers: { Authorization: token ? `Bearer ${token}` : "" },
        });
        throwIfNotOk(response);
        return (await response.json()) as Entrant[];
    });

export const addEntrant = async (
    entrant: Entrant,
    token: string | undefined
): Promise<void> => {
    const { entrantId, ...rest } = entrant;
    const response = await fetch(`/api/entrants/${entrantId}`, {
        headers: {
            "Content-Type": "application/json",
            Authorization: token ? `Bearer ${token}` : "",
        },
        method: "PUT",
        body: JSON.stringify(rest),
    });
    throwIfNotOk(response);
};
