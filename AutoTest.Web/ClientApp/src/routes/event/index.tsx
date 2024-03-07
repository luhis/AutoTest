import { FunctionalComponent, h } from "preact";
import { useCallback, useEffect, useState } from "preact/hooks";
import { Button, Heading, Panel } from "react-bulma-components";
import { useSelector } from "react-redux";
import { newValidDate } from "ts-date";
import UUID from "uuid-int";
import { FaBell } from "react-icons/fa";
import save from "save-file";
import { route } from "preact-router";

import { getAccessToken } from "../../api/api";
import {
  GetEventsIfRequired,
  GetNotifications,
  CreateNotification,
  SetEventStatus,
} from "../../store/event/actions";
import { selectEvents, selectNotifications } from "../../store/event/selectors";
import { findIfLoaded, mapOrDefault } from "../../types/loadingState";
import NotificationsModal from "../../components/events/NotificationsModal";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import { EventNotification, EventStatus, Override } from "../../types/models";
import AddNotificationModal from "../../components/events/AddNotificationModal";
import { keySeed } from "../../settings";
import Breadcrumbs from "../../components/shared/Breadcrumbs";
import { getDateString } from "../../lib/date";
import { selectClubs } from "../../store/clubs/selectors";
import { GetClubsIfRequired } from "../../store/clubs/actions";
import { useThunkDispatch } from "../../store";
import { selectAccess, selectAccessToken } from "../../store/profile/selectors";
import { getMaps, getRegs } from "../../api/events";

const uid = UUID(keySeed);

interface Props {
  readonly eventId: number;
}

const Event: FunctionalComponent<Props> = ({ eventId }) => {
  const dispatch = useThunkDispatch();
  const auth = useSelector(selectAccessToken);
  const currentEvent = findIfLoaded(
    useSelector(selectEvents),
    (a) => a.eventId === eventId,
  );
  const currentClub = findIfLoaded(
    useSelector(selectClubs),
    (a) => a.clubId === currentEvent?.clubId,
  );
  const notifications = useSelector(selectNotifications);
  useEffect(() => {
    void dispatch(GetEventsIfRequired());
    void dispatch(GetClubsIfRequired(getAccessToken(auth)));
  }, [auth, dispatch]);
  useEffect(() => {
    void dispatch(GetNotifications(eventId));
  }, [dispatch, eventId]);
  const [showModal, setShowModal] = useState(false);
  const [showAddNotificationModal, setShowAddNotificationModal] = useState<
    EventNotification | undefined
  >(undefined);
  const saveButton = useCallback(async () => {
    if (showAddNotificationModal) {
      const res = await dispatch(
        CreateNotification(showAddNotificationModal, getAccessToken(auth)),
      );
      if (res) {
        setShowAddNotificationModal(undefined);
      }
    }
  }, [auth, dispatch, showAddNotificationModal]);
  const saveRegs = useCallback(async () => {
    if (currentEvent) {
      const data = await getRegs(eventId);
      return save(
        data,
        `${currentEvent.location}-${getDateString(
          currentEvent.startTime,
        )}-regs.pdf`,
      );
    } else {
      return Promise.resolve();
    }
  }, [currentEvent, eventId]);
  const saveMaps = useCallback(async () => {
    if (currentEvent) {
      const data = await getMaps(eventId);
      return save(
        data,
        `${currentEvent.location}-${getDateString(
          currentEvent.startTime,
        )}-maps.pdf`,
      );
    } else {
      return Promise.resolve();
    }
  }, [currentEvent]);
  const { adminClubs, marshalEvents } = useSelector(selectAccess);
  const canNotEdit =
    currentEvent === undefined || !adminClubs.includes(currentEvent.clubId);

  const previousStatus = useCallback(() => {
    if (currentEvent) {
      return dispatch(
        SetEventStatus(
          currentEvent.eventId,
          currentEvent.eventStatus - 1,
          getAccessToken(auth),
        ),
      );
    } else {
      return Promise.resolve();
    }
  }, [auth, currentEvent, dispatch]);
  const nextStatus = useCallback(() => {
    if (currentEvent) {
      return dispatch(
        SetEventStatus(
          currentEvent.eventId,
          currentEvent.eventStatus + 1,
          getAccessToken(auth),
        ),
      );
    } else {
      return Promise.resolve();
    }
  }, [auth, currentEvent, dispatch]);
  return (
    <div>
      <Breadcrumbs club={currentClub} event={currentEvent} />
      <Heading>Event {currentEvent?.location}</Heading>
      {!canNotEdit ? (
        <Panel>
          <Panel.Header>Event Status</Panel.Header>
          <Panel.Block>
            <Button.Group>
              <Button
                onClick={previousStatus}
                disabled={currentEvent.eventStatus === EventStatus.Open}
              >
                Back
              </Button>
              {EventStatus[currentEvent.eventStatus]}
              <Button
                onClick={nextStatus}
                disabled={currentEvent.eventStatus === EventStatus.Finalised}
              >
                Forward
              </Button>
            </Button.Group>
          </Panel.Block>
        </Panel>
      ) : null}

      <Panel>
        <Panel.Header>Notifications</Panel.Header>
        <Panel.Block>
          <Button.Group>
            <Button
              onClick={() => setShowModal(true)}
              loading={mapOrDefault(notifications, (_) => false, true)}
            >
              <FaBell />
              &nbsp;
              {mapOrDefault(
                notifications,
                (loadedNotifications) => loadedNotifications.length,
                0,
              )}
            </Button>
            <Button
              disabled={canNotEdit}
              onClick={() =>
                setShowAddNotificationModal({
                  eventId,
                  notificationId: uid.uuid(),
                  created: newValidDate(),
                  message: "",
                })
              }
            >
              Add Notification
            </Button>
          </Button.Group>
        </Panel.Block>
      </Panel>
      <Panel>
        <Panel.Header>People</Panel.Header>
        <Panel.Block>
          <Button.Group>
            <Button onClick={() => route(`/entrants/${eventId}`)}>
              Entrants
            </Button>
            <Button onClick={() => route(`/marshals/${eventId}`)}>
              Marshals
            </Button>
          </Button.Group>
        </Panel.Block>
      </Panel>
      <Panel>
        <Panel.Header>Times</Panel.Header>
        <Panel.Block>
          <Button.Group>
            <Button
              disabled={!marshalEvents.includes(eventId)}
              onClick={() => route(`/tests/${eventId}`)}
            >
              Tests
            </Button>
            <Button
              disabled={canNotEdit}
              onClick={() => route(`/editRuns/${eventId}`)}
            >
              Edit Runs
            </Button>
          </Button.Group>
        </Panel.Block>
      </Panel>
      <Panel>
        <Panel.Header>Results</Panel.Header>
        <Panel.Block>
          <Button.Group>
            <Button onClick={() => route(`/results/${eventId}`)}>
              Results
            </Button>
            <Button onClick={() => route(`/liveRuns/${eventId}`)}>
              Live Runs
            </Button>
          </Button.Group>
        </Panel.Block>
      </Panel>
      <Panel>
        <Panel.Header>Downloads</Panel.Header>
        <Panel.Block>
          <Button.Group>
            <Button
              disabled={currentEvent?.regulations === ""}
              onClick={saveRegs}
            >
              Regs
            </Button>
            <Button disabled={currentEvent?.maps === ""} onClick={saveMaps}>
              Maps
            </Button>
          </Button.Group>
        </Panel.Block>
      </Panel>
      {showAddNotificationModal ? (
        <AddNotificationModal
          cancel={() => setShowAddNotificationModal(undefined)}
          setField={(a: Partial<EventNotification>) =>
            setShowAddNotificationModal(
              (b) => ({ ...b, ...a }) as EventNotification,
            )
          }
          save={saveButton}
          notification={showAddNotificationModal}
        />
      ) : null}
      {showModal && notifications.tag === "Loaded" ? (
        <NotificationsModal
          cancel={() => setShowModal(false)}
          notifications={notifications.value}
        />
      ) : null}
    </div>
  );
};

export default RouteParamsParser<
  Override<
    Props,
    {
      readonly eventId: string;
    }
  >,
  Props
>(({ eventId, ...props }) => ({ ...props, eventId: Number.parseInt(eventId) }))(
  Event,
);
