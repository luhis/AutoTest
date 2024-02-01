import { profileReducer } from "../store/profile/reducers";
import { ProfileState } from "../store/profile/types";

const populatedState: ProfileState = {
  profile: { tag: "Idle" },
  access: {
    adminClubs: [1, 2, 3],
    canViewClubs: true,
    canViewProfile: true,
    editableEntrants: [4, 5, 6],
    editableMarshals: [7, 8, 9],
    isLoggedIn: true,
    isRootAdmin: true,
    marshalEvents: [10, 11, 12],
  },
};

describe("Profile Reducer", () => {
  test("Reset Access", () => {
    const finalState = profileReducer(populatedState, {
      type: "RESET_ACCESS",
    });
    expect(finalState.access.isLoggedIn).toBe(false);
    expect(finalState.access.isRootAdmin).toBe(false);
  });
});
