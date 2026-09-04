declare module "*.css";

interface ImportMeta {
  readonly env: {
    readonly VITE_APP_GOOGLE_CLIENT_ID: string;
    readonly VITE_APP_KEY_SEED: string;
    readonly VITE_APP_APP_INSIGHTS: string;
  };
}
