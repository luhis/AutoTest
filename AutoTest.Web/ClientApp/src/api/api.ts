import { HookReturnValue } from "react-use-googlelogin/dist/types";

export const throwIfNotOk = (response: Response): void => {
    if (!response.ok) {
        throw new Error(`Network response was not ok ${response.status}`);
    }
};

export const getAccessToken = ({
    googleUser,
}: HookReturnValue): string | undefined => {
    return googleUser ? googleUser.tokenId : undefined;
};
