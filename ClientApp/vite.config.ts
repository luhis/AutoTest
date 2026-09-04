import { resolve } from "path";
import { defineConfig } from "vite";
import preact from "@preact/preset-vite";

export default defineConfig({
  plugins: [preact()],
  build: {
    outDir: "build",
  },
  resolve: {
    alias: {
      react: resolve(__dirname, "node_modules/preact/compat"),
      "react-dom": resolve(__dirname, "node_modules/preact/compat"),
    },
  },
  define: {
    __BUILD_DATE__: JSON.stringify(new Date().toISOString()),
  },
  server: {
    port: 8080,
    strictPort: true,
    proxy: {
      "/api": "https://localhost:7212",
      "/hubs": {
        target: "https://localhost:7212",
        ws: true,
      },
    },
  },
});
