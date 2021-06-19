import { h } from "preact";

export const addPreventDefault =
    (save: () => void, setSavingStatus: (saving: boolean) => void) =>
    (e: h.JSX.TargetedEvent<HTMLFormElement>) => {
        setSavingStatus(true);
        e.preventDefault();
        save();
        setSavingStatus(false);
    };
