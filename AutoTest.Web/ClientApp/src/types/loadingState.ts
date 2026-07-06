import { isEqual } from "@s-libs/micro-dash";
import { ValidDate, addMinutes, newValidDate } from "ts-date";

const staleDataMinutes = 0.1;

export const isStale = <T, TT>(response: LoadingState<T, TT>) =>
  response.tag === "Loaded"
    ? response.loaded < addMinutes(newValidDate(), -1 * staleDataMinutes)
    : false;

interface Loaded<TValue> {
  readonly tag: "Loaded";
  readonly loaded: ValidDate;
  readonly value: TValue;
}

interface Id<TId> {
  readonly id: TId;
}

export type ApiResponse<TValue, TId = undefined> =
  | (Loaded<TValue> & Id<TId>)
  | { readonly tag: "Error"; readonly value: string };

export type LoadingState<TValue, TId = undefined> =
  | ApiResponse<TValue, TId>
  | ({ readonly tag: "Loading" } & Id<TId>)
  | { readonly tag: "Idle" };

export const toApiResponse = async <T, TT>(
  f: () => Promise<T>,
  id: TT,
): Promise<ApiResponse<T, TT>> => {
  try {
    return {
      tag: "Loaded",
      value: await f(),
      id,
      loaded: newValidDate(),
    };
  } catch (e) {
    const message =
      e instanceof Error
        ? e.message
        : typeof e === "string"
          ? e
          : "An unknown error occurred";
    return { tag: "Error", value: message };
  }
};

export const requiresLoading = <T, TT>(tag: LoadingState<T, TT>["tag"]) => {
  if (tag === "Error" || tag === "Idle") {
    return true;
  }
  return false;
};

export const idsMatch = <T, TT>(loading: LoadingState<T, TT>, id: TT) => {
  if (loading.tag === "Loading" || loading.tag === "Loaded") {
    return isEqual(loading.id, id);
  } else {
    return true;
  }
};

export const findIfLoaded = <T, TT>(
  loading: LoadingState<readonly T[], TT>,
  find: (t: T) => boolean,
) => {
  if (loading.tag === "Loaded") {
    return loading.value.find(find);
  } else {
    return undefined;
  }
};

export const ifLoaded = <T, TT>(
  state: LoadingState<T, TT>,
  f: (_: T) => T,
) => {
  if (state.tag === "Loaded") {
    return {
      ...state,
      value: f(state.value),
    };
  } else {
    return state;
  }
};

export const mapOrDefault = <T, TT, TTT>(
  state: LoadingState<T, TT>,
  f: (_: T) => TTT,
  defaultValue: TTT,
) => {
  if (state.tag === "Loaded") {
    return f(state.value);
  } else {
    return defaultValue;
  }
};

const statesAllowingErrorResult = ["Idle", "Error", "Loading"];

export const canUpdate = <T, TT>(
  oldState: LoadingState<T, TT>,
  newState: LoadingState<T, TT>,
) =>
  statesAllowingErrorResult.includes(oldState.tag) || newState.tag === "Loaded";
