import path from "node:path";
import { fileURLToPath } from "node:url";
import js from "@eslint/js";
import tseslint from "typescript-eslint";
import reactPluginRaw from "eslint-plugin-react";
import reactHooksPluginRaw from "eslint-plugin-react-hooks";
import functional from "eslint-plugin-functional";
import eslintPluginPrettierRecommended from "eslint-plugin-prettier/recommended";
import { fixupPluginRules } from "@eslint/compat";

const __dirname = path.dirname(fileURLToPath(import.meta.url));

const reactPlugin = fixupPluginRules(reactPluginRaw);
const reactHooksPlugin = fixupPluginRules(reactHooksPluginRaw);

export default tseslint.config(
  {
    ignores: ["build/**", "src/integrationTests/**"],
  },
  js.configs.recommended,
  ...tseslint.configs.recommendedTypeChecked,
  functional.configs.lite,
  {
    plugins: {
      react: reactPlugin,
      "react-hooks": reactHooksPlugin,
    },
    files: ["**/*.{ts,tsx,js,jsx}"],
    languageOptions: {
      globals: {
        browser: true,
      },
      parserOptions: {
        projectService: {
          allowDefaultProject: [
            "src/sw.js",
            "src/tests/__mocks__/*.js",
          ],
        },
        tsconfigRootDir: __dirname,
      },
    },
    settings: {
      react: {
        pragma: "h",
        version: "detect",
      },
    },
    rules: {
      ...reactPluginRaw.configs.recommended.rules,
      ...reactHooksPluginRaw.configs.recommended.rules,
      "functional/no-return-void": "off",
      "functional/no-mixed-types": "off",
      "functional/functional-parameters": "off",
      "react/no-unknown-property": ["error", { ignore: ["class"] }],
      "react/prop-types": ["off"],
      "@typescript-eslint/explicit-function-return-type": "off",
      "@typescript-eslint/no-unused-vars": [
        "error",
        { argsIgnorePattern: "^_", varsIgnorePattern: "^_" },
      ],
      "@typescript-eslint/no-require-imports": "off",
      "@typescript-eslint/no-base-to-string": "off",
      "@typescript-eslint/explicit-module-boundary-types": "off",
      "@typescript-eslint/no-shadow": ["error"],
      "@typescript-eslint/switch-exhaustiveness-check": "error",
      "@typescript-eslint/no-unnecessary-condition": "error",
      "no-duplicate-imports": "error",
      "no-shadow": "off",
      "no-implicit-globals": "error",
      eqeqeq: "error",
    },
  },
  {
    files: ["**/*.js"],
    extends: [js.configs.recommended],
    languageOptions: {
      globals: {
        self: "readonly",
        caches: "readonly",
        fetch: "readonly",
      },
    },
    rules: {
      "@typescript-eslint/explicit-function-return-type": "off",
    },
  },
  eslintPluginPrettierRecommended,
);
