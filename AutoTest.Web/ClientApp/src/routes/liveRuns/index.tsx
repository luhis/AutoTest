import { FunctionalComponent, h } from "preact";
import { useEffect, useMemo, useState } from "preact/hooks";
import { Heading } from "react-bulma-components";
import { useDispatch, useSelector } from "react-redux";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { compact, last } from "@s-libs/micro-dash";
// import { route } from "preact-router";

import { Override, TestRunFromServer } from "../../types/models";
import { findIfLoaded } from "../../types/loadingState";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import { selectEntrants, selectEvents } from "../../store/event/selectors";
import { GetEventsIfRequired } from "../../store/event/actions";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import Breadcrumbs from "../../components/shared/Breadcrumbs";
import { selectClubs } from "../../store/clubs/selectors";
import { GetClubsIfRequired } from "../../store/clubs/actions";
import FilterDropdown from "../../components/shared/FilterDropdown";
import Penalties from "../../components/shared/Penalties";

interface Props {
    readonly eventId: number;
    readonly testFilter: readonly string[];
}

const baseConn = new HubConnectionBuilder()
    .withUrl("/resultsHub")
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Debug);

const Results: FunctionalComponent<Props> = ({ eventId, testFilter }) => {
    const connection = useMemo(
        () => (typeof window !== "undefined" ? baseConn.build() : undefined),
        []
    );
    const dispatch = useDispatch();
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
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
        dispatch(GetEventsIfRequired());
    }, [dispatch, auth]);
    const [testFilterState, setTestFilterState] =
        useState<readonly string[]>(testFilter);
    // useEffect(() => {
    //     route(
    //         `/liveRuns/${eventId}?testFilter=${testFilterState.join(",")}`,
    //         false
    //     );
    // }, [testFilterState, eventId]);

    const filterRuns = (r: TestRunFromServer) =>
        testFilterState.length === 0 ||
        testFilterState.includes(r.ordinal.toString());

    useEffect(() => {
        if (connection) {
            connection.on("NewTestRun", (newRun: TestRunFromServer) => {
                setRun((a) => a.concat(newRun));
            });
            void connection
                .start()
                .then(() => {
                    void connection.invoke("ListenToEvent", eventId);
                })
                .catch(console.error);
            return () => {
                debugger;
                const f = async () => {
                    await connection.invoke("LeaveEvent", eventId);
                    await connection.stop();
                };
                void f();
            };
        } else {
            return () => undefined;
        }
    }, [connection, dispatch, eventId]);

    const allTests = currentEvent
        ? currentEvent.tests.map((a) => a.ordinal.toString())
        : [];

    const getEntrantName = (entrantId: number) => {
        const found = findIfLoaded(
            currentEntrants,
            (a) => a.entrantId === entrantId
        );
        return found ? `${found.givenName} ${found.familyName}` : "Not Found";
    };

    const currentRun = last(runs.filter(filterRuns));
    return (
        <div>
            <Breadcrumbs club={currentClub} event={currentEvent} />
            <Heading>Live Runs</Heading>
            <FilterDropdown
                filterName="Test"
                options={allTests}
                selected={testFilterState}
                setFilter={setTestFilterState}
            />
            {currentRun ? (
                <p>
                    {getEntrantName(currentRun.entrantId)}:{" "}
                    {(currentRun.timeInMS / 1_000).toFixed(2)}s
                    <Penalties penalties={currentRun.penalties} />
                </p>
            ) : null}
        </div>
    );
};

export default RouteParamsParser<
    Override<
        Props,
        {
            readonly eventId: string;
            readonly testFilter: string;
        }
    >,
    Props
>(({ eventId, testFilter, ...props }) => ({
    ...props,
    eventId: Number.parseInt(eventId),
    testFilter: testFilter ? compact(testFilter.split(",")) : [],
}))(Results);
