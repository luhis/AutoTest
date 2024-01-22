import { HookReturnValue } from "react-use-googlelogin/dist/types";

export const throwIfNotOk = (response: Response) => {
  if (!response.ok) {
    throw new Error(`Network response was not ok ${response.status}`);
  }
};

export const getAccessToken = ({
  googleUser,
}: HookReturnValue): string | undefined => {
  return googleUser ? googleUser.tokenId : undefined;
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
