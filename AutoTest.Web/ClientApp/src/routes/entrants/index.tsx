import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title } from "rbx";

import { getEntrants } from "../../api/entrants";
import { LoadingState, Entrant } from "../../types/models";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import ifSome from "../../components/shared/isSome";

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
            {ifSome(events, (a) => (
                <p key={a.entrantId}>{a.entrantId}</p>
            ))}
        </div>
    );
};

export default Events;
