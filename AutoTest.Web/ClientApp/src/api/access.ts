import { Access } from "../types/profileModels";
import { throwIfNotOk } from "./api";

export const getAccess = async (token: string | undefined): Promise<Access> => {
    const response = await fetch("/api/access", {
        headers: { Authorization: token ? `Bearer ${token}` : "" },
    });
    throwIfNotOk(response);
    return (await response.json()) as Access;
};
