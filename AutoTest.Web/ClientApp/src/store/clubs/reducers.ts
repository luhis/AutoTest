import { ClubsActionTypes, ClubsState } from "./types";
import { ifLoaded } from "../../types/loadingState";
import { neverReached } from "../../types/shared";

const initialState: ClubsState = {
  clubs: { tag: "Idle" },
};

export const clubsReducer = (
  state = initialState,
  action: ClubsActionTypes,
): ClubsState => {
  switch (action.type) {
    case "CLEAR_CACHE":
      return {
        ...state,
        clubs: { tag: "Idle" },
      };
    case "GET_CLUBS":
      return {
        ...state,
        clubs: action.payload,
      };
    case "ADD_CLUB":
      return {
        ...state,
        clubs: ifLoaded(state.clubs, (c) =>
          c
            .filter(({ clubId }) => clubId !== action.payload.clubId)
            .concat(action.payload),
        ),
      };
    default: {
      neverReached(action);
      return state;
    }
  }
};
