import { FunctionalComponent, h } from "preact";
import { useCallback, useEffect, useState } from "preact/hooks";
import { Button, Heading } from "react-bulma-components";
import { useDispatch, useSelector } from "react-redux";
import { newValidDate } from "ts-date";
import UUID from "uuid-int";
import { FaBell } from "react-icons/fa";

import { getAccessToken } from "../../api/api";
import { useGoogleAuth } from "../../components/app";
import {
    GetEventsIfRequired,
    GetClubsIfRequired,
    GetNotifications,
    CreateNotification,
} from "../../store/event/actions";
import {
    selectClubs,
    selectEvents,
    selectNotifications,
} from "../../store/event/selectors";
import { findIfLoaded, mapOrDefault } from "../../types/loadingState";
import NotificationsModal from "../../components/events/NotificationsModal";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import { Notification, Override } from "../../types/models";
import AddNotificationModal from "../../components/events/AddNotificationModal";
import { keySeed } from "../../settings";
import Breadcrumbs from "../../components/shared/Breadcrumbs";

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
    const save = useCallback(() => {
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
    return (
        <div>
            <Breadcrumbs club={currentClub} event={currentEvent} />
            <Heading>Event {currentEvent?.location}</Heading>

            <Button
                onClick={() => setShowModal(true)}
                loading={mapOrDefault(notifications, (_) => false, true)}
            >
                <FaBell />
                &nbsp;
                {mapOrDefault(
                    notifications,
                    (loadedNotifications) =>
                        loadedNotifications.length.toString(),
                    ""
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
            {showAddNotificationModal && notifications.tag === "Loaded" ? (
                <AddNotificationModal
                    cancel={() => setShowAddNotificationModal(undefined)}
                    setField={(a: Partial<Notification>) =>
                        setShowAddNotificationModal(
                            (b) => ({ ...b, ...a } as Notification)
                        )
                    }
                    save={save}
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
