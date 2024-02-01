import { Access, Profile } from "../../types/profileModels";
import { LoadingState } from "../../types/loadingState";

export interface ProfileState {
  readonly profile: LoadingState<Profile>;
  readonly access: Access;
}

interface GetProfile {
  readonly type: "GET_PROFILE";
  readonly payload: LoadingState<Profile>;
}

interface GetAccess {
  readonly type: "GET_ACCESS";
  readonly payload: Access;
}

interface AddClubAdmin {
  readonly type: "ADD_CLUB_ADMIN";
  readonly payload: number;
}

interface RemoveClubAdmin {
  readonly type: "REMOVE_CLUB_ADMIN";
  readonly payload: number;
}

interface AddEventMarshal {
  readonly type: "ADD_EVENT_MARSHAL";
  readonly payload: number;
}

interface AddEditableMarshal {
  readonly type: "ADD_EDITABLE_MARSHAL";
  readonly payload: number;
}

interface AddEditableEntrant {
  readonly type: "ADD_EDITABLE_ENTRANT";
  readonly payload: number;
}

interface RemoveEventMarshal {
  readonly type: "REMOVE_EVENT_MARSHAL";
  readonly payload: number;
}

interface ResetAccess {
  readonly type: "RESET_ACCESS";
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
