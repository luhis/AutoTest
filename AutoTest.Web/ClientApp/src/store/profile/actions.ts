import { ThunkAction } from "redux-thunk";

import { getProfile, saveProfile } from "../../api/user";
import { Profile } from "../../types/profileModels";
import {
    GET_ACCESS,
    GET_PROFILE,
    ProfileActionTypes,
    RESET_ACCESS,
} from "./types";
import { getAccess } from "../../api/access";
import { selectProfile } from "./selectors";
import { AppState } from "..";
import { isStale, requiresLoading } from "../../types/loadingState";

export const GetProfileIfRequired =
    (
        token: string | undefined
    ): ThunkAction<Promise<void>, AppState, unknown, ProfileActionTypes> =>
    async (dispatch, getState) => {
        const profile = selectProfile(getState());
        if (requiresLoading(profile.tag) || isStale(profile)) {
            await GetProfile(token)(dispatch, getState, undefined);
        }
    };

const GetProfile =
    (
        token: string | undefined
    ): ThunkAction<Promise<void>, AppState, unknown, ProfileActionTypes> =>
    async (dispatch) => {
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
    (
        profile: Profile,
        token: string | undefined
    ): ThunkAction<Promise<void>, AppState, unknown, ProfileActionTypes> =>
    async (dispatch, getState) => {
        await saveProfile(profile, token);
        await GetProfile(token)(dispatch, getState, undefined);
    };

export const GetAccess =
    (
        token: string | undefined
    ): ThunkAction<Promise<void>, AppState, unknown, ProfileActionTypes> =>
    async (dispatch) => {
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
