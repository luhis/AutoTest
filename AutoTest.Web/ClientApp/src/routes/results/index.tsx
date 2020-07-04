import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Column } from "rbx";

import { LoadingState, Result } from "../../types/models";
import { getResults } from "../../api/results";
import ifSome from "../../components/shared/isSome";

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
            {ifSome(results, (a) => (
                <Column.Group key={a.totalTime}>
                    <Column>
                        <p>{a.totalTime}</p>
                    </Column>
                </Column.Group>
            ))}
        </div>
    );
};

export default Results;
