import { h, FunctionComponent, Fragment } from "preact";
import { Label, Button, Level } from "rbx";
import { FaPlus, FaMinus } from "react-icons/fa";

import { startCase } from "../../lib/string";
import { PenaltyType, Penalty } from "../../types/models";

interface Props {
    readonly penalties: readonly Penalty[];
    readonly increase: (a: PenaltyType) => void;
    readonly decrease: (a: PenaltyType) => void;
}

const PenaltyItem: FunctionComponent<
    Props & { readonly penaltyType: PenaltyType }
> = ({ penaltyType, penalties, increase, decrease }) => {
    return (
        <Level>
            <Level.Item align="left">
                <Button.Group>
                    <Button onClick={() => decrease(penaltyType)}>
                        <FaMinus />
                    </Button>
                    <Button onClick={() => increase(penaltyType)}>
                        <FaPlus />
                    </Button>
                </Button.Group>
                {startCase(PenaltyType[penaltyType])}:{" "}
                {getCount(penalties, penaltyType)}
            </Level.Item>
        </Level>
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
