import { ClearCache } from "../store/event/actions";
import { ClubsActionTypes, ClubsState } from "../store/clubs/types";
import { clubsReducer } from "../store/clubs/reducers";

const populatedState: ClubsState = {
    clubs: { tag: "Error", value: "Fail" },
};

describe("Club Reducer", () => {
    test("Clear Cache", () => {
        const finalState = clubsReducer(
            populatedState,
            ClearCache() as ClubsActionTypes
        );
        expect(finalState.clubs.tag).toBe("Idle");
    });
});
