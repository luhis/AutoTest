import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Column, Button } from "rbx";
import { route } from "preact-router";
import { useDispatch } from "react-redux";

import { LoadingState, Test } from "../../types/models";
import { getTests } from "../../api/tests";
import ifSome from "../../components/shared/isSome";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import { GetEntrants } from "../../store/event/actions";

interface Props {
    eventId: string;
}

const Tests: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const dispatch = useDispatch();
    const auth = useGoogleAuth();
    const [tests, setTests] = useState<LoadingState<readonly Test[]>>({
        tag: "Loading",
    });
    const eventIdAsNum = Number.parseInt(eventId);
    useEffect(() => {
        dispatch(GetEntrants(eventIdAsNum, getAccessToken(auth)));
        const fetchData = async () => {
            const events = await getTests(eventIdAsNum, getAccessToken(auth));
            setTests(events);
        };
        void fetchData();
    }, [eventIdAsNum, dispatch, auth]);
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
