import { FunctionComponent, h } from "preact";
import { Column, Button } from "rbx";
import { route } from "preact-router";

import ifSome from "../../components/shared/isSome";
import { LoadingState, Event } from "../../types/models";

interface Props {
    events: LoadingState<readonly Event[]>;
    setEditingEvent: (event: Event) => void;
}

const List: FunctionComponent<Props> = ({ events, setEditingEvent }) =>
    ifSome(events, (a) => (
        <Column.Group>
            <Column>
                <p key={a.eventId}>{a.location}</p>
            </Column>
            <Column>
                <Button.Group>
                    <Button onClick={() => setEditingEvent(a)}>Edit</Button>
                    <Button onClick={() => route(`/entrants/${a.eventId}`)}>
                        Entrants
                    </Button>
                    <Button onClick={() => route(`/tests/${a.eventId}`)}>
                        Tests
                    </Button>
                    <Button onClick={() => route(`/results/${a.eventId}`)}>
                        Results
                    </Button>
                </Button.Group>
            </Column>
        </Column.Group>
    ));

export default List;
