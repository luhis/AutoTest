import { h, FunctionComponent } from "preact";
import { Tag } from "react-bulma-components";

interface Props {
  readonly registration: string;
}

const NumberPlate: FunctionComponent<Props> = ({ registration }) => {
  return (
    <Tag color="warning" size="medium" className="has-text-weight-bold">
      {registration}
    </Tag>
  );
};

export default NumberPlate;
