import { AppState } from "..";
import { MakeAndModel, Event } from "../../types/models";
import { distinct } from "../../lib/array";
import { mapOrDefault } from "../../types/loadingState";

export const selectEntrants = (a: AppState) => a.event.entrants;
export const selectMarshals = (a: AppState) => a.event.marshals;
export const selectAllRoles = (state: AppState): readonly string[] =>
  mapOrDefault(
    selectMarshals(state),
    (a) => distinct(a.map((entrant) => entrant.role).sort()),
    [],
  );

export const selectLicenseTypeOptions = (_: AppState): readonly string[] => [
  "Clubman",
];

export const selectClubOptions = (state: AppState): readonly string[] =>
  mapOrDefault(
    selectEntrants(state),
    (a) => distinct(a.map((entrant) => entrant.club).sort()),
    [],
  );

export const selectClassOptions = (state: AppState): readonly string[] =>
  mapOrDefault(
    selectEntrants(state),
    (a) => distinct(a.map((entrant) => entrant.class).sort()),
    [],
  );

export const selectMakeModelOptions = (
  state: AppState,
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
          .sort(),
      ),
    [],
  );

export const selectEvents = (a: AppState) => a.event.events;

export const selectNotifications = (a: AppState) => a.event.notifications;

export const get10LatestEvents = (arr: readonly Event[]) => {
  // eslint-disable-next-line @typescript-eslint/no-unsafe-call, @typescript-eslint/no-unsafe-assignment
  const sorted: readonly Event[] = arr.toSorted(
    (x: Event, y: Event) => y.created.getTime() - x.created.getTime(),
  );
  return sorted.slice(0, 10);
};
