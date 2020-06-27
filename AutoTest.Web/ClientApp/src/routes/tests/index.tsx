import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Column } from "rbx";
import { Link } from "preact-router";

import { getEvents } from "../../api/events";
import { LoadingState, Event } from "../../types/models";

const Tests: FunctionalComponent = () => {
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
            <Title>Tests</Title>
            {events.tag === "Loaded"
                ? events.value.map((a) => (
                      <Column.Group key={a.eventId}>
                          <Column>
                              <p key={a.eventId}>{a.location}</p>
                          </Column>
                          <Column>
                              <Link href={`/entrants/${a.eventId}`}>
                                  Entrants
                              </Link>
                              <Link href={`/tests/${a.eventId}`}>Tests</Link>
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

export default Tests;
