import { FunctionalComponent, h, createContext } from "preact";
import { Route, Router } from "preact-router";
import { useGoogleLogin } from "react-use-googlelogin";
import { useContext, useState, StateUpdater } from "preact/hooks";

import Home from "../routes/home";
import Profile from "../routes/profile";
import Club from "../routes/club";
import NotFoundPage from "../routes/notfound";
import Events from "../routes/events";
import Header from "./header";
import { GoogleAuth, Access } from "../types/models";

import "rbx/index.css";

// eslint-disable-next-line @typescript-eslint/no-explicit-any
// eslint-disable-next-line @typescript-eslint/no-unsafe-member-access
if ((module as any).hot) {
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
        clientId: process.env.PREACT_APP_GOOGLE_CLIENT_ID as string, // Your clientID from Google.
    });
    const access = useState<Access>(defaultAccess);

    return (
        <div id="app">
            <AccessContext.Provider value={access}>
                <GoogleAuthContext.Provider value={googleAuth as GoogleAuth}>
                    <Header />
                    <Router>
                        <Route path="/" component={Home} />
                        <Route path="/profile/" component={Profile} user="me" />
                        <Route path="/profile/:user" component={Profile} />
                        <Route path="/clubs/" component={Club} />
                        <Route path="/events/" component={Events} />
                        <NotFoundPage default />
                    </Router>
                </GoogleAuthContext.Provider>
            </AccessContext.Provider>
        </div>
    );
};

export default App;

export const useGoogleAuth = () => useContext(GoogleAuthContext);
export const useAccess = () => useContext(AccessContext);
