import { CredentialResponse } from "@react-oauth/google";

export const throwIfNotOk = (response: Response) => {
  if (!response.ok) {
    throw new Error(`Network response was not ok ${response.status}`);
  }
};

export const getAccessToken = (
  t: CredentialResponse | undefined,
): string | undefined => {
  return t !== undefined ? t.credential : undefined;
};

export const getHeaders = (token: string | undefined) => ({
  Authorization: token ? `Bearer ${token}` : "",
  "Content-Type": "application/json",
  Accept: "application/json",
});

export const extract = async <T>(response: Response): Promise<T> => {
  throwIfNotOk(response);
  return (await response.json()) as T;
};
