import { FunctionalComponent, h, createContext } from "preact";
import { Route, Router } from "preact-router";
import { useGoogleLogin } from "react-use-googlelogin";
import { useContext, useState, StateUpdater } from "preact/hooks";
import { Provider } from "react-redux";
import { Content, Loader } from "rbx";
import { PersistGate } from "redux-persist/integration/react";

import Home from "../routes/home";
import Profile from "../routes/profile";
import Club from "../routes/clubs";
import NotFoundPage from "../routes/notfound";
import Events from "../routes/events";
import Entrant from "../routes/entrants";
import Header from "./header";
import { GoogleAuth, Access } from "../types/models";
import Results from "../routes/results";
import Tests from "../routes/tests";
import Marshal from "../routes/marshal";
import store from "../store";
import { googleKey } from "../settings";

import "rbx/index.css";

interface Module {
    hot: unknown | undefined;
}

if ((module as Module).hot) {
    require("preact/debug");
}

const GoogleAuthContext = createContext<GoogleAuth>({
    signIn: () => {
        throw new Error();
    },
    googleUser: null,
}); // Not necessary, but recommended.

const defaultAccess: Access = {
    canViewClubs: false,
};

const AccessContext = createContext<[Access, StateUpdater<Access>]>([
    defaultAccess,
    (_) => undefined,
]);

const App: FunctionalComponent = () => {
    const googleAuth = useGoogleLogin({
        clientId: googleKey, // Your clientID from Google.
    });
    const access = useState<Access>(defaultAccess);

    const { storeX, persistor } = store();
    return (
        <div id="app">
            <Provider store={storeX}>
                <PersistGate loading={<Loader />} persistor={persistor}>
                    <AccessContext.Provider value={access}>
                        <GoogleAuthContext.Provider
                            value={googleAuth as GoogleAuth}
                        >
                            <Content>
                                <Header />
                                <Router>
                                    <Route path="/" component={Home} />
                                    <Route
                                        path="/profile/"
                                        component={Profile}
                                        user="me"
                                    />
                                    <Route
                                        path="/profile/:user"
                                        component={Profile}
                                    />
                                    <Route path="/clubs/" component={Club} />
                                    <Route path="/events/" component={Events} />
                                    <Route
                                        path="/entrants/:eventId"
                                        component={Entrant}
                                    />
                                    <Route
                                        path="/results/:eventId"
                                        component={Results}
                                    />
                                    <Route
                                        path="/tests/:eventId"
                                        component={Tests}
                                    />
                                    <Route
                                        path="/marshal/:eventId/:testId"
                                        component={Marshal}
                                    />
                                    <NotFoundPage default />
                                </Router>
                            </Content>
                        </GoogleAuthContext.Provider>
                    </AccessContext.Provider>
                </PersistGate>
            </Provider>
        </div>
    );
};

export default App;

export const useGoogleAuth = () => useContext(GoogleAuthContext);
export const useAccess = () => useContext(AccessContext);
