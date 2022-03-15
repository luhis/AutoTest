module.exports = {
    env: {
        browser: true
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
        "plugin:ts-immutable/recommended"
    ],
    parser: "@typescript-eslint/parser",
    parserOptions: {
        ecmaFeatures: {
            jsx: true
        },
        project: "./tsconfig.eslint.json",
    },
    rules: {
        "react/no-unknown-property": ["error", { ignore: ["class"] }],
        "react/prop-types": ["off"],
        "@typescript-eslint/explicit-function-return-type": "off",
        "@typescript-eslint/no-unused-vars": ["error", { "argsIgnorePattern": "^_", "varsIgnorePattern": "^_" }],
        "@typescript-eslint/explicit-module-boundary-types": "off",
        "no-duplicate-imports": "error",
        "no-shadow": "off",
        "@typescript-eslint/no-shadow": ["error"],
        "@typescript-eslint/switch-exhaustiveness-check": "error",
        "no-implicit-globals": "error",
        "linebreak-style": ["error", "unix"]
    },
    settings: {
        react: {
            pragma: "h",
            version: "detect"
        },
    },
    overrides: [
        {
            files: ["*.js"],
            rules: {
                "@typescript-eslint/explicit-function-return-type": "off",
            }
        }
    ]
};
