import { FunctionComponent, h } from "preact";
import { Box, Button, Form } from "react-bulma-components";
import { route } from "preact-router";
const { Field, Control } = Form;

import { Club } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import ifSome from "../shared/ifSome";
import DeleteButton from "../shared/DeleteButton";

interface Props {
  readonly clubs: LoadingState<readonly Club[]>;
  readonly setEditingClub: (club: Club) => void;
  readonly deleteClub: (club: Club) => Promise<void>;
  readonly isClubAdmin: (club: Club) => boolean;
}

const ClubsList: FunctionComponent<Props> = ({
  clubs,
  setEditingClub,
  deleteClub,
  isClubAdmin,
}) =>
  ifSome(
    clubs,
    (club) => club.clubId,
    (club) => (
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
              {club.clubName}
            </span>
            {club.website !== "" ? (
              <span class="ml-2">
                <a
                  href={club.website}
                  target="_blank"
                  rel="noopener noreferrer"
                  class="has-text-link"
                >
                  Homepage
                </a>
              </span>
            ) : null}
          </div>
          <Field kind="group">
            <Control>
              <Button
                color="link is-outlined"
                onClick={() => route(`/events?clubId=${club.clubId}`)}
              >
                Events
              </Button>
            </Control>
            <Control>
              <Button
                onClick={() => setEditingClub(club)}
                disabled={!isClubAdmin(club)}
              >
                Edit
              </Button>
            </Control>
            <Control>
              <DeleteButton
                deleteFunc={() => deleteClub(club)}
                disabled={!isClubAdmin(club)}
              >
                Delete
              </DeleteButton>
            </Control>
          </Field>
        </div>
      </Box>
    ),
  );

export default ClubsList;
