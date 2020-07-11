import { FunctionalComponent, h, Fragment } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Column } from "rbx";
import { flatten } from "micro-dash";

import { Result } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import { getResults } from "../../api/results";
import ifSome from "../../components/shared/ifSome";
import { useDispatch, useSelector } from "react-redux";
import { getAccessToken } from "../../api/api";
import { GetTests } from "../../store/event/actions";
import { selectTests } from "../../store/event/selectors";
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
    const [results, setResults] = useState<LoadingState<readonly Result[]>>({
        tag: "Loading",
        id: eventIdAsNum,
    });
    useEffect(() => {
        const fetchData = async () => {
            const events = await getResults(eventIdAsNum, getAccessToken(auth));
            setResults(events);
        };
        void fetchData();
    }, [auth, eventIdAsNum]);
    useEffect(() => {
        dispatch(GetTests(eventIdAsNum, getAccessToken(auth)));
    }, [eventIdAsNum, dispatch, auth]);

    const getLen = (a: readonly Result[]) => {
        const x = flatten(
            flatten(
                a.map((a) =>
                    a.entrantTimes.map((b) =>
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
            <Column.Group>
                <Column>Class</Column>
                <Column>Name</Column>
                <Column>Total Time</Column>
                {tests.tag === "Loaded"
                    ? tests.value.map((i) => (
                          <Column key={i.testId}>{i.ordinal}</Column>
                      ))
                    : null}
            </Column.Group>
            {ifSome(
                results,
                (r) => r.class,
                (a) => (
                    <Column.Group>
                        <Column>
                            <p>{a.class}</p>
                        </Column>
                        {a.entrantTimes.map((a) => (
                            <Fragment key={a.entrant.entrantId}>
                                <Column>{`${a.entrant.givenName} ${a.entrant.familyName}`}</Column>
                                <Column>{a.totalTime / 1000}</Column>
                                {a.times.map((x) =>
                                    x.timesInMs.map((z) => (
                                        <Column key={z.toString()}>
                                            {z / 1000}
                                        </Column>
                                    ))
                                )}
                                {numToRange(
                                    timeColumnCount - a.times.length
                                ).map((a) => (
                                    <Column key={a}></Column>
                                ))}
                            </Fragment>
                        ))}
                    </Column.Group>
                )
            )}
        </div>
    );
};

export default Results;
