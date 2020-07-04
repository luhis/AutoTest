import { h, FunctionComponent, Fragment } from "preact";
import { Label, Button, List } from "rbx";
import { FaPlus, FaMinus } from "react-icons/fa";

import { PenaltyType, Penalty } from "../../types/models";

interface Props {
    penalties: readonly Penalty[];
    increase: (a: PenaltyType) => void;
    decrease: (a: PenaltyType) => void;
}

const PenaltyItem: FunctionComponent<Props & { penaltyType: PenaltyType }> = ({
    penaltyType,
    penalties,
    increase,
    decrease,
}) => {
    return (
        <List.Item>
            {PenaltyType[penaltyType]} {getCount(penalties, penaltyType)}
            <Button.Group>
                <Button onClick={() => increase(penaltyType)}>
                    <FaPlus />
                </Button>
                <Button onClick={() => decrease(penaltyType)}>
                    <FaMinus />
                </Button>
            </Button.Group>
        </List.Item>
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
            <List>
                {Object.keys(PenaltyType)
                    .filter((key) => !isNaN(Number(key)))
                    .map((key) => (
                        <PenaltyItem
                            key={key}
                            penaltyType={Number(key)}
                            increase={increase}
                            decrease={decrease}
                            penalties={penalties}
                        />
                    ))}
            </List>
        </Fragment>
    );
};

export default Penalties;
