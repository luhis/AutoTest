import { FormSubmit } from "src/types/inputs";

export const addPreventDefault = (save: () => void) => (e: FormSubmit) => {
    e.preventDefault();
    save();
};
