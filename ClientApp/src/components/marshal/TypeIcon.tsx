import { h, FunctionComponent } from "preact";
import {
  FaCarCrash,
  FaClock,
  FaDirections,
  FaStopCircle,
  FaUserSlash,
} from "react-icons/fa";

import { PenaltyType } from "../../types/models";

const TypeIcon: FunctionComponent<{ readonly type: PenaltyType }> = ({
  type,
}) => {
  switch (type) {
    case PenaltyType.HitBarrier:
      return <FaCarCrash size="medium" />;
    case PenaltyType.Late:
      return <FaClock size="medium" />;
    case PenaltyType.NoAttendance:
      return <FaUserSlash size="medium" />;
    case PenaltyType.WrongTest:
      return <FaDirections size="medium" />;
    case PenaltyType.FailToStop:
      return <FaStopCircle size="medium" />;
  }
};

export default TypeIcon;
