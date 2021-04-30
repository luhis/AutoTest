import { FunctionComponent, h } from "preact";
import { Columns, Button, Form } from "react-bulma-components";
import { route } from "preact-router";
const { Field } = Form;

import { Club } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import ifSome from "../shared/ifSome";
import DeleteButton from "../shared/DeleteButton";

interface Props {
    readonly clubs: LoadingState<readonly Club[]>;
    readonly setEditingClub: (club: Club) => void;
    readonly deleteClub: (club: Club) => void;
}

const ClubsList: FunctionComponent<Props> = ({
    clubs,
    setEditingClub,
    deleteClub,
}) =>
    ifSome(
        clubs,
        (club) => club.clubId,
        (club) => (
            <Columns>
                <Columns.Column>
                    {club.clubName}
                    &nbsp;
                    {club.website !== "" ? (
                        <a href={club.website}>Homepage</a>
                    ) : null}
                </Columns.Column>
                <Columns.Column>
                    <Field kind="group">
                        <Button
                            onClick={() =>
                                route(`/events?clubId=${club.clubId}`)
                            }
                        >
                            Events
                        </Button>
                        <Button onClick={() => setEditingClub(club)}>
                            Edit
                        </Button>
                        <DeleteButton deleteFunc={() => deleteClub(club)}>
                            Delete
                        </DeleteButton>
                    </Field>
                </Columns.Column>
            </Columns>
        )
    );

export default ClubsList;
