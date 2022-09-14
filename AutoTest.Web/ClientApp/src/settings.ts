export const keySeed = Number.parseInt(
    process.env.PREACT_APP_KEY_SEED as string
);

export const googleKey = process.env.PREACT_APP_GOOGLE_CLIENT_ID as string;

export const appInsightsKey = process.env.PREACT_APP_APP_INSIGHTS as string;

export const MarshalAgreement =
    "https://www.motorsportuk.org/wp-content/uploads/2022/02/2022-03-01-signing-on-declaration-officials-pdf.pdf";

export const EntrantAgreement =
    "https://www.motorsportuk.org/wp-content/uploads/2022/02/2022-03-01-signing-on-declaration-competitor-table.pdf";
