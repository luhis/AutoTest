import { h, FunctionComponent } from "preact";
import { Breadcrumb } from "react-bulma-components";

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
                <Breadcrumb.Item href={`/events?clubId=${club.clubId}`}>
                    {club.clubName}
                </Breadcrumb.Item>
            ) : null}
            {event ? (
                <Breadcrumb.Item href={`/event/${event.eventId}`}>
                    {`${
                        event.location
                    } ${event.startTime.toLocaleDateString()}`}
                </Breadcrumb.Item>
            ) : null}
            {test ? (
                <Breadcrumb.Item>Test No. {test + 1}</Breadcrumb.Item>
            ) : null}
        </Breadcrumb>
    );
};

export default Breadcrumbs;
