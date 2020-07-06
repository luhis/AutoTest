import { AppState } from "..";
import { TestRunUploadState } from "../../types/models";

export const requiresSync = (a: AppState) =>
    a.event.testRuns.filter((r) => r.state !== TestRunUploadState.Uploaded)
        .length > 0;

export const selectEntrants = (a: AppState) => a.event.entrants;

export const selectTests = (a: AppState) => a.event.tests;
