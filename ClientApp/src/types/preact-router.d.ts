import "preact-router";
import "preact-router/match";

declare module "preact-router" {
  export function Link(
    props: import("preact").JSX.AnchorHTMLAttributes<HTMLAnchorElement>,
  ): import("preact").VNode;
}

declare module "preact-router/match" {
  export interface LinkProps {
    readonly href?: string;
  }
}
