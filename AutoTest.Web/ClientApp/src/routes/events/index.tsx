import { FunctionalComponent, h } from "preact";
import { useCallback, useEffect, useState } from "preact/hooks";
import { Heading, Button } from "react-bulma-components";
import UUID from "uuid-int";
import { newValidDate, newValidDateOrThrow } from "ts-date";
import { useSelector } from "react-redux";

import {
  Event,
  EditingEvent,
  Override,
  TimingSystem,
  EventStatus,
} from "../../types/models";
import Modal from "../../components/events/Modal";
import { getAccessToken } from "../../api/api";
import List from "../../components/events/List";
import {
  GetEventsIfRequired,
  AddEvent,
  DeleteEvent,
} from "../../store/event/actions";
import { selectEvents } from "../../store/event/selectors";
import { keySeed } from "../../settings";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import { selectClubs } from "../../store/clubs/selectors";
import { GetClubsIfRequired } from "../../store/clubs/actions";
import { selectAccess, selectAccessToken } from "../../store/profile/selectors";
import { useThunkDispatch } from "../../store";
import { getDateTimeString } from "../../lib/date";

interface Props {
  readonly clubId: number | undefined;
}
const uid = UUID(keySeed);

const blankEvent = (clubId: number | undefined): EditingEvent => ({
  clubId: clubId,
  eventId: uid.uuid(),
  location: "",
  startTime: getDateTimeString(newValidDate()),
  courseCount: 12,
  maxAttemptsPerCourse: 2,
  maxEntrants: 30,
  isNew: true,
  isClubEditable: clubId === undefined,
  courses: [],
  regulations: null,
  maps: null,
  eventTypes: [],
  entryOpenDate: getDateTimeString(newValidDate()),
  entryCloseDate: getDateTimeString(newValidDate()),
  timingSystem: TimingSystem.App,
  eventStatus: EventStatus.Open,
});

const Events: FunctionalComponent<Props> = ({ clubId }) => {
  const thunkDispatch = useThunkDispatch();
  const auth = useSelector(selectAccessToken);
  const events = useSelector(selectEvents);
  const clubs = useSelector(selectClubs);
  const { adminClubs } = useSelector(selectAccess);
  const canAdmin = (a: number) => adminClubs.includes(a);
  const [editingEvent, setEditingEvent] = useState<EditingEvent | undefined>(
    undefined,
  );
  useEffect(() => {
    void thunkDispatch(GetEventsIfRequired());
    thunkDispatch(GetClubsIfRequired(getAccessToken(auth)));
  }, [auth, thunkDispatch]);
  const dispatchThunk = useThunkDispatch();
  const save = useCallback(async () => {
    if (editingEvent && editingEvent.clubId) {
      await dispatchThunk(
        AddEvent(
          {
            ...editingEvent,
            clubId: editingEvent.clubId,
            startTime: newValidDateOrThrow(editingEvent.startTime),
            entryOpenDate: newValidDateOrThrow(editingEvent.entryOpenDate),
            entryCloseDate: newValidDateOrThrow(editingEvent.entryCloseDate),
            created: newValidDate(),
          },
          getAccessToken(auth),
          () => setEditingEvent(undefined),
        ),
      );
    }
  }, [auth, dispatchThunk, editingEvent]);
  const deleteEvent = useCallback(
    async (event: Event) => {
      await dispatchThunk(DeleteEvent(event.eventId, getAccessToken(auth)));
    },
    [auth, dispatchThunk],
  );
  const createNewEvent = useCallback(
    () => setEditingEvent(blankEvent(clubId)),
    [clubId],
  );
  return (
    <div>
      <Heading>Events</Heading>
      <List
        canAdmin={canAdmin}
        events={events}
        setEditingEvent={(a) =>
          setEditingEvent({
            ...a,
            isNew: false,
            isClubEditable: clubId === undefined,
            startTime: getDateTimeString(a.startTime),
            entryOpenDate: getDateTimeString(a.entryOpenDate),
            entryCloseDate: getDateTimeString(a.entryCloseDate),
          })
        }
        deleteEvent={deleteEvent}
      />
      <Button
        color="primary"
        disabled={
          (clubId !== undefined && !adminClubs.includes(clubId)) ||
          (clubId === undefined && adminClubs.length === 0)
        }
        onClick={createNewEvent}
      >
        Add Event
      </Button>
      {editingEvent ? (
        <Modal
          event={editingEvent}
          clubs={clubs}
          setField={(a: Partial<EditingEvent>) => {
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
