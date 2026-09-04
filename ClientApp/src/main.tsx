import "./style/index.css";
import { h, render } from "preact";
import App from "./components/app";

const root = document.getElementById("app");
if (root) {
  render(<App />, root);
}
