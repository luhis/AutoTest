import { FunctionComponent, h } from "preact";
import { Route, Router } from "preact-router";
import { Provider } from "react-redux";
import { Container, Loader } from "react-bulma-components";
import { PersistGate } from "redux-persist/integration/react";
import { GoogleOAuthProvider } from "@react-oauth/google";
import { reactAI } from "react-appinsights";
import {
  ApplicationInsights,
  ITelemetryPlugin,
} from "@microsoft/applicationinsights-web";

import Home from "../routes/home";
import Profile from "../routes/profile";
import Club from "../routes/clubs";
import NotFoundPage from "../routes/notfound";
import Events from "../routes/events";
import Event from "../routes/event";
import Entrant from "../routes/entrants";
import Marshals from "../routes/marshals";
import Header from "./header";
import Results from "../routes/results";
import Tests from "../routes/tests";
import Marshal from "../routes/marshal";
import LiveRuns from "../routes/liveRuns";
import EditRuns from "../routes/editRuns";
import store from "../store";
import { appInsightsKey, googleKey } from "../settings";

import "bulma/css/bulma.css";

interface Module {
  readonly hot: boolean | undefined;
}

if ((module as Module).hot) {
  require("preact/debug");
}

if (typeof window !== "undefined") {
  const appInsights = new ApplicationInsights({
    config: {
      instrumentationKey: appInsightsKey,
      extensions: [reactAI as ITelemetryPlugin],
      extensionConfig: {
        [reactAI.extensionId]: { debug: false },
      },
    },
  });
  appInsights.loadAppInsights();
}

const App: FunctionComponent = () => {
  const { appStore, persistor } = store();
  return (
    <div id="app">
      <Provider store={appStore}>
        <PersistGate loading={<Loader />} persistor={persistor}>
          <GoogleOAuthProvider clientId={googleKey}>
            <Header />
            <Container fluid>
              <Router>
                <Route path="/" component={Home} />
                <Route path="/profile/" component={Profile} />
                <Route path="/clubs/" component={Club} />
                <Route path="/events/" component={Events} />
                <Route path="/event/:eventId" component={Event} />
                <Route path="/entrants/:eventId" component={Entrant} />
                <Route path="/marshals/:eventId" component={Marshals} />
                <Route path="/results/:eventId" component={Results} />
                <Route path="/tests/:eventId" component={Tests} />
                <Route path="/marshal/:eventId/:ordinal" component={Marshal} />
                <Route path="/liveRuns/:eventId/" component={LiveRuns} />
                <Route path="/editRuns/:eventId/" component={EditRuns} />
                <NotFoundPage default />
              </Router>
            </Container>
          </GoogleOAuthProvider>
        </PersistGate>
      </Provider>
    </div>
  );
};

export default App;
