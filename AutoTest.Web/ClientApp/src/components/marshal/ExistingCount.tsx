import { uniqBy } from "@s-libs/micro-dash";
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
            ? uniqBy(
                  testRuns.filter(
                      (run) =>
                          run.entrantId === entrantId && run.ordinal === ordinal
                  ),
                  (a) => a.testRunId
              ).length
            : "NA"}{" "}
        of{" "}
        {currentEvent !== undefined
            ? currentEvent.maxAttemptsPerTest
            : "unknown"}
    </span>
);

export default ExportCount;
