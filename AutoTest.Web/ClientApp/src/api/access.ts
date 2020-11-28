import { Access } from "../types/profileModels";
import { extract, getBearerHeader } from "./api";

export const getAccess = async (token: string | undefined): Promise<Access> => {
    const response = await fetch("/api/access", {
        headers: getBearerHeader(token),
    });
    return await extract(response);
};
