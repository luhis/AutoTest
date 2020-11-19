import { FunctionalComponent, h } from "preact";
import { useEffect } from "preact/hooks";
import { route } from "preact-router";
import { Title, Column, Button, Numeric, Loader, Breadcrumb } from "rbx";
import { useDispatch, useSelector } from "react-redux";

import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import { GetEntrants, GetEventsIfRequired } from "../../store/event/actions";
import { selectEvents, selectClubs } from "../../store/event/selectors";
import { findIfLoaded } from "../../types/loadingState";

interface Props {
    readonly eventId: string;
}

const Tests: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const dispatch = useDispatch();
    const auth = useGoogleAuth();
    const eventIdAsNum = Number.parseInt(eventId);
    const currentEvent = findIfLoaded(
        useSelector(selectEvents),
        (a) => a.eventId === eventIdAsNum
    );
    const currentClub = findIfLoaded(
        useSelector(selectClubs),
        (a) => a.clubId === currentEvent?.clubId
    );
    useEffect(() => {
        dispatch(GetEntrants(eventIdAsNum, getAccessToken(auth)));
    }, [eventIdAsNum, dispatch, auth]);
    useEffect(() => {
        dispatch(GetEventsIfRequired());
    }, [eventIdAsNum, dispatch, auth]);
    return (
        <div>
            <Breadcrumb>
                <Breadcrumb.Item
                    href={`/events?clubId=${currentClub?.clubId || 0}`}
                >
                    {currentClub?.clubName}
                </Breadcrumb.Item>
                <Breadcrumb.Item>{currentEvent?.location}</Breadcrumb.Item>
            </Breadcrumb>
            <Title>Tests</Title>
            {currentEvent ? (
                currentEvent.tests.map((a) => (
                    <Column.Group key={a.ordinal}>
                        <Column>
                            <Numeric>{a.ordinal + 1}</Numeric>
                        </Column>
                        <Column>
                            <Button.Group>
                                <Button
                                    onClick={() =>
                                        route(
                                            `/marshal/${eventId}/${a.ordinal}`
                                        )
                                    }
                                >
                                    Marshal
                                </Button>
                            </Button.Group>
                        </Column>
                    </Column.Group>
                ))
            ) : (
                <span>
                    Loading... <Loader />
                </span>
            )}
        </div>
    );
};

export default Tests;
