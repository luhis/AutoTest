import { Access } from "../../types/profileModels";
import { neverReached } from "../../types/shared";
import { ProfileState, ProfileActionTypes } from "./types";

const defaultAccess: Access = {
  isLoggedIn: false,
  canViewClubs: false,
  canViewProfile: false,
  isRootAdmin: false,
  adminClubs: [],
  marshalEvents: [],
  editableEntrants: [],
  editableMarshals: [],
};

const initialProfileState: ProfileState = {
  profile: { tag: "Idle" },
  access: defaultAccess,
};

export const profileReducer = (
  state = initialProfileState,
  action: ProfileActionTypes,
): ProfileState => {
  switch (action.type) {
    case "GET_PROFILE":
      return {
        ...state,
        profile: action.payload,
      };
    case "GET_ACCESS":
      return {
        ...state,
        access: action.payload,
      };
    case "RESET_ACCESS":
      return {
        ...state,
        access: defaultAccess,
      };
    case "ADD_CLUB_ADMIN":
      return {
        ...state,
        access: {
          ...state.access,
          adminClubs: state.access.adminClubs.concat(action.payload),
        },
      };
    case "REMOVE_CLUB_ADMIN":
      return {
        ...state,
        access: {
          ...state.access,
          adminClubs: state.access.adminClubs.filter(
            (id) => id !== action.payload,
          ),
        },
      };
    case "ADD_EVENT_MARSHAL":
      return {
        ...state,
        access: {
          ...state.access,
          marshalEvents: state.access.marshalEvents.concat(action.payload),
        },
      };
    case "REMOVE_EVENT_MARSHAL":
      return {
        ...state,
        access: {
          ...state.access,
          marshalEvents: state.access.marshalEvents.filter(
            (id) => id !== action.payload,
          ),
        },
      };
    case "ADD_EDITABLE_ENTRANT":
      return {
        ...state,
        access: {
          ...state.access,
          editableEntrants: state.access.editableEntrants.concat(
            action.payload,
          ),
        },
      };
    case "ADD_EDITABLE_MARSHAL":
      return {
        ...state,
        access: {
          ...state.access,
          editableMarshals: state.access.editableMarshals.concat(
            action.payload,
          ),
        },
      };
    default: {
      neverReached(action);
      return state;
    }
  }
};
