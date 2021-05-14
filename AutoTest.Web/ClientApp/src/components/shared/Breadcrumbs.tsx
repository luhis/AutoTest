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
                <Breadcrumb.Item>
                    <a href={`/events?clubId=${club.clubId}`}>
                        {club.clubName}
                    </a>
                </Breadcrumb.Item>
            ) : null}
            {event ? (
                <Breadcrumb.Item>
                    <a href={`/event/${event.eventId}`}>
                        {`${
                            event.location
                        } ${event.startTime.toLocaleDateString()}`}
                    </a>
                </Breadcrumb.Item>
            ) : null}
            {test ? (
                <Breadcrumb.Item>Test No. {test + 1}</Breadcrumb.Item>
            ) : null}
        </Breadcrumb>
    );
};

export default Breadcrumbs;
