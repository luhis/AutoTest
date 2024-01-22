import { h, FunctionComponent, Fragment } from "preact";
import { Form, Button, Icon } from "react-bulma-components";
import {
  FaPlus,
  FaMinus,
  FaClock,
  FaCarCrash,
  FaStopCircle,
  FaDirections,
  FaUserSlash,
} from "react-icons/fa";
const { Field, Label } = Form;

import { startCase } from "../../lib/string";
import { PenaltyType, Penalty } from "../../types/models";

interface Props {
  readonly penalties: readonly Penalty[];
  readonly increase: (a: PenaltyType) => void;
  readonly decrease: (a: PenaltyType) => void;
}

const TypeIcon: FunctionComponent<{ readonly type: PenaltyType }> = ({
  type,
}) => {
  switch (type) {
    case PenaltyType.HitBarrier:
      return <FaCarCrash size="medium" />;
    case PenaltyType.Late:
      return <FaClock size="medium" />;
    case PenaltyType.NoAttendance:
      return <FaUserSlash size="medium" />;
    case PenaltyType.WrongTest:
      return <FaDirections size="medium" />;
    case PenaltyType.FailToStop:
      return <FaStopCircle size="medium" />;
  }
};

const PenaltyItem: FunctionComponent<
  Props & { readonly penaltyType: PenaltyType }
> = ({ penaltyType, penalties, increase, decrease }) => {
  return (
    <p class="mb-3">
      <Field kind="group" class="has-text-left">
        <Button.Group>
          <Button type="button" onClick={() => decrease(penaltyType)}>
            <FaMinus />
          </Button>
          <Button type="button" onClick={() => increase(penaltyType)}>
            <FaPlus />
          </Button>
        </Button.Group>
        <Icon size="medium ml-2">
          <TypeIcon type={penaltyType} />
        </Icon>
        &nbsp;
        <span class="has-text-medium">
          {`${startCase(PenaltyType[penaltyType])}: ${getCount(
            penalties,
            penaltyType,
          )}`}
        </span>
      </Field>
    </p>
  );
};

const getCount = (penalties: readonly Penalty[], penaltyType: PenaltyType) => {
  const found = penalties.find((a) => a.penaltyType === penaltyType);
  if (found === undefined) {
    return 0;
  } else {
    return found.instanceCount;
  }
};
const Penalties: FunctionComponent<Readonly<Props>> = ({
  penalties,
  increase,
  decrease,
}) => {
  return (
    <Fragment>
      <Label>Penalties</Label>
      {Object.keys(PenaltyType)
        .map((a) => Number.parseInt(a))
        .filter((key) => !isNaN(key))
        .map((key) => (
          <PenaltyItem
            key={key}
            penaltyType={key}
            increase={increase}
            decrease={decrease}
            penalties={penalties}
          />
        ))}
    </Fragment>
  );
};

export default Penalties;
