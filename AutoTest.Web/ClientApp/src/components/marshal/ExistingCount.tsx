import { FunctionalComponent, h } from "preact";

import { TestRun, Event } from "../../types/models";

interface Props {
    entrantId: number | undefined;
    ordinal: number;
    testRuns: readonly TestRun[];
    currentEvent: Event | undefined;
}

const ExportCount: FunctionalComponent<Props> = ({
    entrantId,
    ordinal,
    testRuns,
    currentEvent,
}) => (
    <span>
        {
            testRuns.filter(
                (a) => a.entrantId === entrantId && a.ordinal === ordinal
            ).length
        }{" "}
        of{" "}
        {currentEvent !== undefined
            ? currentEvent.maxAttemptsPerTest
            : "unknown"}
    </span>
);

export default ExportCount;
