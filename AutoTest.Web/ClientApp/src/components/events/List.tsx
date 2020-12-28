import { FunctionComponent, h } from "preact";
import { Column, Button } from "rbx";
import { Link, route } from "preact-router";
import save from "save-file";

import { getDateString } from "../../lib/date";
import ifSome from "../shared/ifSome";
import { Event } from "../../types/models";
import { LoadingState } from "../../types/loadingState";

interface Props {
    readonly events: LoadingState<readonly Event[]>;
    readonly setEditingEvent: (event: Event) => void;
    readonly deleteEvent: (event: Event) => void;
}

const List: FunctionComponent<Props> = ({
    events,
    setEditingEvent,
    deleteEvent,
}) =>
    ifSome(
        events,
        (a) => a.eventId,
        (a) => {
            const saveRegs = () =>
                save(
                    a.regulations,
                    `${a.location}-${getDateString(a.startTime)}-regs.pdf`
                );
            return (
                <Column.Group>
                    <Column>
                        <p key={a.eventId}>
                            {a.startTime.toLocaleDateString()} {a.location}
                        </p>
                    </Column>
                    <Column>
                        <Button.Group>
                            <Link href={`/event/${a.eventId}`}>View</Link>
                            <Button onClick={() => setEditingEvent(a)}>
                                Edit
                            </Button>
                            <Button
                                onClick={() => route(`/entrants/${a.eventId}`)}
                            >
                                Entrants
                            </Button>
                            <Button
                                onClick={() => route(`/tests/${a.eventId}`)}
                            >
                                Tests
                            </Button>
                            <Button
                                onClick={() => route(`/results/${a.eventId}`)}
                            >
                                Results
                            </Button>
                            <Button onClick={saveRegs}>Regs</Button>
                            <Button onClick={() => deleteEvent(a)}>
                                Delete
                            </Button>
                        </Button.Group>
                    </Column>
                </Column.Group>
            );
        }
    );

export default List;
