const path = require("node:path");
const { FlatCompat } = require("@eslint/eslintrc");
const js = require("@eslint/js");

const compat = new FlatCompat({
  baseDirectory: __dirname,
  recommendedConfig: js.configs.recommended,
  allConfig: js.configs.all,
});

module.exports = [
  {
    ignores: ["build/**"],
  },
  ...compat.config({
    env: {
      browser: true,
    },
    plugins: ["@typescript-eslint", "ts-immutable"],
    extends: [
      "eslint:recommended",
      "plugin:@typescript-eslint/eslint-recommended",
      "plugin:@typescript-eslint/recommended",
      "plugin:@typescript-eslint/recommended-requiring-type-checking",
      "plugin:react/recommended",
      "plugin:react-hooks/recommended",
      "plugin:prettier/recommended",
      "plugin:ts-immutable/recommended",
    ],
    parser: "@typescript-eslint/parser",
    parserOptions: {
      ecmaFeatures: {
        jsx: true,
      },
      project: "./tsconfig.eslint.json",
      tsconfigRootDir: path.resolve(__dirname),
    },
    rules: {
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
      "react-hooks/immutability": "off",
    },
    settings: {
      react: {
        pragma: "h",
        version: "detect",
      },
    },
    overrides: [
      {
        files: ["*.js"],
        rules: {
          "@typescript-eslint/explicit-function-return-type": "off",
        },
      },
    ],
  }),
];
