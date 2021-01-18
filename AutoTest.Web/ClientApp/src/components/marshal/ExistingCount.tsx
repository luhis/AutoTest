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
        {entrantId
            ? testRuns.filter(
                  (run) =>
                      run.entrantId === entrantId && run.ordinal === ordinal
              ).length
            : "NA"}{" "}
        of{" "}
        {currentEvent !== undefined
            ? currentEvent.maxAttemptsPerTest
            : "unknown"}
    </span>
);

export default ExportCount;
