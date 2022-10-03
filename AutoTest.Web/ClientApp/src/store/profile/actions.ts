import { ThunkAction } from "redux-thunk";

import { getProfile, saveProfile } from "../../api/user";
import { Profile } from "../../types/profileModels";
import {
    ADD_CLUB_ADMIN,
    ADD_EDITABLE_ENTRANT,
    ADD_EDITABLE_MARSHAL,
    ADD_EVENT_MARSHAL,
    GET_ACCESS,
    GET_PROFILE,
    ProfileActionTypes,
    REMOVE_CLUB_ADMIN,
    REMOVE_EVENT_MARSHAL,
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

export const AddClubAdmin = (clubId: number) => ({
    type: ADD_CLUB_ADMIN,
    payload: clubId,
});

export const RemoveClubAdmin = (clubId: number) => ({
    type: REMOVE_CLUB_ADMIN,
    payload: clubId,
});

export const AddEventMarshal = (eventId: number) => ({
    type: ADD_EVENT_MARSHAL,
    payload: eventId,
});
export const AddEditableMarshal = (marshalId: number) => ({
    type: ADD_EDITABLE_MARSHAL,
    payload: marshalId,
});
export const AddEditableEntrant = (entrantId: number) => ({
    type: ADD_EDITABLE_ENTRANT,
    payload: entrantId,
});

export const RemoveEventMarshal = (eventId: number) => ({
    type: REMOVE_EVENT_MARSHAL,
    payload: eventId,
});
