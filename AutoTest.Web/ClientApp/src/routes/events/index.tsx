import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Button } from "rbx";
import UUID from "uuid-int";
import { newValidDate } from "ts-date";
import { useDispatch, useSelector } from "react-redux";

import { Event, EditingEvent, Override } from "../../types/models";
import Modal from "../../components/events/Modal";
import { getAccessToken } from "../../api/api";
import { useGoogleAuth } from "../../components/app";
import List from "../../components/events/List";
import {
    GetEventsIfRequired,
    GetClubsIfRequired,
    AddEvent,
    DeleteEvent,
} from "../../store/event/actions";
import { selectEvents, selectClubs } from "../../store/event/selectors";
import { keySeed } from "../../settings";
import RegsModal from "../../components/events/RegsModal";
import RouteParamsParser from "../../components/shared/RouteParamsParser";

interface Props {
    readonly clubId: number | undefined;
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
    const save = () => {
        if (editingEvent && editingEvent.clubId) {
            dispatch(
                AddEvent(
                    { ...editingEvent, clubId: editingEvent.clubId },
                    getAccessToken(auth),
                    () => setEditingEvent(undefined)
                )
            );
        }
    };
    const deleteEvent = (event: Event) => {
        dispatch(DeleteEvent(event.eventId, getAccessToken(auth)));
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
                deleteEvent={deleteEvent}
            />
            <Button
                onClick={() =>
                    setEditingEvent({
                        clubId: clubId,
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
                        eventType: -1,
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

export default RouteParamsParser<
    Override<
        Props,
        {
            readonly eventId: string | undefined;
        }
    >,
    Props
>(({ eventId, ...props }) => ({
    ...props,
    eventId: eventId ? Number.parseInt(eventId) : undefined,
}))(Events);
