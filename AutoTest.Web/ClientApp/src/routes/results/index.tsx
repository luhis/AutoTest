import { FunctionalComponent, h, Fragment } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Heading, Table, Button, Dropdown } from "react-bulma-components";
import { useDispatch, useSelector } from "react-redux";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { compact, range } from "@s-libs/micro-dash";
import { newValidDate } from "ts-date";
import { FaBell } from "react-icons/fa";
import classNames from "classnames";
import { route } from "preact-router";

import { Notification, Override, Result } from "../../types/models";
import {
    LoadingState,
    findIfLoaded,
    mapOrDefault,
} from "../../types/loadingState";
import { getResults } from "../../api/results";
import ifSome from "../../components/shared/ifSome";
import { useGoogleAuth } from "../../components/app";
import Time from "../../components/results/Time";
import { getAccessToken } from "../../api/api";
import { selectEvents, selectNotifications } from "../../store/event/selectors";
import {
    AddNotification,
    GetEventsIfRequired,
    GetNotifications,
} from "../../store/event/actions";
import NotificationsModal from "../../components/events/NotificationsModal";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import Breadcrumbs from "../../components/shared/Breadcrumbs";
import DriverNumber from "../../components/shared/DriverNumber";
import { selectClubs } from "../../store/clubs/selectors";
import { GetClubsIfRequired } from "../../store/clubs/actions";

interface Props {
    readonly eventId: number;
    readonly filter: readonly string[];
}

const numberToChar = (n: number) => "abcdefghijklmnopqrstuvwxyz".charAt(n);
const connection =
    typeof window !== "undefined"
        ? new HubConnectionBuilder()
              .withUrl("/resultsHub")
              .withAutomaticReconnect()
              .configureLogging(LogLevel.Error)
              .build()
        : undefined;

const Results: FunctionalComponent<Props> = ({ eventId, filter }) => {
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
    const testRuns = range(
        currentEvent !== undefined ? currentEvent.maxAttemptsPerTest : 0
    );
    const [results, setResults] = useState<
        LoadingState<readonly Result[], number>
    >({
        tag: "Loading",
        id: eventId,
    });
    useEffect(() => {
        const fetchData = async () => {
            const resultsData = await getResults(eventId, getAccessToken(auth));
            setResults(resultsData);
        };
        void fetchData();
    }, [auth, eventId]);
    useEffect(() => {
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
        dispatch(GetEventsIfRequired());
    }, [dispatch, auth]);
    useEffect(() => {
        dispatch(GetNotifications(eventId));
    }, [eventId, dispatch]);

    useEffect(() => {
        if (connection) {
            connection.on("NewNotification", (notification: Notification) => {
                dispatch(AddNotification(notification));
            });
            connection.on("NewTestRun", (newResults: readonly Result[]) => {
                setResults({
                    tag: "Loaded",
                    value: newResults,
                    id: eventId,
                    loaded: newValidDate(),
                });
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
    }, [dispatch, eventId]);
    const [showModal, setShowModal] = useState(false);
    const [classFilter, setClassFilter] = useState<readonly string[]>(filter);
    useEffect(() => {
        route(`/results/${eventId}?filter=${classFilter.join(",")}`, true);
    }, [classFilter, eventId]);
    const allClasses = mapOrDefault(results, (a) => a.map((b) => b.class), []);

    return (
        <div>
            <Breadcrumbs club={currentClub} event={currentEvent} />
            <Heading>Results</Heading>
            <Button
                onClick={() => setShowModal(true)}
                loading={notifications.tag === "Loaded" ? false : true}
            >
                <FaBell />
                &nbsp;
                {notifications.tag === "Loaded"
                    ? notifications.value.length
                    : 0}
            </Button>
            <Dropdown
                label={`Class Filter: ${
                    classFilter.length === 0 ? "All" : classFilter.join(", ")
                }`}
            >
                <a
                    href="#"
                    class={classNames("dropdown-item", {
                        "is-active": classFilter.length === 0,
                    })}
                    onClick={() => setClassFilter([])}
                >
                    All
                </a>
                <hr class="dropdown-divider" />
                {allClasses.map((c) => (
                    <a
                        key={c}
                        href="#"
                        class={classNames("dropdown-item", {
                            "is-active": classFilter.includes(c),
                        })}
                        onClick={() =>
                            setClassFilter((f) =>
                                f.includes(c)
                                    ? filter.filter((a) => a != c)
                                    : filter.concat(c)
                            )
                        }
                    >
                        {c}
                    </a>
                ))}
            </Dropdown>
            <Table>
                <thead>
                    <tr>
                        <th>Class</th>
                        <th>Number</th>
                        <th>Name</th>
                        <th>Total Time</th>
                        {currentEvent
                            ? currentEvent.tests.map((test) =>
                                  testRuns.map((run) => (
                                      <th key={`${test.ordinal}.${run}`}>
                                          {test.ordinal + 1}.{numberToChar(run)}
                                      </th>
                                  ))
                              )
                            : null}
                        <th>Class</th>
                        <th>Overall</th>
                    </tr>
                </thead>
                {ifSome(
                    results,
                    (r) => r.class,
                    (result) => (
                        <Fragment>
                            {result.entrantTimes.map((a) => (
                                <tr key={a.entrant.entrantId}>
                                    <td>
                                        <p>{result.class}</p>
                                    </td>
                                    <td>
                                        <DriverNumber
                                            driverNumber={
                                                a.entrant.driverNumber
                                            }
                                        />
                                    </td>
                                    <td>{`${a.entrant.givenName} ${a.entrant.familyName}`}</td>
                                    <td>{(a.totalTime / 1000).toFixed(2)}</td>
                                    {currentEvent
                                        ? currentEvent.tests.map((test) =>
                                              testRuns.map((run) => (
                                                  <td
                                                      key={`${test.ordinal}.${run}`}
                                                  >
                                                      <Time
                                                          times={a}
                                                          ordinal={test.ordinal}
                                                          run={run}
                                                      />
                                                  </td>
                                              ))
                                          )
                                        : null}
                                    <td>{a.classPosition + 1}</td>
                                    <td>{a.position + 1}</td>
                                </tr>
                            ))}
                        </Fragment>
                    ),
                    (r: Result) =>
                        classFilter.length === 0 ||
                        classFilter.includes(r.class)
                )}
            </Table>
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
            readonly filter: string;
        }
    >,
    Props
>(({ eventId, filter, ...props }) => ({
    ...props,
    eventId: Number.parseInt(eventId),
    filter: filter ? compact(filter.split(",")) : [],
}))(Results);
