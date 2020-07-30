import { FunctionalComponent, h, Fragment } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Table } from "rbx";
import { useDispatch, useSelector } from "react-redux";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

import { Result, EntrantTime } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import { getResults } from "../../api/results";
import ifSome from "../../components/shared/ifSome";
import { getAccessToken } from "../../api/api";
import { selectEvents } from "../../store/event/selectors";
import { useGoogleAuth } from "../../components/app";
import { GetEventsIfRequired } from "../../store/event/actions";

interface Props {
    eventId: string;
}

const numToRange = (length: number) =>
    Array<number>(length)
        .fill(0)
        .map((_, i) => i);

const numberToChar = (n: number) => "abcdefghijklmnopqrstuvwxyz".charAt(n);

const getTime = (times: EntrantTime, ordinal: number, run: number) => {
    const testValues = times.times.find((t) => t.ordinal === ordinal);
    const noneReturn = "X";
    if (testValues) {
        const runValue = testValues.timesInMs[run];
        return runValue ? (runValue / 1000).toFixed(2) : noneReturn;
    }
    return noneReturn;
};

const Results: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const dispatch = useDispatch();
    const auth = useGoogleAuth();
    const eventIdAsNum = Number.parseInt(eventId);
    const events = useSelector(selectEvents);
    const currentEvent =
        events.tag === "Loaded"
            ? events.value.find((a) => a.eventId === eventIdAsNum)
            : undefined;
    const testRuns = numToRange(
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
        const subscribe = async () => {
            await connection.start().catch(console.error);
            await connection.invoke("ListenToEvent", eventIdAsNum);
        };
        void subscribe();
        return () => {
            const tidy = async () => {
                await connection.invoke("LeaveEvent", eventIdAsNum);
                await connection.stop();
            };
            void tidy();
        };
    }, [connection, eventIdAsNum]);

    return (
        <div>
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
                                                      {getTime(
                                                          a,
                                                          test.ordinal,
                                                          run
                                                      )}
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
