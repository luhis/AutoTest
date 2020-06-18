import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";

import { getEntrants } from "../../api/entrants";
import { LoadingState, Entrant } from "../../types/models";

const Events: FunctionalComponent = () => {
    const [events, setEvents] = useState<LoadingState<readonly Entrant[]>>({
        tag: "Loading",
    });
    useEffect(() => {
        const fetchData = async () => {
            const events = await getEntrants();
            setEvents(events);
        };
        void fetchData();
    }, []);
    return (
        <div>
            <h1>Entrants</h1>
            {events.tag === "Loaded"
                ? events.value.map((a) => (
                      <p key={a.entrantId}>{a.entrantId}</p>
                  ))
                : null}
            <p>This is the Events component.</p>
        </div>
    );
};

export default Events;
