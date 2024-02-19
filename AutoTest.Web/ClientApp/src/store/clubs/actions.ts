import { ThunkAction } from "redux-thunk";

import { EditingClub } from "src/types/models";
import { addClub, deleteClub, getClubs } from "../../api/clubs";
import { selectClubs } from "./selectors";
import { AppState } from "..";
import { canUpdate, isStale, requiresLoading } from "../../types/loadingState";
import { ClubsActionTypes } from "./types";
import { showError } from "../../lib/apiErrorToast";

export const GetClubsIfRequired =
  (
    token: string | undefined,
  ): ThunkAction<void, AppState, unknown, ClubsActionTypes> =>
  async (dispatch, getState) => {
    const clubs = selectClubs(getState());
    if (requiresLoading(clubs.tag) || isStale(clubs)) {
      if (clubs.tag === "Idle") {
        dispatch({
          type: "GET_CLUBS",
          payload: { tag: "Loading", id: undefined },
        });
      }
      const res = await getClubs(token);
      if (canUpdate(clubs, res)) {
        dispatch({
          type: "GET_CLUBS",
          payload: res,
        });
      }
    }
  };

export const AddClub =
  (
    club: EditingClub,
    token: string | undefined,
    onSuccess: () => void,
  ): ThunkAction<Promise<void>, AppState, unknown, ClubsActionTypes> =>
  async (dispatch) => {
    try {
      await addClub(club, token);
      dispatch({
        type: "ADD_CLUB",
        payload: club,
      });
      onSuccess();
    } catch (error) {
      showError(error);
    }
  };

export const DeleteClub =
  (
    clubId: number,
    token: string | undefined,
  ): ThunkAction<Promise<void>, AppState, unknown, ClubsActionTypes> =>
  async (dispatch) => {
    await deleteClub(clubId, token);
    dispatch({
      type: "GET_CLUBS",
      payload: { tag: "Loading", id: undefined },
    });
    dispatch({
      type: "GET_CLUBS",
      payload: await getClubs(token),
    });
  };
