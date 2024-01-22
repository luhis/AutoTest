export const distinct = <T>(arr: readonly T[]): readonly T[] =>
  Array.from(new Set(arr));
