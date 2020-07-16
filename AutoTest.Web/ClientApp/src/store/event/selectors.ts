import { AppState } from "..";
import { TestRunUploadState } from "../../types/models";

export const selectRequiresSync = (a: AppState) =>
    a.event.testRuns.filter((r) => r.state !== TestRunUploadState.Uploaded)
        .length > 0;

export const selectEntrants = (a: AppState) => a.event.entrants;

export const selectClassOptions = (state: AppState): readonly string[] =>
    state.event.entrants.tag === "Loaded"
        ? Array.from(
              new Set(state.event.entrants.value.map((a) => a.class).sort())
          )
        : [];

export const selectTests = (a: AppState) => a.event.tests;

export const selectTestRuns = (a: AppState) => a.event.testRuns;

export const selectEvents = (a: AppState) => a.event.events;

export const selectClubs = (a: AppState) => a.event.clubs;
