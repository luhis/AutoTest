import { h, FunctionComponent } from "preact";
import { FaExclamation } from "react-icons/fa";

import { startCase } from "../../lib/string";
import { Penalty, PenaltyType } from "../../types/models";

const penaltyTypeToString = (p: PenaltyType) => startCase(PenaltyType[p]);

const Penalties: FunctionComponent<{
  readonly penalties: readonly Penalty[];
}> = ({ penalties }) =>
  penalties.length > 0 ? (
    <FaExclamation
      color="danger"
      title={penalties
        .map((p) => `${p.instanceCount}x ${penaltyTypeToString(p.penaltyType)}`)
        .join(", ")}
    />
  ) : null;

export default Penalties;
