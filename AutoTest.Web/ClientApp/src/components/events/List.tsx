import { FunctionComponent, h } from "preact";
import { Columns, Button } from "react-bulma-components";
import { Link, route } from "preact-router";
import save from "save-file";
const { Column } = Columns;

import { getDateString } from "../../lib/date";
import ifSome from "../shared/ifSome";
import { Event } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import DeleteButton from "../shared/DeleteButton";

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
                <Columns>
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
                            <DeleteButton
                                deleteFunc={() => deleteEvent(event)}
                            />
                        </Button.Group>
                    </Column>
                </Columns>
            );
        }
    );

export default List;
