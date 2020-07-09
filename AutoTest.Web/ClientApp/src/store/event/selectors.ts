import { AppState } from "..";
import { TestRunUploadState } from "../../types/models";

export const selectRequiresSync = (a: AppState) =>
    a.event.testRuns.filter((r) => r.state !== TestRunUploadState.Uploaded)
        .length > 0;

export const selectEntrants = (a: AppState) => a.event.entrants;

export const selectClassOptions = (a: AppState): readonly string[] =>
    a.event.entrants.tag === "Loaded"
        ? Array.from(new Set(a.event.entrants.value.map((a) => a.class).sort()))
        : [];

export const selectTests = (a: AppState) => a.event.tests;
