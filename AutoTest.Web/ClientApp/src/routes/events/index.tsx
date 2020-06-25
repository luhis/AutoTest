import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Column } from "rbx";

import { getEvents } from "../../api/events";
import { LoadingState } from "../../types/models";
import { Event } from "../../types/models";
import { Link } from "preact-router";

const Events: FunctionalComponent = () => {
    const [events, setEvents] = useState<LoadingState<readonly Event[]>>({
        tag: "Loading",
    });
    useEffect(() => {
        const fetchData = async () => {
            const events = await getEvents();
            setEvents(events);
        };
        void fetchData();
    }, []);
    return (
        <div>
            <Title>Events</Title>
            {events.tag === "Loaded"
                ? events.value.map((a) => (
                      <Column.Group key={a.clubId}>
                          <Column>
                              <p key={a.clubId}>{a.location}</p>
                          </Column>
                          <Column>
                              <Link href={`/entrants?eventId=${a.clubId}`}>
                                  Entrants
                              </Link>
                          </Column>
                      </Column.Group>
                  ))
                : null}
        </div>
    );
};

export default Events;
