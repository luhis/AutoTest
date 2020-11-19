import { FunctionalComponent, h } from "preact";

import { TestRun, Event } from "../../types/models";

interface Props {
    readonly entrantId: number | undefined;
    readonly ordinal: number;
    readonly testRuns: readonly TestRun[];
    readonly currentEvent: Event | undefined;
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
