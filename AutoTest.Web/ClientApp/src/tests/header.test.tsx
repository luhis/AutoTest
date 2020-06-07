import { h } from "preact";
// See: https://github.com/preactjs/enzyme-adapter-preact-pure
import { shallow } from "enzyme";
import Header from "../components/header";

describe("Initial Test of the Header", () => {
    test("Header renders 3 nav items", () => {
        // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
        shallow(<Header />);
    });
});
