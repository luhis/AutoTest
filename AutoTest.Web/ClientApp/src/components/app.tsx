import { FunctionalComponent, h, createContext } from "preact";
import { Route, Router } from "preact-router";
import { useGoogleLogin } from "react-use-googlelogin";
import { useContext } from "preact/hooks";
import { Provider } from "react-redux";
import { Container, Loader } from "rbx";
import { PersistGate } from "redux-persist/integration/react";
import { HookReturnValue } from "react-use-googlelogin/dist/types";

import Home from "../routes/home";
import Profile from "../routes/profile";
import Club from "../routes/clubs";
import NotFoundPage from "../routes/notfound";
import Events from "../routes/events";
import Event from "../routes/event";
import Entrant from "../routes/entrants";
import Header from "./header";
import Results from "../routes/results";
import Tests from "../routes/tests";
import Marshal from "../routes/marshal";
import store from "../store";
import { googleKey } from "../settings";

import "rbx/index.css";

interface Module {
    readonly hot: unknown | undefined;
}

if ((module as Module).hot) {
    require("preact/debug");
}

const GoogleAuthContext = createContext<HookReturnValue>({
    signIn: () => {
        throw new Error();
    },
    signOut: () => {
        throw new Error();
    },
    googleUser: undefined,
    refreshUser: () => {
        throw new Error();
    },
    isSignedIn: false,
    isInitialized: false,
    grantOfflineAccess: () => {
        throw new Error();
    },
}); // Not necessary, but recommended.

const App: FunctionalComponent = () => {
    const googleAuth = useGoogleLogin({
        clientId: googleKey, // Your clientID from Google.
    });

    const { appStore, persistor } = store();
    return (
        <div id="app">
            <Provider store={appStore}>
                <PersistGate loading={<Loader />} persistor={persistor}>
                    <GoogleAuthContext.Provider value={googleAuth}>
                        <Header />
                        <Container fluid>
                            <Router>
                                <Route path="/" component={Home} />
                                <Route path="/profile/" component={Profile} />
                                <Route path="/clubs/" component={Club} />
                                <Route path="/events/" component={Events} />
                                <Route
                                    path="/event/:eventId"
                                    component={Event}
                                />
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
                                    path="/marshal/:eventId/:ordinal"
                                    component={Marshal}
                                />
                                <NotFoundPage default />
                            </Router>
                        </Container>
                    </GoogleAuthContext.Provider>
                </PersistGate>
            </Provider>
        </div>
    );
};

export default App;

export const useGoogleAuth = () => useContext(GoogleAuthContext);
