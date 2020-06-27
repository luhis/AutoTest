import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title } from "rbx";

import { getEntrants } from "../../api/entrants";
import { LoadingState, Entrant } from "../../types/models";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";

interface Props {
    eventId: number;
}

const Events: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const [events, setEvents] = useState<LoadingState<readonly Entrant[]>>({
        tag: "Loading",
    });
    const auth = useGoogleAuth();
    useEffect(() => {
        const fetchData = async () => {
            const events = await getEntrants(eventId, getAccessToken(auth));
            setEvents(events);
        };
        void fetchData();
    }, [auth, eventId]);

    return (
        <div>
            <Title>Entrants</Title>
            {events.tag === "Loaded"
                ? events.value.map((a) => (
                      <p key={a.entrantId}>{a.entrantId}</p>
                  ))
                : null}
        </div>
    );
};

export default Events;
