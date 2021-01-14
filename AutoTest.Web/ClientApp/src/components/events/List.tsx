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
        (event) => event.eventId,
        (event) => {
            const saveRegs = () =>
                save(
                    event.regulations,
                    `${event.location}-${getDateString(
                        event.startTime
                    )}-regs.pdf`
                );
            return (
                <Column.Group>
                    <Column>
                        <p key={event.eventId}>
                            {event.startTime.toLocaleDateString()}{" "}
                            {event.location}
                        </p>
                    </Column>
                    <Column>
                        <Button.Group>
                            <Link href={`/event/${event.eventId}`}>View</Link>
                            <Button onClick={() => setEditingEvent(event)}>
                                Edit
                            </Button>
                            <Button
                                onClick={() =>
                                    route(`/entrants/${event.eventId}`)
                                }
                            >
                                Entrants
                            </Button>
                            <Button
                                onClick={() => route(`/tests/${event.eventId}`)}
                            >
                                Tests
                            </Button>
                            <Button
                                onClick={() =>
                                    route(`/results/${event.eventId}`)
                                }
                            >
                                Results
                            </Button>
                            <Button onClick={saveRegs}>Regs</Button>
                            <Button onClick={() => deleteEvent(event)}>
                                Delete
                            </Button>
                        </Button.Group>
                    </Column>
                </Column.Group>
            );
        }
    );

export default List;
