import { createStore, combineReducers, applyMiddleware } from "redux";
import thunk from "redux-thunk";
import { composeWithDevTools } from "redux-devtools-extension";
import storage from "redux-persist/lib/storage";
import { persistStore, persistReducer, createTransform } from "redux-persist";
import { parseIsoOrThrow } from "ts-date";

import { eventReducer } from "./event/reducers";
import { profileReducer } from "./profile/reducers";

const persistConfig = {
    key: "root",
    storage,
    transforms: [
        createTransform(JSON.stringify, (toRehydrate) =>
            // eslint-disable-next-line @typescript-eslint/no-unsafe-return
            JSON.parse(toRehydrate, (_, value) =>
                // eslint-disable-next-line @typescript-eslint/no-unsafe-return
                typeof value === "string" &&
                /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}/.exec(value)
                    ? parseIsoOrThrow(value)
                    : value
            )
        ) as never,
    ],
};

const rootReducer = combineReducers({
    event: eventReducer,
    profile: profileReducer,
});

const persistedReducer = persistReducer(persistConfig, rootReducer);

export type AppState = ReturnType<typeof persistedReducer>;

export default () => {
    const storeX = createStore(
        persistedReducer,
        composeWithDevTools(applyMiddleware(thunk))
    );
    const persistor = persistStore(storeX as any);
    return { storeX, persistor };
};
