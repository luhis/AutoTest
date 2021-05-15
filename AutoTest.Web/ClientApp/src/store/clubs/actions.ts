import { Dispatch } from "redux";

import { EditingClub } from "src/types/models";
import { addClub, deleteClub, getClubs } from "../../api/clubs";
import { selectClubs } from "./selectors";
import { AppState } from "..";
import { canUpdate, isStale, requiresLoading } from "../../types/loadingState";
import { ADD_CLUB, ClubsActionTypes, GET_CLUBS } from "./types";

export const GetClubsIfRequired =
    (token: string | undefined) =>
    async (dispatch: Dispatch<ClubsActionTypes>, getState: () => AppState) => {
        const clubs = selectClubs(getState());
        if (requiresLoading(clubs.tag) || isStale(clubs)) {
            if (clubs.tag === "Idle") {
                dispatch({
                    type: GET_CLUBS,
                    payload: { tag: "Loading", id: undefined },
                });
            }
            const res = await getClubs(token);
            if (canUpdate(clubs, res)) {
                dispatch({
                    type: GET_CLUBS,
                    payload: res,
                });
            }
        }
    };

export const AddClub =
    (club: EditingClub, token: string | undefined, onSuccess: () => void) =>
    async (dispatch: Dispatch<ClubsActionTypes>) => {
        await addClub(club, token);
        dispatch({
            type: ADD_CLUB,
            payload: club,
        });
        onSuccess();
    };

export const DeleteClub =
    (clubId: number, token: string | undefined) =>
    async (dispatch: Dispatch<ClubsActionTypes>) => {
        await deleteClub(clubId, token);
        dispatch({
            type: GET_CLUBS,
            payload: { tag: "Loading", id: undefined },
        });
        dispatch({
            type: GET_CLUBS,
            payload: await getClubs(token),
        });
    };
