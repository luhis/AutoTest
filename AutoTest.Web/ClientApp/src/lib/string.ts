export const startCase = (s: string) =>
    s.replace(/([A-Z]+)*([A-Z][a-z])/g, "$1 $2").trim();

export const startsWithIgnoreCase = (haystack: string, needle: string) =>
    haystack.toUpperCase().startsWith(needle.toUpperCase());
