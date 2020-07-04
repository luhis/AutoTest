export interface OnChange {
    readonly target: HTMLInputElement;
}

export interface OnSelectChange {
    readonly target: { value: string };
}

export interface OnChangeCheck {
    readonly target: { checked: boolean };
}
