import { FunctionalComponent, h } from "preact";
import { useEffect } from "preact/hooks";
import { route } from "preact-router";
import { Title, Column, Button, Numeric, Loader } from "rbx";
import { useDispatch, useSelector } from "react-redux";

import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import { GetEntrants, GetEventsIfRequired } from "../../store/event/actions";
import { selectEvents } from "../../store/event/selectors";
import EventTitle from "../../components/shared/EventTitle";

interface Props {
    eventId: string;
}

const Tests: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const dispatch = useDispatch();
    const auth = useGoogleAuth();
    const events = useSelector(selectEvents);
    const eventIdAsNum = Number.parseInt(eventId);
    const currentEvent =
        events.tag === "Loaded"
            ? events.value.find((a) => a.eventId == eventIdAsNum)
            : undefined;
    useEffect(() => {
        dispatch(GetEntrants(eventIdAsNum, getAccessToken(auth)));
    }, [eventIdAsNum, dispatch, auth]);
    useEffect(() => {
        dispatch(GetEventsIfRequired());
    }, [eventIdAsNum, dispatch, auth]);
    return (
        <div>
            <Title>
                Tests - <EventTitle currentEvent={currentEvent} />
            </Title>
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
