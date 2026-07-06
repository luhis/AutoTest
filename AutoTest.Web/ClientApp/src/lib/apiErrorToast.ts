import { toast } from "bulma-toast";

export const showError = (error: unknown) => {
  const message =
    error instanceof Error
      ? error.message
      : typeof error === "string"
        ? error
        : "An unknown error occurred";
  toast({ message, type: "is-danger" });
};
