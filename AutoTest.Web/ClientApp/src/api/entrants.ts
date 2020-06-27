import { ApiResponse, toApiResponse, Entrant } from "../types/models";
import { throwIfNotOk } from "./api";

export const getEntrants = async (
    eventId: number,
    token: string | undefined
): Promise<ApiResponse<readonly Entrant[]>> =>
    toApiResponse(async () => {
        const response = await fetch(`api/entrants/${eventId}`, {
            headers: { Authorization: token ? `Bearer ${token}` : "" },
        });
        throwIfNotOk(response);
        return (await response.json()) as Entrant[];
    });
