export const startsWithIgnoreCase = (haystack: string, needle: string) =>
  haystack.toUpperCase().startsWith(needle.toUpperCase());
