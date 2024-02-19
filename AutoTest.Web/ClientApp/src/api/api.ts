import { CredentialResponse } from "@react-oauth/google";
import { startCase } from "@s-libs/micro-dash";

type ValidationResponse = {
  readonly type: string;
  readonly title: string;
  readonly status: number;
  readonly traceId: string;
  readonly errors: { readonly [field: string]: string };
};

export const throwIfNotOk = async (response: Response) => {
  if (!response.ok) {
    if (response.status === 400) {
      const validationError = (await response.json()) as ValidationResponse;
      throw new Error(
        `Validation errors with fields: ${Object.keys(validationError.errors).map(startCase).join(", ")}`,
      );
    } else {
      throw new Error(`Network response was not ok ${response.status}`);
    }
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
  await throwIfNotOk(response);
  return (await response.json()) as T;
};
