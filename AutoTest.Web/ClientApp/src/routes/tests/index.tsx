import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Column } from "rbx";
import { Link } from "preact-router";

import { LoadingState, Test } from "../../types/models";
import { getTests } from "../../api/tests";
import ifSome from "../../components/shared/isSome";

interface Props {
    eventId: number;
}

const Tests: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const [tests, setTests] = useState<LoadingState<readonly Test[]>>({
        tag: "Loading",
    });
    useEffect(() => {
        const fetchData = async () => {
            const events = await getTests(eventId, undefined);
            setTests(events);
        };
        void fetchData();
    }, [eventId]);
    return (
        <div>
            <Title>Tests</Title>
            {ifSome(tests, (a) => (
                <Column.Group key={a.testId}>
                    <Column>
                        <p>{a.ordinal}</p>
                    </Column>
                    <Column>
                        <Link href={`/marshal/${eventId}/${a.testId}`}>
                            Marshal
                        </Link>
                    </Column>
                </Column.Group>
            ))}
        </div>
    );
};

export default Tests;
