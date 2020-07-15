import { FunctionalComponent, h } from "preact";

import { TestRun, Event } from "../../types/models";

interface Props {
    entrantId: number | undefined;
    testId: number;
    testRuns: readonly TestRun[];
    currentEvent: Event | undefined;
}

const ExportCount: FunctionalComponent<Props> = ({
    entrantId,
    testId,
    testRuns,
    currentEvent,
}) => (
    <span>
        {
            testRuns.filter(
                (a) => a.entrantId === entrantId && a.testId === testId
            ).length
        }{" "}
        of{" "}
        {currentEvent !== undefined
            ? currentEvent.maxAttemptsPerTest
            : "unknown"}
    </span>
);

export default ExportCount;
