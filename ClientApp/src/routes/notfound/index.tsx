import { FunctionalComponent, h } from "preact";
import { Hero, Heading, Button } from "react-bulma-components";
import { Link } from "preact-router/match";

const Notfound: FunctionalComponent = () => {
  return (
    <Hero color="danger" size="medium">
      <Hero.Body>
        <Heading class="has-text-white" spaced>
          404
        </Heading>
        <Heading subtitle class="has-text-white-ter">
          The page you&apos;re looking for doesn&apos;t exist.
        </Heading>
        <Button color="light" renderAs={Link} href="/">
          Back to Home
        </Button>
      </Hero.Body>
    </Hero>
  );
};

export default Notfound;
