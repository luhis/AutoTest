import { h } from "preact";
import { render, screen } from "@testing-library/preact";
import { configureStore } from "@reduxjs/toolkit";
import { Provider } from "react-redux";
import { GoogleOAuthProvider } from "@react-oauth/google";

import Header from "../components/header";
import { rootReducer } from "../store";

const store = configureStore({ reducer: rootReducer });

const renderHeader = () =>
  render(
    <GoogleOAuthProvider clientId="test-client-id">
      <Provider store={store}>
        <Header />
      </Provider>
    </GoogleOAuthProvider>,
  );

describe("Header", () => {
  test("renders the app title", () => {
    renderHeader();
    expect(screen.getByText("AutoTest")).toBeInTheDocument();
  });

  test("renders navigation links", () => {
    renderHeader();
    expect(screen.getByText("Home")).toBeInTheDocument();
    expect(screen.getByText("Events")).toBeInTheDocument();
  });
});
