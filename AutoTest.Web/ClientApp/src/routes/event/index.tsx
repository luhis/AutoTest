import { FunctionalComponent, h } from "preact";
import { useCallback, useEffect, useState } from "preact/hooks";
import { Button, Heading } from "react-bulma-components";
import { useDispatch, useSelector } from "react-redux";
import { newValidDate } from "ts-date";
import UUID from "uuid-int";
import { FaBell } from "react-icons/fa";
import save from "save-file";
import { route } from "preact-router";

import { getAccessToken } from "../../api/api";
import { useGoogleAuth } from "../../components/app";
import {
    GetEventsIfRequired,
    GetNotifications,
    CreateNotification,
} from "../../store/event/actions";
import { selectEvents, selectNotifications } from "../../store/event/selectors";
import { findIfLoaded, mapOrDefault } from "../../types/loadingState";
import NotificationsModal from "../../components/events/NotificationsModal";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import { Notification, Override } from "../../types/models";
import AddNotificationModal from "../../components/events/AddNotificationModal";
import { keySeed } from "../../settings";
import Breadcrumbs from "../../components/shared/Breadcrumbs";
import { getDateString } from "../../lib/date";
import { selectClubs } from "../../store/clubs/selectors";
import { GetClubsIfRequired } from "../../store/clubs/actions";

const uid = UUID(keySeed);

interface Props {
    readonly eventId: number;
}

const Event: FunctionalComponent<Props> = ({ eventId }) => {
    const dispatch = useDispatch();
    const auth = useGoogleAuth();
    const currentEvent = findIfLoaded(
        useSelector(selectEvents),
        (a) => a.eventId === eventId
    );
    const currentClub = findIfLoaded(
        useSelector(selectClubs),
        (a) => a.clubId === currentEvent?.clubId
    );
    const notifications = useSelector(selectNotifications);
    useEffect(() => {
        dispatch(GetEventsIfRequired());
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
    }, [auth, dispatch]);
    useEffect(() => {
        dispatch(GetNotifications(eventId));
    }, [dispatch, eventId]);
    const [showModal, setShowModal] = useState(false);
    const [showAddNotificationModal, setShowAddNotificationModal] =
        useState<Notification | undefined>(undefined);
    const saveButton = useCallback(() => {
        if (showAddNotificationModal) {
            dispatch(
                CreateNotification(
                    showAddNotificationModal,
                    getAccessToken(auth)
                )
            );
            setShowAddNotificationModal(undefined);
        }
    }, [auth, dispatch, showAddNotificationModal]);
    const saveRegs = useCallback(
        () =>
            currentEvent
                ? save(
                      currentEvent.regulations,
                      `${currentEvent.location}-${getDateString(
                          currentEvent.startTime
                      )}-regs.pdf`
                  )
                : Promise.resolve(),
        [currentEvent]
    );
    const saveMaps = useCallback(
        () =>
            currentEvent
                ? save(
                      currentEvent.maps,
                      `${currentEvent.location}-${getDateString(
                          currentEvent.startTime
                      )}-maps.pdf`
                  )
                : Promise.resolve(),
        [currentEvent]
    );
    return (
        <div>
            <Breadcrumbs club={currentClub} event={currentEvent} />
            <Heading>Event {currentEvent?.location}</Heading>

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
                        0
                    )}
                </Button>
                <Button
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

            <Button.Group>
                <Button onClick={() => route(`/editRuns/${eventId}`)}>
                    Edit Runs
                </Button>
                <Button onClick={() => route(`/entrants/${eventId}`)}>
                    Entrants
                </Button>
                <Button onClick={() => route(`/marshals/${eventId}`)}>
                    Marshals
                </Button>
            </Button.Group>
            <Button onClick={() => route(`/tests/${eventId}`)}>Tests</Button>
            <Button.Group>
                <Button onClick={() => route(`/results/${eventId}`)}>
                    Results
                </Button>
                <Button onClick={() => route(`/liveRuns/${eventId}`)}>
                    Live Runs
                </Button>
            </Button.Group>
            <Button.Group>
                <Button onClick={saveRegs}>Regs</Button>
                <Button onClick={saveMaps}>Maps</Button>
            </Button.Group>
            {showAddNotificationModal ? (
                <AddNotificationModal
                    cancel={() => setShowAddNotificationModal(undefined)}
                    setField={(a: Partial<Notification>) =>
                        setShowAddNotificationModal(
                            (b) => ({ ...b, ...a } as Notification)
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
    Event
);
