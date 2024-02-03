import { newValidDate } from "ts-date";

import { TestRunUploadState } from "../../types/models";
import { runReducer } from "../../store/runs/reducers";
import { RunState } from "../../store/runs/types";
import { ClearCache } from "../../store/event/actions";

const populatedState: RunState = {
  testRuns: [
    {
      created: newValidDate(),
      entrantId: 1,
      eventId: 1,
      ordinal: 1,
      penalties: [],
      state: TestRunUploadState.NotSent,
      testRunId: 1,
      timeInMS: 1000,
    },
  ],
  testRunsFromServer: { tag: "Error", value: "Fail" },
};

describe("Run Reducer", () => {
  test("Clear Cache", () => {
    const finalState = runReducer(populatedState, ClearCache());
    expect(finalState.testRunsFromServer.tag).toBe("Idle");
    expect(finalState.testRuns.length).toBe(1);
  });

  test("Update test run state", () => {
    const stateWithEntrant: RunState = {
      ...populatedState,
      testRuns: [
        {
          state: TestRunUploadState.NotSent,
          timeInMS: 10000,
          created: newValidDate(),
          entrantId: 1,
          eventId: 1,
          testRunId: 2,
          ordinal: 1,
          penalties: [],
        },
      ],
    };
    const finalState = runReducer(stateWithEntrant, {
      type: "UPDATE_TEST_RUN_STATE",
      payload: { testRunId: 2, state: TestRunUploadState.Uploaded },
    });
    expect(finalState.testRuns[0].state).toBe(TestRunUploadState.Uploaded);
  });
});
