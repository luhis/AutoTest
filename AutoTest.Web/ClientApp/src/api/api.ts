import { GoogleAuth } from "../types/models";

export const throwIfNotOk = (response: Response): void => {
    if (!response.ok) {
        throw new Error("Network response was not ok");
    }
};

export const getAccessToken = (auth: GoogleAuth): string | undefined => {
    return auth.googleUser ? auth.googleUser.tokenId : undefined;
};
