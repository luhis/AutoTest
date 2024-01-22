import { uniqBy } from "@s-libs/micro-dash";
import { FunctionalComponent, h } from "preact";

import { Event } from "../../types/models";

interface Props {
  readonly entrantId: number | undefined;
  readonly testRuns: readonly {
    readonly testRunId: number;
    readonly entrantId: number;
  }[];
  readonly currentEvent: Event | undefined;
}

const ExportCount: FunctionalComponent<Props> = ({
  entrantId,
  testRuns,
  currentEvent,
}) => (
  <span>
    {entrantId
      ? uniqBy(
          testRuns.filter((run) => run.entrantId === entrantId),
          (a) => a.testRunId,
        ).length
      : "NA"}{" "}
    of{" "}
    {currentEvent !== undefined ? currentEvent.maxAttemptsPerTest : "unknown"}
  </span>
);

export default ExportCount;
