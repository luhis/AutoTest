import { FunctionalComponent, h } from "preact";
import { useEffect } from "preact/hooks";
import { route } from "preact-router";
import { Heading, Columns, Button, Loader } from "react-bulma-components";
import { useDispatch, useSelector } from "react-redux";
const { Column } = Columns;

import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import {
    GetEntrantsIfRequired,
    GetEventsIfRequired,
} from "../../store/event/actions";
import { selectEvents } from "../../store/event/selectors";
import { findIfLoaded } from "../../types/loadingState";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import { Override } from "../../types/models";
import Breadcrumbs from "../../components/shared/Breadcrumbs";
import { selectClubs } from "../../store/clubs/selectors";
import { GetClubsIfRequired } from "../../store/clubs/actions";

interface Props {
    readonly eventId: number;
}

const Tests: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
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
    useEffect(() => {
        dispatch(GetEntrantsIfRequired(eventId));
    }, [eventId, dispatch, auth]);
    useEffect(() => {
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
        dispatch(GetEventsIfRequired());
    }, [dispatch, auth]);
    return (
        <div>
            <Breadcrumbs club={currentClub} event={currentEvent} />
            <Heading>Tests</Heading>
            {currentEvent ? (
                currentEvent.tests.map(({ ordinal }) => (
                    <Columns key={ordinal}>
                        <Column>
                            <p class="number">{ordinal + 1}</p>
                        </Column>
                        <Column>
                            <Button.Group>
                                <Button
                                    onClick={() =>
                                        route(`/marshal/${eventId}/${ordinal}`)
                                    }
                                >
                                    Marshal
                                </Button>
                            </Button.Group>
                        </Column>
                    </Columns>
                ))
            ) : (
                <div>
                    Loading... <Loader />
                </div>
            )}
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
    Tests
);
