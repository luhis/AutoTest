module.exports = {
    preset: "jest-preset-preact",
    setupFiles: [
        "<rootDir>/src/tests/__mocks__/setupTests.js",
        "<rootDir>/src/tests/__mocks__/browserMocks.js"
    ],
    testEnvironmentOptions: {
        url: "http://localhost:8080",
    },
    moduleNameMapper: {
        "\\.(jpg|jpeg|png|gif|eot|otf|webp|svg|ttf|woff|woff2|mp4|webm|wav|mp3|m4a|aac|oga)$": "<rootDir>/src/tests/__mocks__/fileMocks.js",
    },
    transformIgnorePatterns: ['../../node_modules/(?!${react-bulma-components})']
}

const dotenv = require('dotenv');
dotenv.config({path: './.env.test'});