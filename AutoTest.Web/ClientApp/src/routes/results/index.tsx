import { FunctionalComponent, h, Fragment } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Table } from "rbx";
import { flatten } from "micro-dash";

import { Result } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import { getResults } from "../../api/results";
import ifSome from "../../components/shared/ifSome";
import { useDispatch, useSelector } from "react-redux";
import { getAccessToken } from "../../api/api";
import { GetTests } from "../../store/event/actions";
import { selectTests, selectEvents } from "../../store/event/selectors";
import { useGoogleAuth } from "../../components/app";

interface Props {
    eventId: string;
}

const numToRange = (length: number) =>
    Array<number>(length)
        .fill(1)
        .map((_, i) => i);

const Results: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const dispatch = useDispatch();
    const auth = useGoogleAuth();
    const eventIdAsNum = Number.parseInt(eventId);
    const tests = useSelector(selectTests);
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
        dispatch(GetTests(eventIdAsNum, getAccessToken(auth)));
    }, [eventIdAsNum, dispatch, auth]);

    const getLen = (a: readonly Result[]) => {
        const x = flatten(
            flatten(
                a.map((result) =>
                    result.entrantTimes.map((b) =>
                        b.times.map((c) => c.timesInMs.length)
                    )
                )
            )
        );
        return Math.max(...x);
    };

    const timeColumnCount: number =
        results.tag === "Loaded" ? getLen(results.value) : 0;
    return (
        <div>
            <Title>Results</Title>
            <Table>
                <Table.Head>
                    <Table.Row>
                        <Table.Heading>Class</Table.Heading>
                        <Table.Heading>Name</Table.Heading>
                        <Table.Heading>Total Time</Table.Heading>
                        {tests.tag === "Loaded"
                            ? tests.value.map((test) =>
                                  testRuns.map((run) => (
                                      <Table.Heading
                                          key={`${test.testId}.${run}`}
                                      >
                                          {test.ordinal}.{run}
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
                                        {a.totalTime / 1000}
                                    </Table.Cell>
                                    {a.times.map((x) =>
                                        x.timesInMs.map((z) => (
                                            <Table.Cell key={z.toString()}>
                                                {z / 1000}
                                            </Table.Cell>
                                        ))
                                    )}
                                    {numToRange(
                                        timeColumnCount - a.times.length
                                    ).map((test) => (
                                        <Table.Cell key={test}></Table.Cell>
                                    ))}
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
