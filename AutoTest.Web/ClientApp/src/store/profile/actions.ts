import { Dispatch } from "redux";

import { GET_PROFILE, ProfileActionTypes } from "./types";
import { getProfile } from "../../api/user";

export const GetProfile = (token: string | undefined) => async (
    dispatch: Dispatch<ProfileActionTypes>
) => {
    dispatch({
        type: GET_PROFILE,
        payload: { tag: "Loading", id: undefined },
    });
    dispatch({
        type: GET_PROFILE,
        payload: await getProfile(token),
    });
};
