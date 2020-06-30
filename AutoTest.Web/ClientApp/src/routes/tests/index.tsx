import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Column, Button } from "rbx";
import { route } from "preact-router";

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
                        <Button.Group>
                            <Button
                                onClick={() =>
                                    route(`/marshal/${eventId}/${a.testId}`)
                                }
                            >
                                Marshal
                            </Button>
                        </Button.Group>
                    </Column>
                </Column.Group>
            ))}
        </div>
    );
};

export default Tests;
