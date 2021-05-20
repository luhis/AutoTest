import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Heading } from "react-bulma-components";
import { useDispatch, useSelector } from "react-redux";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { compact, last } from "@s-libs/micro-dash";
import { route } from "preact-router";
import { FaExclamation } from "react-icons/fa";

import { Override, PenaltyType, TestRun } from "../../types/models";
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
import { startCase } from "../../lib/string";

interface Props {
    readonly eventId: number;
    readonly testFilter: readonly string[];
}

const Results: FunctionalComponent<Props> = ({ eventId, testFilter }) => {
    const connection =
        typeof window !== "undefined"
            ? new HubConnectionBuilder()
                  .withUrl("/resultsHub")
                  .withAutomaticReconnect()
                  .configureLogging(LogLevel.Debug)
                  .build()
            : undefined;
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
    const [runs, setRun] = useState<readonly TestRun[]>([]);
    useEffect(() => {
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
        dispatch(GetEventsIfRequired());
    }, [dispatch, auth]);
    const [testFilterState, setTestFilterState] =
        useState<readonly string[]>(testFilter);
    useEffect(() => {
        route(
            `/liveruns/${eventId}?testFilter=${testFilterState.join(",")}`,
            false
        );
    }, [testFilterState, eventId]);

    const filterRuns = (r: TestRun) =>
        testFilterState.length === 0 ||
        testFilterState.includes(r.ordinal.toString());

    useEffect(() => {
        if (connection) {
            connection.on("NewTestRun", (newRun: TestRun) => {
                setRun((a) => a.concat(newRun));
            });
            void connection
                .start()
                .then(() => {
                    void connection.invoke("ListenToEvent", eventId);
                })
                .catch(console.error);
            return async () => {
                await connection.invoke("LeaveEvent", eventId);
                await connection.stop();
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
        return found ? `${found.givenName} ${found.familyName}` : "";
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
                    {(currentRun.timeInMS / 1_000).toFixed(2)}
                    {currentRun.penalties.length > 0 ? <FaExclamation /> : null}
                    {currentRun.penalties.map((p) => (
                        <p key={p.penaltyType}>
                            {p.instanceCount} X{" "}
                            {startCase(PenaltyType[p.penaltyType])}
                        </p>
                    ))}
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
