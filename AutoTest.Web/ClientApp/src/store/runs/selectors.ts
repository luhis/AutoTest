import { AppState } from "..";
import { TestRunUploadState } from "../../types/models";

export const selectRequiresSync = (a: AppState) =>
  a.runs.testRuns.filter((r) => r.state !== TestRunUploadState.Uploaded).length;

export const selectTestRuns = (a: AppState) => a.runs.testRuns;

export const selectTestRunsFromServer = (a: AppState) =>
  a.runs.testRunsFromServer;
