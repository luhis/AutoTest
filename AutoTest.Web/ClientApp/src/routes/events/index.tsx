import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Column } from "rbx";
import { Link } from "preact-router";

import { getEvents } from "../../api/events";
import { LoadingState } from "../../types/models";
import { Event } from "../../types/models";

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
                      <Column.Group key={a.eventId}>
                          <Column>
                              <p key={a.eventId}>{a.location}</p>
                          </Column>
                          <Column>
                              <Link href={`/entrants?eventId=${a.eventId}`}>
                                  Entrants
                              </Link>
                              <Link href={`/marshal/${a.eventId}`}>
                                  Marshal
                              </Link>
                              <Link href={`results/${a.eventId}`}>Results</Link>
                          </Column>
                      </Column.Group>
                  ))
                : null}
        </div>
    );
};

export default Events;
