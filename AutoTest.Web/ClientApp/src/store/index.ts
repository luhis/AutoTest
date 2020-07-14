import { createStore, combineReducers, applyMiddleware } from "redux";
import thunk from "redux-thunk";
import { composeWithDevTools } from "redux-devtools-extension";
import storage from "redux-persist/lib/storage";
import { persistStore, persistReducer } from "redux-persist";

import { eventReducer } from "./event/reducers";

const persistConfig = {
    key: "root",
    storage,
};

const rootReducer = combineReducers({
    event: eventReducer,
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
