import { FunctionalComponent, h } from "preact";
import { useEffect } from "preact/hooks";

import { getEvents } from "../../api/events";

const Events: FunctionalComponent = () => {
    useEffect(() => void getEvents(), []);
    return (
        <div>
            <h1>Events</h1>
            <p>This is the Events component.</p>
        </div>
    );
};

export default Events;
