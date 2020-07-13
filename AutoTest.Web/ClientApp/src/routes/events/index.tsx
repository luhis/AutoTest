import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Button } from "rbx";
import UUID from "uuid-int";
import { fromDateOrThrow } from "ts-date";

import { getEvents, addEvent } from "../../api/events";
import { Event, EditingEvent } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import Modal from "../../components/events/Modal";
import { getAccessToken } from "../../api/api";
import { useGoogleAuth } from "../../components/app";
import List from "../../components/events/List";

interface Props {
    clubId: string | undefined;
}
const uid = UUID(Number.parseInt(process.env.PREACT_APP_KEY_SEED as string));

const Events: FunctionalComponent<Readonly<Props>> = ({ clubId }) => {
    const auth = useGoogleAuth();
    const [events, setEvents] = useState<LoadingState<readonly Event[]>>({
        tag: "Loading",
        id: undefined,
    });
    const [editingEvent, setEditingEvent] = useState<EditingEvent | undefined>(
        undefined
    );
    useEffect(() => {
        const fetchData = async () => {
            setEvents(await getEvents());
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
            <List
                events={events}
                setEditingEvent={(a) => setEditingEvent({ ...a, isNew: false })}
            />
            <Button
                onClick={() =>
                    setEditingEvent({
                        clubId:
                            clubId === undefined ? 0 : Number.parseInt(clubId), // todo
                        eventId: uid.uuid(),
                        location: "",
                        startTime: fromDateOrThrow(new Date()),
                        testCount: 12,
                        maxAttemptsPerTest: 2,
                        marshalEmails: [],
                        isNew: true,
                    })
                }
            >
                Add Event
            </Button>
            {editingEvent ? (
                <Modal
                    event={editingEvent}
                    setField={(a: Partial<Event>) =>
                        setEditingEvent((b) => ({ ...b, ...a } as EditingEvent))
                    }
                    cancel={() => setEditingEvent(undefined)}
                    save={save}
                />
            ) : null}
        </div>
    );
};

export default Events;
