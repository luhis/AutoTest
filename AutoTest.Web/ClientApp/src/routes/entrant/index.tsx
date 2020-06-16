import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";

import { getEvents } from "../../api/events";
import { LoadingState } from "../../types/models";
import { Event } from "../../types/models";

const Events: FunctionalComponent = () => {
    const [events, setEvents] = useState<LoadingState<readonly Entrant[]>>({
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
            <h1>Events</h1>
            {events.tag === "Loaded"
                ? events.value.map((a) => <p key={a.clubId}>{a.location}</p>)
                : null}
            <p>This is the Events component.</p>
        </div>
    );
};

export default Events;
