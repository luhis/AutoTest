import { Access } from "../types/profileModels";
import { extract, getHeaders } from "./api";

export const getAccess = async (token: string | undefined): Promise<Access> => {
  const response = await fetch("/api/access", {
    headers: getHeaders(token),
  });
  return await extract(response);
};
