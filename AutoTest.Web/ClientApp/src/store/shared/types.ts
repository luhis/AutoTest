export const CLEAR_CACHE = "CLEAR_CACHE";

interface ClearCache {
  readonly type: typeof CLEAR_CACHE;
}

export type SharedActionTypes = ClearCache;
