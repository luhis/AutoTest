import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Numeric, Title } from "rbx";
import { useDispatch, useSelector } from "react-redux";

import { getAccessToken } from "../../api/api";
import { useGoogleAuth } from "../../components/app";
import {
    GetEventsIfRequired,
    GetClubsIfRequired,
    GetNotifications,
} from "../../store/event/actions";
import { selectEvents, selectNotifications } from "../../store/event/selectors";
import { findIfLoaded } from "../../types/loadingState";
import NotificationsModal from "../../components/events/NotificationsModal";

interface Props {
    readonly eventId: string;
}

const Events: FunctionalComponent<Props> = ({ eventId }) => {
    const dispatch = useDispatch();
    const eventIdNum = Number.parseInt(eventId);
    const auth = useGoogleAuth();
    const currentEvent = findIfLoaded(
        useSelector(selectEvents),
        (a) => a.eventId === eventIdNum
    );
    const notifications = useSelector(selectNotifications);
    useEffect(() => {
        dispatch(GetEventsIfRequired());
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
    }, [auth, dispatch]);
    useEffect(() => {
        dispatch(GetNotifications(eventIdNum));
    }, [dispatch, eventIdNum]);
    const [showModal, setShowModal] = useState(false);
    return (
        <div>
            <Title>Event {currentEvent?.location}</Title>
            <Numeric onClick={() => setShowModal(true)}>
                {notifications.tag === "Loaded"
                    ? notifications.value.length
                    : ""}
            </Numeric>
            {showModal && notifications.tag === "Loaded" ? (
                <NotificationsModal
                    cancel={() => setShowModal(false)}
                    notifications={notifications.value}
                />
            ) : null}
        </div>
    );
};

export default Events;
