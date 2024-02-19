import { toast } from "bulma-toast";

export const showError = (error: unknown) => {
  toast({ message: (error as Error).message, type: "is-danger" });
};
