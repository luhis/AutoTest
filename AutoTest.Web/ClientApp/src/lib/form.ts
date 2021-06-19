import { h } from "preact";

export const addPreventDefault =
    (save: () => Promise<void>, setSavingStatus: (saving: boolean) => void) =>
    async (e: h.JSX.TargetedEvent<HTMLFormElement>) => {
        setSavingStatus(true);
        e.preventDefault();
        await save();
        setSavingStatus(false);
    };
