import { uniqBy } from "@s-libs/micro-dash";
import { type FunctionComponent, h } from "preact";

import type { Event } from "../../types/models";

interface Props {
  readonly entrantId: number | undefined;
  readonly testRuns: readonly {
    readonly testRunId: number;
    readonly entrantId: number;
  }[];
  readonly currentEvent: Event | undefined;
}

const ExportCount: FunctionComponent<Props> = ({
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
    {currentEvent !== undefined ? currentEvent.maxAttemptsPerCourse : "unknown"}
  </span>
);

export default ExportCount;
