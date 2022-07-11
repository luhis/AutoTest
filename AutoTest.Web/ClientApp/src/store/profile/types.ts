import { Access, Profile } from "../../types/profileModels";
import { LoadingState } from "../../types/loadingState";

export const GET_PROFILE = "GET_PROFILE";
export const GET_ACCESS = "GET_ACCESS";
export const RESET_ACCESS = "RESET_ACCESS";
export const ADD_CLUB_ADMIN = "ADD_CLUB_ADMIN";

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

interface AddCLubAdmin {
    readonly type: typeof ADD_CLUB_ADMIN;
    readonly payload: number;
}

interface ResetAccess {
    readonly type: typeof RESET_ACCESS;
}

export type ProfileActionTypes =
    | GetProfile
    | GetAccess
    | ResetAccess
    | AddCLubAdmin;
