import { h, FunctionComponent, Fragment } from "preact";
import { Form, Button, Columns } from "react-bulma-components";
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
            return <FaCarCrash />;
        case PenaltyType.Late:
            return <FaClock />;
        case PenaltyType.NoAttendance:
            return <FaUserSlash />;
        case PenaltyType.WrongTest:
            return <FaDirections />;
        case PenaltyType.FailToStop:
            return <FaStopCircle />;
    }
};

const PenaltyItem: FunctionComponent<
    Props & { readonly penaltyType: PenaltyType }
> = ({ penaltyType, penalties, increase, decrease }) => {
    return (
        <Columns>
            <Columns.Column>
                <Field kind="group" class="has-text-left">
                    <Button onClick={() => decrease(penaltyType)}>
                        <FaMinus />
                    </Button>
                    <Button onClick={() => increase(penaltyType)}>
                        <FaPlus />
                    </Button>
                </Field>
            </Columns.Column>
            <Columns.Column>
                <TypeIcon type={penaltyType} />
                &nbsp;
                {startCase(PenaltyType[penaltyType])}:{" "}
                {getCount(penalties, penaltyType)}
            </Columns.Column>
        </Columns>
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
