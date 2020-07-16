import { FunctionalComponent, h } from "preact";
import { useEffect } from "preact/hooks";
import { route } from "preact-router";
import { Title, Column, Button, Numeric } from "rbx";
import { useDispatch, useSelector } from "react-redux";

import ifSome from "../../components/shared/ifSome";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import { GetEntrants, GetTests } from "../../store/event/actions";
import { selectTests } from "../../store/event/selectors";

interface Props {
    eventId: string;
}

const Tests: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const dispatch = useDispatch();
    const auth = useGoogleAuth();
    const tests = useSelector(selectTests);
    const eventIdAsNum = Number.parseInt(eventId);
    useEffect(() => {
        dispatch(GetEntrants(eventIdAsNum, getAccessToken(auth)));
    }, [eventIdAsNum, dispatch, auth]);
    useEffect(() => {
        dispatch(GetTests(eventIdAsNum, getAccessToken(auth)));
    }, [eventIdAsNum, dispatch, auth]);
    return (
        <div>
            <Title>Tests</Title>
            {ifSome(
                tests,
                (r) => r.testId,
                (a) => (
                    <Column.Group>
                        <Column>
                            <Numeric>{a.ordinal + 1}</Numeric>
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
                )
            )}
        </div>
    );
};

export default Tests;
