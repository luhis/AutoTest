import { h, FunctionComponent } from "preact";
import { Columns } from "react-bulma-components";

export const comp: FunctionComponent = ({ children }) => (
  <Columns.Column className="pb-0">{children}</Columns.Column>
);

export default comp;
