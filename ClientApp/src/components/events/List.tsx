import { FunctionComponent, h } from "preact";
import { Box, Button, Form } from "react-bulma-components";
import { Link } from "preact-router";
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
        <Box>
          <div
            style={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              flexWrap: "wrap",
              gap: "0.5rem",
            }}
          >
            <div>
              <span class="has-text-weight-semibold is-size-5">
                {event.location}
              </span>
              <br />
              <span class="has-text-grey">
                {event.startTime.toLocaleDateString()}
              </span>
            </div>
            <Field kind="group">
              <Control>
                <Link
                  class="button is-link is-outlined"
                  href={`/event/${event.eventId}`}
                >
                  View
                </Link>
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
          </div>
        </Box>
      );
    },
  );

export default List;
