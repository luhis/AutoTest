// import { neverReached } from "../../types/shared";
import { ProfileState, ProfileActionTypes, GET_PROFILE } from "./types";

const initialProfileState: ProfileState = {
    profile: { tag: "Idle" },
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
        default: {
            // neverReached(action);
            return state;
        }
    }
};
