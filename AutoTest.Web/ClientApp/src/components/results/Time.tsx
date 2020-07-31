import { FunctionComponent, h } from "preact";
import { FaExclamation } from "react-icons/fa";

import { EntrantTime, Penalty, PenaltyType } from "../../types/models";

const None: FunctionComponent = () => <span>X</span>;

const startCase = (s: string) => s.replace(/([A-Z]+)*([A-Z][a-z])/g, "$1 $2");

const Penalties: FunctionComponent<{ penalties: readonly Penalty[] }> = ({
    penalties,
}) =>
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
    times: EntrantTime;
    ordinal: number;
    run: number;
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
