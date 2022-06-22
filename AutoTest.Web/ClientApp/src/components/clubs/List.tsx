import { FunctionComponent, h } from "preact";
import { Columns, Button, Form } from "react-bulma-components";
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
                        <Control>
                            <Button
                                onClick={() =>
                                    route(`/events?clubId=${club.clubId}`)
                                }
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
                </Columns.Column>
            </Columns>
        )
    );

export default ClubsList;
