import { FunctionalComponent, h } from "preact";
import { Heading } from "react-bulma-components";

const Home: FunctionalComponent = () => {
  return (
    <div>
      <Heading>Home</Heading>
      <p>This is the Mangaji AutoTest app.</p>
      <Heading>Today&apos;s Events</Heading>
      <Heading>New Events</Heading>
    </div>
  );
};

export default Home;
