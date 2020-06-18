import { ApiResponse, toApiResponse, Entrant } from "../types/models";
import { throwIfNotOk } from "./api";

export const getEntrants = async (): Promise<ApiResponse<readonly Entrant[]>> =>
    toApiResponse(async () => {
        const response = await fetch("api/entrants");
        throwIfNotOk(response);
        return (await response.json()) as Entrant[];
    });
