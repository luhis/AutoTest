import { Access } from "../types/profileModels";
import { throwIfNotOk } from "./api";
import { getBearerHeader } from "./headers";

export const getAccess = async (token: string | undefined): Promise<Access> => {
    const response = await fetch("/api/access", {
        headers: getBearerHeader(token),
    });
    throwIfNotOk(response);
    return (await response.json()) as Access;
};
