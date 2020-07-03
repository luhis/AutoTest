import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Button } from "rbx";
import UUID from "uuid-int";
import { fromDateOrThrow } from "ts-date";
import { useDispatch } from "react-redux";

import { getEvents, addEvent } from "../../api/events";
import { LoadingState, Event } from "../../types/models";
import Modal from "../../components/events/Modal";
import { getAccessToken } from "../../api/api";
import { useGoogleAuth } from "../../components/app";
import List from "../../components/events/List";
import { GetEntrants } from "../../store/event/actions";

interface Props {
    clubId: string | undefined;
}
const uid = UUID(Number.parseInt(process.env.PREACT_APP_KEY_SEED as string));

const Events: FunctionalComponent<Props> = ({ clubId }) => {
    const dispatch = useDispatch();
    const auth = useGoogleAuth();
    const [events, setEvents] = useState<LoadingState<readonly Event[]>>({
        tag: "Loading",
    });
    const [editingEvent, setEditingEvent] = useState<Event | undefined>(
        undefined
    );
    useEffect(() => {
        dispatch(GetEntrants())
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
            <List events={events} setEditingEvent={setEditingEvent} />
            <Button
                onClick={() =>
                    setEditingEvent({
                        clubId:
                            clubId === undefined ? 0 : Number.parseInt(clubId), // todo
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
