import { Profile } from "../../types/profileModels";
import { LoadingState } from "../../types/loadingState";

export const GET_PROFILE = "GET_PROFILE";

export interface ProfileState {
    readonly profile: LoadingState<Profile>;
}

interface GetProfile {
    readonly type: typeof GET_PROFILE;
    readonly payload: LoadingState<Profile>;
}

export type ProfileActionTypes = GetProfile;
