import { ValidDate, addMinutes, newValidDate } from "ts-date";

export const isStale = <T, TT>(response: LoadingState<T, TT>) =>
    response.tag === "Loaded"
        ? response.loaded < addMinutes(newValidDate(), -5)
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
    id: TT
): Promise<ApiResponse<T, TT>> => {
    try {
        return {
            tag: "Loaded",
            value: await f(),
            id,
            loaded: newValidDate(),
        };
    } catch (e) {
        return { tag: "Error", value: "API error" };
    }
};

export const requiresLoading = <T, TT>(tag: LoadingState<T, TT>["tag"]) => {
    if (tag === "Error" || tag === "Idle") {
        return true;
    }
    return false;
};

export const idsMatch = <T, TT extends unknown>(
    loading: LoadingState<T, TT>,
    id: TT
) => {
    if (loading.tag === "Loading" || loading.tag === "Loaded") {
        return loading.tag ? loading.id === id : true;
    } else {
        return true;
    }
};

export const findIfLoaded = <T, TT>(
    loading: LoadingState<readonly T[], TT>,
    find: (t: T) => boolean
) => {
    if (loading.tag === "Loaded") {
        return loading.value.find(find);
    } else {
        return undefined;
    }
};

export const ifLoaded = <T, TT>(
    entrants: LoadingState<T, TT>,
    f: (_: T) => T
) => {
    if (entrants.tag === "Loaded") {
        return {
            ...entrants,
            value: f(entrants.value),
        };
    } else {
        return entrants;
    }
};

export const mapOrDefault = <T, TT, TTT>(
    entrants: LoadingState<T, TT>,
    f: (_: T) => TTT,
    defaultValue: TTT
) => {
    if (entrants.tag === "Loaded") {
        return f(entrants.value);
    } else {
        return defaultValue;
    }
};
