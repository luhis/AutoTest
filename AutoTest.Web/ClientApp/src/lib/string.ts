export const startCase = (s: string) =>
    s.replace(/([A-Z]+)*([A-Z][a-z])/g, "$1 $2");
