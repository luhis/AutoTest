import { h } from "preact";

export const addPreventDefault =
    (save: () => void) => (e: h.JSX.TargetedEvent<HTMLFormElement>) => {
        e.preventDefault();
        save();
    };
