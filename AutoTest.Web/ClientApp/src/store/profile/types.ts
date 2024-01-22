import { Access, Profile } from "../../types/profileModels";
import { LoadingState } from "../../types/loadingState";

export const GET_PROFILE = "GET_PROFILE";
export const GET_ACCESS = "GET_ACCESS";
export const RESET_ACCESS = "RESET_ACCESS";
export const ADD_CLUB_ADMIN = "ADD_CLUB_ADMIN";
export const REMOVE_CLUB_ADMIN = "REMOVE_CLUB_ADMIN";
export const ADD_EVENT_MARSHAL = "ADD_EVENT_MARSHAL";
export const REMOVE_EVENT_MARSHAL = "REMOVE_EVENT_MARSHAL";
export const ADD_EDITABLE_MARSHAL = "ADD_EDITABLE_MARSHAL";
export const ADD_EDITABLE_ENTRANT = "ADD_EDITABLE_ENTRANT";

export interface ProfileState {
  readonly profile: LoadingState<Profile>;
  readonly access: Access;
}

interface GetProfile {
  readonly type: typeof GET_PROFILE;
  readonly payload: LoadingState<Profile>;
}

interface GetAccess {
  readonly type: typeof GET_ACCESS;
  readonly payload: Access;
}

interface AddClubAdmin {
  readonly type: typeof ADD_CLUB_ADMIN;
  readonly payload: number;
}

interface RemoveClubAdmin {
  readonly type: typeof REMOVE_CLUB_ADMIN;
  readonly payload: number;
}

interface AddEventMarshal {
  readonly type: typeof ADD_EVENT_MARSHAL;
  readonly payload: number;
}

interface AddEditableMarshal {
  readonly type: typeof ADD_EDITABLE_MARSHAL;
  readonly payload: number;
}

interface AddEditableEntrant {
  readonly type: typeof ADD_EDITABLE_ENTRANT;
  readonly payload: number;
}

interface RemoveEventMarshal {
  readonly type: typeof REMOVE_EVENT_MARSHAL;
  readonly payload: number;
}

interface ResetAccess {
  readonly type: typeof RESET_ACCESS;
}

export type ProfileActionTypes =
  | GetProfile
  | GetAccess
  | ResetAccess
  | AddClubAdmin
  | RemoveClubAdmin
  | AddEventMarshal
  | RemoveEventMarshal
  | AddEditableMarshal
  | AddEditableEntrant;
