import { FunctionalComponent, h, Fragment } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Table, Breadcrumb } from "rbx";
import { useDispatch, useSelector } from "react-redux";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { range } from "micro-dash";

import { Result } from "../../types/models";
import { LoadingState, findIfLoaded } from "../../types/loadingState";
import { getResults } from "../../api/results";
import ifSome from "../../components/shared/ifSome";
import { useGoogleAuth } from "../../components/app";
import Time from "../../components/results/Time";
import { getAccessToken } from "../../api/api";
import { selectEvents, selectClubs } from "../../store/event/selectors";
import { GetEventsIfRequired } from "../../store/event/actions";
import { getNow } from "../../lib/date";

interface Props {
    readonly eventId: string;
}

const numberToChar = (n: number) => "abcdefghijklmnopqrstuvwxyz".charAt(n);

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
    const testRuns = range(
        currentEvent !== undefined ? currentEvent.maxAttemptsPerTest : 0
    );
    const [results, setResults] = useState<LoadingState<readonly Result[]>>({
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
    }, [eventIdAsNum, dispatch, auth]);

    const connection = new HubConnectionBuilder()
        .withUrl(`/resultsHub`)
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Error)
        .build();
    useEffect(() => {
        void (async () => {
            await connection.start().catch(console.error);
            await connection.invoke("ListenToEvent", eventIdAsNum);
            connection.on("NewTestRun", (newResults: readonly Result[]) => {
                setResults({
                    tag: "Loaded",
                    value: newResults,
                    id: eventIdAsNum,
                    loaded: getNow(),
                });
            });
        })();
        return async () => {
            await connection.invoke("LeaveEvent", eventIdAsNum);
            await connection.stop();
        };
    }, [connection, eventIdAsNum]);

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
        </div>
    );
};

export default Results;
