import {
  createStore,
  combineReducers,
  applyMiddleware,
  Store,
  AnyAction,
} from "redux";
import thunk, { ThunkDispatch } from "redux-thunk";
import { composeWithDevTools } from "@redux-devtools/extension";
import storage from "redux-persist/lib/storage";
import { persistStore, persistReducer, createTransform } from "redux-persist";
import { parseIsoOrThrow } from "ts-date";
import { useDispatch } from "react-redux";

import { eventReducer } from "./event/reducers";
import { profileReducer } from "./profile/reducers";
import { clubsReducer } from "./clubs/reducers";
import { EventActionTypes } from "./event/types";
import { ClubsActionTypes } from "./clubs/types";
import { ProfileActionTypes } from "./profile/types";
import { runReducer } from "./runs/reducers";
import { RunActionTypes } from "./runs/types";

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
          : value,
      ),
    ) as never,
  ],
  blacklist: ["profile"],
};

export const rootReducer = combineReducers({
  event: eventReducer,
  profile: profileReducer,
  clubs: clubsReducer,
  runs: runReducer,
});

const persistedReducer = persistReducer(persistConfig, rootReducer);

export type AppState = ReturnType<typeof persistedReducer>;

export default () => {
  const appStore: Store<AppState, AnyAction> = createStore(
    persistedReducer,
    composeWithDevTools(applyMiddleware(thunk)),
  );
  const persistor = persistStore(appStore);
  return { appStore, persistor };
};

type AppActionTypes =
  | EventActionTypes
  | ClubsActionTypes
  | ProfileActionTypes
  | RunActionTypes;

export const useThunkDispatch = () =>
  useDispatch<ThunkDispatch<AppState, unknown, AppActionTypes>>();
