import { GET_ENTRANTS, ADD_TEST_RUN } from "./types";
import { TestRun } from "src/types/models";

export const GetEntrants = () => ({
    type: GET_ENTRANTS,
});

export const AddTestRun = (testRun: TestRun) => ({
    type: ADD_TEST_RUN,
    payload: testRun,
});
