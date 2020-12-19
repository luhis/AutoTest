import { FunctionalComponent, h, Fragment } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Table, Breadcrumb, Numeric } from "rbx";
import { useDispatch, useSelector } from "react-redux";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { range } from "@s-libs/micro-dash";
import { newValidDate } from "ts-date";

import { Notification, Result } from "../../types/models";
import { LoadingState, findIfLoaded } from "../../types/loadingState";
import { getResults } from "../../api/results";
import ifSome from "../../components/shared/ifSome";
import { useGoogleAuth } from "../../components/app";
import Time from "../../components/results/Time";
import { getAccessToken } from "../../api/api";
import {
    selectEvents,
    selectClubs,
    selectNotifications,
} from "../../store/event/selectors";
import {
    AddNotification,
    GetEventsIfRequired,
    GetNotifications,
} from "../../store/event/actions";
import NotificationsModal from "../../components/events/NotificationsModal";

interface Props {
    readonly eventId: string;
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

const Results: FunctionalComponent<Props> = ({ eventId }) => {
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
    const notifications = useSelector(selectNotifications);
    const testRuns = range(
        currentEvent !== undefined ? currentEvent.maxAttemptsPerTest : 0
    );
    const [results, setResults] = useState<
        LoadingState<readonly Result[], number>
    >({
        tag: "Loading",
        id: eventIdAsNum,
    });
    useEffect(() => {
        const fetchData = async () => {
            const resultsData = await getResults(
                eventIdAsNum,
                getAccessToken(auth)
            );
            setResults(resultsData);
        };
        void fetchData();
    }, [auth, eventIdAsNum]);
    useEffect(() => {
        dispatch(GetEventsIfRequired());
        dispatch(GetNotifications(eventIdAsNum));
    }, [eventIdAsNum, dispatch, auth]);

    useEffect(() => {
        if (connection) {
            connection.on("NewNotification", (notification: Notification) => {
                dispatch(AddNotification(notification));
            });
            connection.on("NewTestRun", (newResults: readonly Result[]) => {
                setResults({
                    tag: "Loaded",
                    value: newResults,
                    id: eventIdAsNum,
                    loaded: newValidDate(),
                });
            });
            void connection
                .start()
                .then(() => {
                    void connection.invoke("ListenToEvent", eventIdAsNum);
                })
                .catch(console.error);
            return async () => {
                await connection.invoke("LeaveEvent", eventIdAsNum);
                await connection.stop();
            };
        } else {
            return () => undefined;
        }
    }, [dispatch, eventIdAsNum]);
    const [showModal, setShowModal] = useState(false);

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
            <Title>Results</Title>
            <Numeric onClick={() => setShowModal(true)}>
                {notifications.tag === "Loaded"
                    ? notifications.value.length
                    : ""}
            </Numeric>
            <Table>
                <Table.Head>
                    <Table.Row>
                        <Table.Heading>Class</Table.Heading>
                        <Table.Heading>Name</Table.Heading>
                        <Table.Heading>Total Time</Table.Heading>
                        {currentEvent
                            ? currentEvent.tests.map((test) =>
                                  testRuns.map((run) => (
                                      <Table.Heading
                                          key={`${test.ordinal}.${run}`}
                                      >
                                          {test.ordinal + 1}.{numberToChar(run)}
                                      </Table.Heading>
                                  ))
                              )
                            : null}
                    </Table.Row>
                </Table.Head>
                {ifSome(
                    results,
                    (r) => r.class,
                    (result) => (
                        <Fragment>
                            {result.entrantTimes.map((a) => (
                                <Table.Row key={a.entrant.entrantId}>
                                    <Table.Cell>
                                        <p>{result.class}</p>
                                    </Table.Cell>
                                    <Table.Cell>{`${a.entrant.givenName} ${a.entrant.familyName}`}</Table.Cell>
                                    <Table.Cell>
                                        {(a.totalTime / 1000).toFixed(2)}
                                    </Table.Cell>
                                    {currentEvent
                                        ? currentEvent.tests.map((test) =>
                                              testRuns.map((run) => (
                                                  <Table.Cell
                                                      key={`${test.ordinal}.${run}`}
                                                  >
                                                      <Time
                                                          times={a}
                                                          ordinal={test.ordinal}
                                                          run={run}
                                                      />
                                                  </Table.Cell>
                                              ))
                                          )
                                        : null}
                                </Table.Row>
                            ))}
                        </Fragment>
                    )
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

export default Results;
