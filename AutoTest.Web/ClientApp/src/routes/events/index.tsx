import { FunctionalComponent, h } from "preact";
import { useCallback, useEffect, useState } from "preact/hooks";
import { Heading, Button } from "react-bulma-components";
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
    const [editingEvent, setEditingEvent] =
        useState<EditingEvent | undefined>(undefined);
    useEffect(() => {
        dispatch(GetEventsIfRequired());
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
    }, [auth, dispatch]);
    const save = useCallback(() => {
        if (editingEvent && editingEvent.clubId) {
            dispatch(
                AddEvent(
                    { ...editingEvent, clubId: editingEvent.clubId },
                    getAccessToken(auth),
                    () => setEditingEvent(undefined)
                )
            );
        }
    }, [auth, dispatch, editingEvent]);
    const deleteEvent = useCallback(
        (event: Event) => {
            dispatch(DeleteEvent(event.eventId, getAccessToken(auth)));
        },
        [auth, dispatch]
    );
    const createNewEvent = useCallback(
        () =>
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
                eventType: Number.NaN,
            }),
        [clubId]
    );
    return (
        <div>
            <Heading>Events</Heading>
            <List
                events={events}
                setEditingEvent={(a) =>
                    setEditingEvent({
                        ...a,
                        isNew: false,
                        isClubEditable: clubId === undefined,
                    })
                }
                deleteEvent={deleteEvent}
            />
            <Button onClick={createNewEvent}>Add Event</Button>
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
        </div>
    );
};

export default RouteParamsParser<
    Override<
        Props,
        {
            readonly clubId: string | undefined;
        }
    >,
    Props
>(({ clubId, ...props }) => ({
    ...props,
    clubId: clubId ? Number.parseInt(clubId) : undefined,
}))(Events);
