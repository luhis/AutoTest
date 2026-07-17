import { FunctionComponent, h } from "preact";
import { Columns, Button, Form } from "react-bulma-components";
import { Link } from "preact-router";
const { Column } = Columns;
const { Field, Control } = Form;

import ifSome from "../shared/ifSome";
import { Event } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import DeleteButton from "../shared/DeleteButton";

interface Props {
  readonly events: LoadingState<readonly Event[]>;
  readonly setEditingEvent: (event: Event) => void;
  readonly deleteEvent: (event: Event) => Promise<void>;
  readonly canAdmin: (clubId: number) => boolean;
}

const List: FunctionComponent<Props> = ({
  events,
  setEditingEvent,
  deleteEvent,
  canAdmin,
}) =>
  ifSome(
    events,
    (event) => event.eventId,
    (event) => {
      return (
        <Columns>
          <Column>
            {event.startTime.toLocaleDateString()} {event.location}
          </Column>
          <Column>
            <Field kind="group">
              <Control>
                <Link href={`/event/${event.eventId}`}>View</Link>
              </Control>
              <Control>
                <Button
                  disabled={!canAdmin(event.clubId)}
                  onClick={() => setEditingEvent(event)}
                >
                  Edit
                </Button>
              </Control>

              <Control>
                <DeleteButton
                  disabled={!canAdmin(event.clubId)}
                  deleteFunc={() => deleteEvent(event)}
                />
              </Control>
            </Field>
          </Column>
        </Columns>
      );
    },
  );

export default List;
