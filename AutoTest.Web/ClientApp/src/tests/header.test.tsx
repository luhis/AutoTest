import { h } from "preact";
// See: https://github.com/preactjs/enzyme-adapter-preact-pure
import { shallow } from "enzyme";
import { createStore } from "redux";
import { Provider } from "react-redux";

import Header from "../components/header";
import { rootReducer } from "../store";

describe("Initial Test of the Header", () => {
    const store = createStore(rootReducer);
    test("Header renders 3 nav items", () => {
        shallow(
            <Provider store={store}>
                <Header />
            </Provider>
        );
    });
});
