import { h } from "preact";

export const addPreventDefault =
  (save: () => Promise<void>, setSavingStatus: (saving: boolean) => void) =>
  (e: h.JSX.TargetedEvent<HTMLFormElement>) => {
    const f = async () => {
      setSavingStatus(true);
      e.preventDefault();
      await save();
      setSavingStatus(false);
    };
    void f();
  };

export const toggleValue = <T>(values: readonly T[], valueToToggle: T) =>
  values.includes(valueToToggle)
    ? values.filter((a) => a !== valueToToggle)
    : values.concat(valueToToggle);
