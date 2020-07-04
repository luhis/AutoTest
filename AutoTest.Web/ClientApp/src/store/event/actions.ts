import { Dispatch } from "redux";

import { GET_ENTRANTS, ADD_TEST_RUN, EventActionTypes } from "./types";
import { TestRun } from "../../types/models";
import { getEntrants } from "../../api/entrants";

export const GetEntrants = (
    eventId: number,
    token: string | undefined
) => async (dispatch: Dispatch<EventActionTypes>) => {
    dispatch({
        type: GET_ENTRANTS,
        payload: { tag: "Loading" },
    });
    const entrants = await getEntrants(eventId, token);
    return {
        type: GET_ENTRANTS,
        payload: entrants,
    };
};

export const AddTestRun = (testRun: TestRun) => ({
    type: ADD_TEST_RUN,
    payload: testRun,
});
