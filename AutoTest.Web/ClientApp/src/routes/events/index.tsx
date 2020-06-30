import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Column, Button } from "rbx";
import { route } from "preact-router";
import UUID from "uuid-int";
import { fromDateOrThrow } from "ts-date";

import { getEvents, addEvent } from "../../api/events";
import { LoadingState, Event } from "../../types/models";
import ifSome from "../../components/shared/isSome";
import Modal from "../../components/events/Modal";
import { getAccessToken } from "../../api/api";
import { useGoogleAuth } from "../../components/app";

interface Props {
    clubId: number | undefined;
}
const uid = UUID(Number.parseInt(process.env.PREACT_APP_KEY_SEED as string));

const Events: FunctionalComponent<Props> = () => {
    const auth = useGoogleAuth();
    const [events, setEvents] = useState<LoadingState<readonly Event[]>>({
        tag: "Loading",
    });
    const [editingEvent, setEditingEvent] = useState<Event | undefined>(
        undefined
    );
    useEffect(() => {
        const fetchData = async () => {
            const events = await getEvents();
            setEvents(events);
        };
        void fetchData();
    }, []);
    const save = async () => {
        if (editingEvent) {
            await addEvent(editingEvent, getAccessToken(auth));
            setEditingEvent(undefined);
            setEvents(await getEvents());
        }
    };
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
                            <Button onClick={() => setEditingEvent(a)}>
                                Edit
                            </Button>
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
            <Button
                onClick={() =>
                    setEditingEvent({
                        clubId: 1, // todo
                        eventId: uid.uuid(),
                        location: "",
                        startTime: fromDateOrThrow(new Date()),
                        testCount: 12,
                    })
                }
            >
                Add Club
            </Button>
            {editingEvent ? (
                <Modal
                    event={editingEvent}
                    setField={(a: Partial<Event>) =>
                        setEditingEvent((b) => ({ ...b, ...a } as Event))
                    }
                    cancel={() => setEditingEvent(undefined)}
                    save={save}
                />
            ) : null}
        </div>
    );
};

export default Events;
