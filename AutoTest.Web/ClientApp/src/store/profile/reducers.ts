import { neverReached } from "../../types/shared";
import {
    ProfileState,
    ProfileActionTypes,
    GET_PROFILE,
    GET_ACCESS,
    RESET_ACCESS,
} from "./types";

const defaultAccess = {
    isLoggedIn: false,
    canViewClubs: false,
    canViewProfile: false,
};

const initialProfileState: ProfileState = {
    profile: { tag: "Idle" },
    access: defaultAccess,
};

export const profileReducer = (
    state = initialProfileState,
    action: ProfileActionTypes
): ProfileState => {
    switch (action.type) {
        case GET_PROFILE:
            return {
                ...state,
                profile: action.payload,
            };
        case GET_ACCESS:
            return {
                ...state,
                access: action.payload,
            };
        case RESET_ACCESS:
            return {
                ...state,
                access: defaultAccess,
            };
        default: {
            neverReached(action);
            return state;
        }
    }
};
