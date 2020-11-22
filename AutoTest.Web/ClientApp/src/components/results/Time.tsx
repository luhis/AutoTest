import { FunctionComponent, h } from "preact";
import { FaExclamation } from "react-icons/fa";

import { startCase } from "../../lib/string";
import { EntrantTime, Penalty, PenaltyType } from "../../types/models";

const None: FunctionComponent = () => <span>X</span>;

const Penalties: FunctionComponent<{
    readonly penalties: readonly Penalty[];
}> = ({ penalties }) =>
    penalties.length > 0 ? (
        <FaExclamation
            title={penalties
                .map(
                    (p) =>
                        `${p.instanceCount}x ${startCase(
                            PenaltyType[p.penaltyType]
                        )}`
                )
                .join(", ")}
        />
    ) : null;

const Time: FunctionComponent<{
    readonly times: EntrantTime;
    readonly ordinal: number;
    readonly run: number;
}> = ({ times, ordinal, run }) => {
    const testValues = times.times.find((t) => t.ordinal === ordinal);
    if (testValues) {
        const runValue = testValues.testRuns[run];
        return runValue ? (
            <span>
                {(runValue.timeInMS / 1000).toFixed(2)}
                <Penalties penalties={runValue.penalties} />
            </span>
        ) : (
            <None />
        );
    }
    return <None />;
};

export default Time;
