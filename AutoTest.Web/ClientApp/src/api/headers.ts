export const getHeaders = (token: string | undefined) => ({
    ...getBearerHeader(token),
    "Content-Type": "application/json",
});

export const getBearerHeader = (token: string | undefined) => ({
    Authorization: token ? `Bearer ${token}` : "",
});
