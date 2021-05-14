import { FunctionComponent, h } from "preact";
import { Columns, Button, Form } from "react-bulma-components";
import { Link } from "preact-router";
const { Column } = Columns;
const { Field } = Form;

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
            return (
                <Columns>
                    <Column>
                        <p key={event.eventId}>
                            {event.startTime.toLocaleDateString()}{" "}
                            {event.location}
                        </p>
                    </Column>
                    <Column>
                        <Field kind="group">
                            <Link href={`/event/${event.eventId}`}>View</Link>
                            <Button onClick={() => setEditingEvent(event)}>
                                Edit
                            </Button>
                            <DeleteButton
                                deleteFunc={() => deleteEvent(event)}
                            />
                        </Field>
                    </Column>
                </Columns>
            );
        }
    );

export default List;
