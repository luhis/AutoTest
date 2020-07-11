export type ApiResponse<T> =
    | {
          readonly tag: "Loaded";
          readonly id: number | undefined;
          readonly value: T;
      }
    | { readonly tag: "Error"; readonly value: string };

export type LoadingState<T> =
    | ApiResponse<T>
    | { tag: "Loading"; readonly id: number | undefined }
    | { tag: "Idle" };

export const toApiResponse = async <T>(
    f: () => Promise<T>,
    id?: number
): Promise<ApiResponse<T>> => {
    try {
        return { tag: "Loaded", value: await f(), id };
    } catch (e) {
        return { tag: "Error", value: "API error" };
    }
};

export const requiresLoading = <T>(tag: LoadingState<T>["tag"]) => {
    if (tag === "Error" || tag === "Idle") {
        return true;
    }
    return false;
};

export const idsMatch = <T>(loading: LoadingState<T>, id: number) => {
    if (loading.tag === "Loading" || loading.tag === "Loaded") {
        return loading.tag ? loading.id === id : true;
    } else {
        return true;
    }
};
