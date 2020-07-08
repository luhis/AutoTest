import { FunctionalComponent, h, Fragment } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Column } from "rbx";

import { Result } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import { getResults } from "../../api/results";
import ifSome from "../../components/shared/ifSome";

interface Props {
    eventId: string;
}

const Results: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const eventIdAsNum = Number.parseInt(eventId);
    const [results, setResults] = useState<LoadingState<readonly Result[]>>({
        tag: "Loading",
    });
    useEffect(() => {
        const fetchData = async () => {
            const events = await getResults(eventIdAsNum, undefined);
            setResults(events);
        };
        void fetchData();
    }, [eventIdAsNum]);
    return (
        <div>
            <Title>Results</Title>
            {ifSome(
                results,
                (r) => r.class,
                (a) => (
                    <Column.Group>
                        <Column>
                            <p>{a.class}</p>
                        </Column>
                        <Column>
                            {a.entrantTimes.map((a) => (
                                <Fragment key={a.entrant.entrantId}>
                                    <p
                                        key={a.entrant.entrantId}
                                    >{`${a.entrant.givenName} ${a.entrant.familyName}`}</p>
                                    {a.times.map((x) => (
                                        <p key={x.toString()}>{x.ordinal}</p>
                                    ))}
                                    <p></p>
                                </Fragment>
                            ))}
                        </Column>
                    </Column.Group>
                )
            )}
        </div>
    );
};

export default Results;
