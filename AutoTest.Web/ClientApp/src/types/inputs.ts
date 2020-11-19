export interface OnChange {
    readonly target: HTMLInputElement;
}

export interface OnSelectChange {
    readonly target: { readonly value: string };
}

export interface OnChangeCheck {
    readonly target: { readonly checked: boolean };
}
