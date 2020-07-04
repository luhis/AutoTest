import { createStore, combineReducers, applyMiddleware } from "redux";
import thunk from "redux-thunk";
import { composeWithDevTools } from "redux-devtools-extension";

import { eventReducer } from "./event/reducers";

const rootReducer = combineReducers({
    event: eventReducer,
});

export type AppState = ReturnType<typeof rootReducer>;
const store = createStore(
    rootReducer,
    composeWithDevTools(applyMiddleware(thunk))
);

export default store;
