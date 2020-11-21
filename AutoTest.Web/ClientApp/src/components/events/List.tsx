import { FunctionComponent, h } from "preact";
import { Column, Button } from "rbx";
import { route } from "preact-router";

import ifSome from "../shared/ifSome";
import { Event } from "../../types/models";
import { LoadingState } from "../../types/loadingState";

interface Props {
    readonly events: LoadingState<readonly Event[]>;
    readonly setEditingEvent: (event: Event) => void;
    readonly setRegsModal: (event: Event) => void;
}

const List: FunctionComponent<Props> = ({
    events,
    setEditingEvent,
    setRegsModal,
}) =>
    ifSome(
        events,
        (a) => a.eventId,
        (a) => (
            <Column.Group>
                <Column>
                    <p key={a.eventId}>
                        {a.startTime.toLocaleDateString()} {a.location}
                    </p>
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
                        <Button onClick={() => setRegsModal(a)}>Regs</Button>
                    </Button.Group>
                </Column>
            </Column.Group>
        )
    );

export default List;
