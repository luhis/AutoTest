import { type FunctionComponent, h } from "preact";
import { lazy, Suspense } from "preact/compat";
import { Route, Router } from "preact-router";
import { Provider } from "react-redux";
import { Container, Loader } from "react-bulma-components";
import { PersistGate } from "redux-persist/integration/react";
import { GoogleOAuthProvider } from "@react-oauth/google";
import { ReactPlugin } from "@microsoft/applicationinsights-react-js";
import { ApplicationInsights } from "@microsoft/applicationinsights-web";

import Header from "./header";
import store from "../store";
import { appInsightsKey, googleKey } from "../settings";

import "bulma/css/bulma.min.css";

const Home = lazy(() => import("../routes/home"));
const Profile = lazy(() => import("../routes/profile"));
const Club = lazy(() => import("../routes/clubs"));
const NotFoundPage = lazy(() => import("../routes/notfound"));
const Events = lazy(() => import("../routes/events"));
const Event = lazy(() => import("../routes/event"));
const Entrant = lazy(() => import("../routes/entrants"));
const Marshals = lazy(() => import("../routes/marshals"));
const Results = lazy(() => import("../routes/results"));
const Tests = lazy(() => import("../routes/tests"));
const Marshal = lazy(() => import("../routes/marshal"));
const LiveRuns = lazy(() => import("../routes/liveRuns"));
const EditRuns = lazy(() => import("../routes/editRuns"));

if (import.meta.hot) {
  void import("preact/debug");
}

if (typeof window !== "undefined") {
  const reactPlugin = new ReactPlugin();

  const appInsights = new ApplicationInsights({
    config: {
      instrumentationKey: appInsightsKey,
      extensions: [reactPlugin],
      extensionConfig: {
        [reactPlugin.identifier]: { enableAutoRouteTracking: true },
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
            <Container fluid class="section">
              <Suspense fallback={<Loader />}>
                <Router>
                  <Route path="/" component={Home} />
                  <Route path="/profile/" component={Profile} />
                  <Route path="/clubs/" component={Club} />
                  <Route path="/events/" component={Events} />
                  <Route path="/event/:eventId" component={Event} />
                  <Route path="/entrants/:eventId" component={Entrant} />
                  <Route path="/marshals/:eventId" component={Marshals} />
                  <Route
                    path="/results/:eventId/:params?"
                    component={Results}
                  />
                  <Route path="/tests/:eventId" component={Tests} />
                  <Route
                    path="/marshal/:eventId/:ordinal"
                    component={Marshal}
                  />
                  <Route
                    path="/liveRuns/:eventId/:params?"
                    component={LiveRuns}
                  />
                  <Route path="/editRuns/:eventId/" component={EditRuns} />
                  <NotFoundPage default />
                </Router>
              </Suspense>
            </Container>
          </GoogleOAuthProvider>
        </PersistGate>
      </Provider>
    </div>
  );
};

export default App;
