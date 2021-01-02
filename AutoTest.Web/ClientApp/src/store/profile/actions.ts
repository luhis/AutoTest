import { Dispatch } from "redux";

import { GET_PROFILE, ProfileActionTypes } from "./types";
import { getProfile, saveProfile } from "../../api/user";
import { Profile } from "../../types/profileModels";

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

export const SaveProfile = (
    profile: Profile,
    token: string | undefined
) => async (dispatch: Dispatch<ProfileActionTypes>) => {
    await saveProfile(profile, token);
    await GetProfile(token)(dispatch);
};
