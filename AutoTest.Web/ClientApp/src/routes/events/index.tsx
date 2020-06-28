import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Column, Button } from "rbx";
import { route } from "preact-router";

import { getEvents } from "../../api/events";
import { LoadingState, Event } from "../../types/models";
import ifSome from "../../components/shared/isSome";

interface Props {
    clubId: number | undefined;
}

const Events: FunctionalComponent<Props> = () => {
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
            {ifSome(events, (a) => (
                <Column.Group>
                    <Column>
                        <p key={a.eventId}>{a.location}</p>
                    </Column>
                    <Column>
                        <Button.Group>
                            <Button
                                onClick={() => route(`/entrants/${a.eventId}`)}
                            >
                                Entrants
                            </Button>
                            <Button
                                onClick={() => route(`/tests/${a.eventId}`)}
                            >
                                Tests
                            </Button>
                            <Button
                                onClick={() => route(`/results/${a.eventId}`)}
                            >
                                Results
                            </Button>
                        </Button.Group>
                    </Column>
                </Column.Group>
            ))}
        </div>
    );
};

export default Events;
