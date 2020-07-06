export type ApiResponse<T> =
    | { readonly tag: "Loaded"; readonly value: T }
    | { readonly tag: "Error"; readonly value: string };

export type LoadingState<T> =
    | ApiResponse<T>
    | { tag: "Loading" }
    | { tag: "Idle" };

export const toApiResponse = async <T>(
    f: () => Promise<T>
): Promise<ApiResponse<T>> => {
    try {
        return { tag: "Loaded", value: await f() };
    } catch (e) {
        return { tag: "Error", value: "API error" };
    }
};

export const requiresLoading = <T>(state: LoadingState<T>) => {
    if (state.tag === "Error" || state.tag === "Idle") {
        return true;
    }
    return false;
};
