import { FunctionalComponent, FunctionComponent, h } from "preact";
import { useEffect, useMemo, useState } from "preact/hooks";
import { Heading } from "react-bulma-components";
import { useSelector } from "react-redux";
import {
    HubConnectionBuilder,
    LogLevel,
    HubConnection,
} from "@microsoft/signalr";
import { compact, last, sortBy, identity } from "@s-libs/micro-dash";
import { route } from "preact-router";

import { Override, PublicEntrant, TestRunFromServer } from "../../types/models";
import { findIfLoaded, LoadingState } from "../../types/loadingState";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import { selectEntrants, selectEvents } from "../../store/event/selectors";
import { GetEventsIfRequired } from "../../store/event/actions";
import Breadcrumbs from "../../components/shared/Breadcrumbs";
import { selectClubs } from "../../store/clubs/selectors";
import { GetClubsIfRequired } from "../../store/clubs/actions";
import FilterDropdown from "../../components/shared/FilterDropdown";
import Penalties from "../../components/shared/Penalties";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import { useThunkDispatch } from "../../store";

interface Props {
    readonly eventId: number;
    readonly testFilter: readonly number[];
    readonly connection: HubConnection | undefined;
}

const getEntrantName = (
    currentEntrants: LoadingState<readonly PublicEntrant[], number>,
    entrantId: number
) => {
    const found = findIfLoaded(
        currentEntrants,
        (a) => a.entrantId === entrantId
    );
    return found ? `${found.givenName} ${found.familyName}` : "Not Found";
};

const baseConn = new HubConnectionBuilder()
    .withUrl("/resultsHub")
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Debug);

const Results: FunctionalComponent<Props> = ({
    eventId,
    testFilter,
    connection,
}) => {
    const thunkDispatch = useThunkDispatch();
    const auth = useGoogleAuth();
    const currentEvent = findIfLoaded(
        useSelector(selectEvents),
        (a) => a.eventId === eventId
    );
    const currentEntrants = useSelector(selectEntrants);
    const currentClub = findIfLoaded(
        useSelector(selectClubs),
        (a) => a.clubId === currentEvent?.clubId
    );
    const [runs, setRun] = useState<readonly TestRunFromServer[]>([]);
    useEffect(() => {
        thunkDispatch(GetClubsIfRequired(getAccessToken(auth)));
        void thunkDispatch(GetEventsIfRequired());
    }, [thunkDispatch, auth]);
    const [testFilterState, setTestFilterState] =
        useState<readonly number[]>(testFilter);
    useEffect(() => {
        route(
            `/liveRuns/${eventId}?testFilter=${testFilterState.join(",")}`,
            false
        );
    }, [testFilterState, eventId]);
    useEffect(() => {
        if (connection) {
            connection.on("NewTestRun", (newRun: TestRunFromServer) => {
                setRun((a) => a.concat(newRun));
            });
        }
    }, [connection]);

    const filterRuns = (r: TestRunFromServer) =>
        testFilterState.length === 0 || testFilterState.includes(r.ordinal);

    const allTests = currentEvent
        ? currentEvent.tests.map((a) => a.ordinal + 1)
        : [];

    const currentRun = last(runs.filter(filterRuns));
    return (
        <div>
            <Breadcrumbs club={currentClub} event={currentEvent} />
            <Heading>Live Runs</Heading>
            <FilterDropdown
                filterName="Test"
                options={allTests}
                selected={sortBy(testFilterState, identity)}
                setFilter={setTestFilterState}
            />
            {currentRun ? (
                <p>
                    {getEntrantName(currentEntrants, currentRun.entrantId)}:{" "}
                    {(currentRun.timeInMS / 1_000).toFixed(2)}s
                    <Penalties penalties={currentRun.penalties} />
                </p>
            ) : null}
        </div>
    );
};

interface OtherProps {
    readonly matches: {
        readonly eventId: number;
        readonly testFilter: readonly number[];
    };
}
const SignalRWrapper: FunctionComponent<OtherProps> = ({ matches }) => {
    const connection = useMemo(
        () => (typeof window !== "undefined" ? baseConn.build() : undefined),
        []
    );

    useEffect(() => {
        if (connection) {
            void connection
                .start()
                .then(() => {
                    void connection.invoke("ListenToEvent", matches.eventId);
                })
                .catch(console.error);
            return () => {
                const f = async () => {
                    await connection.invoke("LeaveEvent", matches.eventId);
                    await connection.stop();
                };
                void f();
            };
        } else {
            return () => undefined;
        }
    }, [connection, matches.eventId]);
    return (
        <Results
            eventId={matches.eventId}
            testFilter={matches.testFilter}
            connection={connection}
        />
    );
};

export default RouteParamsParser<
    Override<
        OtherProps,
        {
            readonly matches: {
                readonly eventId: string;
                readonly testFilter: string;
            };
        }
    >,
    OtherProps
>(({ matches, ...props }) => ({
    ...props,
    matches: {
        eventId: Number.parseInt(matches.eventId),
        testFilter: matches.testFilter
            ? compact(
                  matches.testFilter.split(",").map((a) => Number.parseInt(a))
              )
            : [],
    },
}))(SignalRWrapper);
