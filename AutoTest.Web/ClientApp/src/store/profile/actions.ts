import { Dispatch } from "redux";

import { getProfile, saveProfile } from "../../api/user";
import { Profile } from "../../types/profileModels";
import {
    GET_ACCESS,
    GET_PROFILE,
    ProfileActionTypes,
    RESET_ACCESS,
} from "./types";
import { getAccess } from "../../api/access";

export const GetProfile =
    (token: string | undefined) =>
    async (dispatch: Dispatch<ProfileActionTypes>) => {
        dispatch({
            type: GET_PROFILE,
            payload: { tag: "Loading", id: undefined },
        });
        dispatch({
            type: GET_PROFILE,
            payload: await getProfile(token),
        });
    };

export const SaveProfile =
    (profile: Profile, token: string | undefined) =>
    async (dispatch: Dispatch<ProfileActionTypes>) => {
        await saveProfile(profile, token);
        await GetProfile(token)(dispatch);
    };

export const GetAccess =
    (token: string | undefined) =>
    async (dispatch: Dispatch<ProfileActionTypes>) => {
        if (token) {
            const access = await getAccess(token);
            dispatch({
                type: GET_ACCESS,
                payload: access,
            });
        } else {
            dispatch({
                type: RESET_ACCESS,
            });
        }
    };

export const ResetAccess = () => ({
    type: RESET_ACCESS,
});
