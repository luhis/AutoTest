import { FunctionComponent, h } from "preact";

import { EntrantTime } from "../../types/models";
import Penalties from "../shared/Penalties";

const None: FunctionComponent = () => <span>X</span>;

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
                {(runValue.timeInMS / 1000).toFixed(2)}s
                <Penalties penalties={runValue.penalties} />
            </span>
        ) : (
            <None />
        );
    }
    return <None />;
};

export default Time;
