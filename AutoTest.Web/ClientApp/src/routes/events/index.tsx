import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Button } from "rbx";
import UUID from "uuid-int";
import { newValidDate } from "ts-date";
import { useDispatch, useSelector } from "react-redux";

import { addEvent } from "../../api/events";
import { Event, EditingEvent } from "../../types/models";
import Modal from "../../components/events/Modal";
import { getAccessToken } from "../../api/api";
import { useGoogleAuth } from "../../components/app";
import List from "../../components/events/List";
import {
    GetEventsIfRequired,
    GetEvents,
    GetClubsIfRequired,
} from "../../store/event/actions";
import { selectEvents, selectClubs } from "../../store/event/selectors";
import { keySeed } from "../../settings";
import RegsModal from "../../components/events/RegsModal";

interface Props {
    readonly clubId: string | undefined;
}
const uid = UUID(keySeed);

const Events: FunctionalComponent<Props> = ({ clubId }) => {
    const dispatch = useDispatch();
    const auth = useGoogleAuth();
    const events = useSelector(selectEvents);
    const clubs = useSelector(selectClubs);
    const [editingEvent, setEditingEvent] = useState<EditingEvent | undefined>(
        undefined
    );
    const [regsId, setRegsId] = useState<Event | undefined>(undefined);
    useEffect(() => {
        dispatch(GetEventsIfRequired());
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
    }, [auth, dispatch]);
    const save = async () => {
        if (editingEvent && editingEvent.clubId) {
            await addEvent(
                { ...editingEvent, clubId: editingEvent.clubId },
                getAccessToken(auth)
            );
            setEditingEvent(undefined);
            dispatch(GetEvents());
        }
    };
    return (
        <div>
            <Title>Events</Title>
            <List
                events={events}
                setEditingEvent={(a) =>
                    setEditingEvent({
                        ...a,
                        isNew: false,
                        isClubEditable: clubId === undefined,
                    })
                }
                setRegsModal={(a) => setRegsId(a)}
            />
            <Button
                onClick={() =>
                    setEditingEvent({
                        clubId:
                            clubId === undefined
                                ? undefined
                                : Number.parseInt(clubId), // todo
                        eventId: uid.uuid(),
                        location: "",
                        startTime: newValidDate(),
                        testCount: 12,
                        maxAttemptsPerTest: 2,
                        marshalEmails: [],
                        isNew: true,
                        isClubEditable: clubId === undefined,
                        tests: [],
                        regulations: null,
                    })
                }
            >
                Add Event
            </Button>
            {editingEvent ? (
                <Modal
                    event={editingEvent}
                    clubs={clubs}
                    setField={(a: Partial<Event>) => {
                        setEditingEvent((b) => {
                            return { ...b, ...a } as EditingEvent;
                        });
                    }}
                    cancel={() => setEditingEvent(undefined)}
                    save={save}
                />
            ) : null}
            {regsId ? (
                <RegsModal event={regsId} cancel={() => setRegsId(undefined)} />
            ) : null}
        </div>
    );
};

export default Events;
