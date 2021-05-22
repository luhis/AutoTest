import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Form, Heading, Table } from "react-bulma-components";
import { useDispatch, useSelector } from "react-redux";
import { range } from "@s-libs/micro-dash";

import { Override } from "../../types/models";
import { findIfLoaded } from "../../types/loadingState";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import {
    selectEntrants,
    selectEvents,
    selectMarshals,
    selectTestRunsFromServer,
} from "../../store/event/selectors";
import {
    GetEntrantsIfRequired,
    GetEventsIfRequired,
    GetMarshalsIfRequired,
    GetTestRunsIfRequired,
} from "../../store/event/actions";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import Breadcrumbs from "../../components/shared/Breadcrumbs";
import { selectClubs } from "../../store/clubs/selectors";
import { GetClubsIfRequired } from "../../store/clubs/actions";
import { OnSelectChange } from "../../types/inputs";
import ifSome from "../../components/shared/ifSome";
import Penalties from "../../components/shared/Penalties";

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

    const [ordinal, setSelectedOrdinal] = useState<number>(0);
    const testRuns = useSelector(selectTestRunsFromServer);
    useEffect(() => {
        dispatch(GetTestRunsIfRequired(eventId, ordinal, getAccessToken(auth)));
        dispatch(GetEntrantsIfRequired(eventId, getAccessToken(auth)));
        dispatch(GetMarshalsIfRequired(eventId, getAccessToken(auth)));
    }, [auth, dispatch, eventId, ordinal]);

    const entrants = useSelector(selectEntrants);

    const getEntrantName = (entrantId: number) => {
        const found = findIfLoaded(entrants, (a) => a.entrantId === entrantId);
        if (found) {
            return `${found.givenName} ${found.familyName}`;
        } else {
            return "Not Found";
        }
    };
    const currentMarshals = useSelector(selectMarshals);

    const getMarshalName = (marshalId: number) => {
        const found = findIfLoaded(
            currentMarshals,
            (a) => a.marshalId === marshalId
        );
        return found ? `${found.givenName} ${found.familyName}` : "Not Found";
    };
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
            <Table>
                <thead>
                    <tr>
                        <th>Test Run ID</th>
                        <th>Marshal</th>
                        <th>Entrant</th>
                        <th>Time</th>
                        <th>Penalties</th>
                        <th>Created</th>
                    </tr>
                </thead>
                {ifSome(
                    testRuns,
                    (r) => r.testRunId,
                    (result) => (
                        <tr key={result.testRunId}>
                            <td>{result.testRunId}</td>
                            <td>{getMarshalName(result.marshalId)}</td>
                            <td>{getEntrantName(result.entrantId)}</td>
                            <td>{(result.timeInMS / 1000).toFixed(2)}s</td>
                            <td>
                                <Penalties penalties={result.penalties} />
                            </td>
                            <td>{result.created.toUTCString()}</td>
                        </tr>
                    ),
                    (_) => true
                )}
            </Table>
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
