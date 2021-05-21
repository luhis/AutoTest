import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Form, Heading } from "react-bulma-components";
import { useDispatch, useSelector } from "react-redux";
import { range } from "@s-libs/micro-dash";

import { Override } from "../../types/models";
import { findIfLoaded } from "../../types/loadingState";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import { selectEvents } from "../../store/event/selectors";
import { GetEventsIfRequired } from "../../store/event/actions";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import Breadcrumbs from "../../components/shared/Breadcrumbs";
import { selectClubs } from "../../store/clubs/selectors";
import { GetClubsIfRequired } from "../../store/clubs/actions";
import { OnSelectChange } from "../../types/inputs";
import { getTestRuns } from "../../api/testRuns";

interface Props {
    readonly eventId: number;
}

const EditRuns: FunctionalComponent<Props> = ({ eventId }) => {
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
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
        dispatch(GetEventsIfRequired());
    }, [dispatch, auth]);

    const [ordinal, setSelectedOrdinal] =
        useState<number | undefined>(undefined);
    useEffect(() => {
        if (ordinal) {
            void getTestRuns(eventId, ordinal, getAccessToken(auth));
        }
    }, [auth, eventId, ordinal]);
    return (
        <div>
            <Breadcrumbs club={currentClub} event={currentEvent} />
            <Heading>Edit Runs</Heading>
            <Form.Select<number>
                value={ordinal}
                onChange={(a: OnSelectChange) =>
                    setSelectedOrdinal(Number.parseInt(a.target.value))
                }
            >
                {(currentEvent
                    ? range(currentEvent.testCount).map((a) => a.toString())
                    : []
                ).map((a) => (
                    <option key={a} value={a}>
                        {a}
                    </option>
                ))}
            </Form.Select>
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
>(({ eventId, ...props }) => ({
    ...props,
    eventId: Number.parseInt(eventId),
}))(EditRuns);
