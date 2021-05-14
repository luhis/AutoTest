import { AppState } from "..";
import { MakeAndModel, TestRunUploadState } from "../../types/models";
import { distinct } from "../../lib/array";
import { mapOrDefault } from "../../types/loadingState";

export const selectRequiresSync = (a: AppState) =>
    a.event.testRuns.filter((r) => r.state !== TestRunUploadState.Uploaded)
        .length;

export const selectEntrants = (a: AppState) => a.event.entrants;

export const selectClassOptions = (state: AppState): readonly string[] =>
    mapOrDefault(
        selectEntrants(state),
        (a) => distinct(a.map((entrant) => entrant.class).sort()),
        []
    );

export const selectMakeModelOptions = (
    state: AppState
): readonly MakeAndModel[] =>
    mapOrDefault(
        selectEntrants(state),
        (a) =>
            distinct(
                a
                    .map(({ vehicle }) => ({
                        make: vehicle.make,
                        model: vehicle.model,
                    }))
                    .sort()
            ),
        []
    );

export const selectTestRuns = (a: AppState) => a.event.testRuns;

export const selectEvents = (a: AppState) => a.event.events;

export const selectClubs = (a: AppState) => a.event.clubs;

export const selectNotifications = (a: AppState) => a.event.notifications;

export const selectTestRunsFromServer = (a: AppState) =>
    a.event.testRunsFromServer;
