import { FunctionalComponent, h } from "preact";
import { useEffect } from "preact/hooks";
import { route } from "preact-router";
import { Title, Column, Button } from "rbx";
import { useDispatch, useSelector } from "react-redux";

import ifSome from "../../components/shared/ifSome";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import { GetEntrants, GetTests } from "../../store/event/actions";
import { selectTests, selectEntrants } from "../../store/event/selectors";
import { requiresLoading } from "../../types/loadingState";

interface Props {
    eventId: string;
}

const Tests: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const dispatch = useDispatch();
    const auth = useGoogleAuth();
    const tests = useSelector(selectTests);
    const entrants = useSelector(selectEntrants);
    const eventIdAsNum = Number.parseInt(eventId);
    useEffect(() => {
        if (requiresLoading(entrants.tag)) {
            dispatch(GetEntrants(eventIdAsNum, getAccessToken(auth)));
        }
    }, [eventIdAsNum, dispatch, auth, entrants.tag]);
    useEffect(() => {
        if (requiresLoading(tests.tag)) {
            dispatch(GetTests(eventIdAsNum, getAccessToken(auth)));
        }
    }, [eventIdAsNum, dispatch, auth, tests.tag]);
    return (
        <div>
            <Title>Tests</Title>
            {ifSome(
                tests,
                (r) => r.testId,
                (a) => (
                    <Column.Group>
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
                )
            )}
        </div>
    );
};

export default Tests;
