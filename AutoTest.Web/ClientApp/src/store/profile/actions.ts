import { ThunkAction } from "redux-thunk";

import { getProfile, saveProfile } from "../../api/user";
import { Profile } from "../../types/profileModels";
import { ProfileActionTypes } from "./types";
import { getAccess } from "../../api/access";
import { selectProfile } from "./selectors";
import { AppState } from "..";
import { isStale, requiresLoading } from "../../types/loadingState";

export const GetProfileIfRequired =
  (
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, ProfileActionTypes> =>
  async (dispatch, getState) => {
    const profile = selectProfile(getState());
    if (requiresLoading(profile.tag) || isStale(profile)) {
      await GetProfile(token)(dispatch, getState, undefined);
    }
  };

const GetProfile =
  (
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, ProfileActionTypes> =>
  async (dispatch) => {
    dispatch({
      type: "GET_PROFILE",
      payload: { tag: "Loading", id: undefined },
    });
    dispatch({
      type: "GET_PROFILE",
      payload: await getProfile(token),
    });
  };

export const SaveProfile =
  (
    profile: Profile,
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, ProfileActionTypes> =>
  async (dispatch, getState) => {
    await saveProfile(profile, token);
    await GetProfile(token)(dispatch, getState, undefined);
  };

export const GetAccess =
  (
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, ProfileActionTypes> =>
  async (dispatch) => {
    if (token) {
      const access = await getAccess(token);
      dispatch({
        type: "GET_ACCESS",
        payload: access,
      });
    } else {
      dispatch(ResetAccess());
    }
  };

export const ResetAccess = (): ProfileActionTypes => ({
  type: "RESET_ACCESS",
});

export const AddClubAdmin = (clubId: number): ProfileActionTypes => ({
  type: "ADD_CLUB_ADMIN",
  payload: clubId,
});

export const RemoveClubAdmin = (clubId: number): ProfileActionTypes => ({
  type: "REMOVE_CLUB_ADMIN",
  payload: clubId,
});

export const AddEventMarshal = (eventId: number): ProfileActionTypes => ({
  type: "ADD_EVENT_MARSHAL",
  payload: eventId,
});
export const AddEditableMarshal = (marshalId: number): ProfileActionTypes => ({
  type: "ADD_EDITABLE_MARSHAL",
  payload: marshalId,
});
export const AddEditableEntrant = (entrantId: number): ProfileActionTypes => ({
  type: "ADD_EDITABLE_ENTRANT",
  payload: entrantId,
});

export const RemoveEventMarshal = (eventId: number): ProfileActionTypes => ({
  type: "REMOVE_EVENT_MARSHAL",
  payload: eventId,
});
