import { h, FunctionComponent } from "preact";
import { Breadcrumb } from "react-bulma-components";
import { Link } from "preact-router";

import { Club, Event } from "../../types/models";

interface Props {
  readonly club: Club | undefined;
  readonly event?: Event;
  readonly test?: number;
}

const Breadcrumbs: FunctionComponent<Props> = ({ club, event, test }) => {
  return (
    <Breadcrumb>
      {club ? (
        <Breadcrumb.Item>
          <Link href={`/events?clubId=${club.clubId}`}>{club.clubName}</Link>
        </Breadcrumb.Item>
      ) : null}
      {event ? (
        <Breadcrumb.Item>
          <Link href={`/event/${event.eventId}`}>
            {`${event.location} ${event.startTime.toLocaleDateString()}`}
          </Link>
        </Breadcrumb.Item>
      ) : null}
      {test !== undefined ? (
        <Breadcrumb.Item>
          <span className="pl-3">Test No. {test + 1}</span>
        </Breadcrumb.Item>
      ) : null}
    </Breadcrumb>
  );
};

export default Breadcrumbs;
