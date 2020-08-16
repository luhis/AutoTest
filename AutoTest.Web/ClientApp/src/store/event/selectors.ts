import { AppState } from "..";
import { TestRunUploadState } from "../../types/models";
import { distinct } from "../../lib/array";

export const selectRequiresSync = (a: AppState) =>
    a.event.testRuns.filter((r) => r.state !== TestRunUploadState.Uploaded)
        .length > 0;

export const selectEntrants = (a: AppState) => a.event.entrants;

export const selectClassOptions = (state: AppState): readonly string[] =>
    state.event.entrants.tag === "Loaded"
        ? distinct(state.event.entrants.value.map((a) => a.class).sort())
        : [];

export const selectTestRuns = (a: AppState) => a.event.testRuns;

export const selectEvents = (a: AppState) => a.event.events;

export const selectClubs = (a: AppState) => a.event.clubs;
