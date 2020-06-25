import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title } from "rbx";

import { getEntrants } from "../../api/entrants";
import { LoadingState, Entrant } from "../../types/models";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";

const Events: FunctionalComponent = () => {
    const [events, setEvents] = useState<LoadingState<readonly Entrant[]>>({
        tag: "Loading",
    });
    const auth = useGoogleAuth();
    useEffect(() => {
        const fetchData = async () => {
            const events = await getEntrants(getAccessToken(auth));
            setEvents(events);
        };
        void fetchData();
    }, [auth]);

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
