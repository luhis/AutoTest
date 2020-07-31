import { FunctionComponent, h } from "preact";

import { EntrantTime } from "../../types/models";
import { FaExclamation } from "react-icons/fa";

const None: FunctionComponent = () => <span>X</span>;

//todo, update the api to pass down penalties
const Penalties: FunctionComponent<{ penalties: readonly unknown[] }> = ({
    penalties,
}) => (penalties.length > 0 ? <FaExclamation /> : null);

const Time: FunctionComponent<{
    times: EntrantTime;
    ordinal: number;
    run: number;
}> = ({ times, ordinal, run }) => {
    const testValues = times.times.find((t) => t.ordinal === ordinal);
    if (testValues) {
        const runValue = testValues.timesInMs[run];
        return runValue ? (
            <span>
                {(runValue / 1000).toFixed(2)} <Penalties penalties={[]} />
            </span>
        ) : (
            <None />
        );
    }
    return <None />;
};

export default Time;
