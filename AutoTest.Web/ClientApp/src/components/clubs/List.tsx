import { FunctionComponent, h } from "preact";
import { Column, Button } from "rbx";
import { route } from "preact-router";

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
            <Column.Group>
                <Column>
                    {club.clubName}
                    &nbsp;
                    {club.website !== "" ? (
                        <a href={club.website}>Homepage</a>
                    ) : null}
                </Column>
                <Column>
                    <Button.Group>
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
                    </Button.Group>
                </Column>
            </Column.Group>
        )
    );

export default ClubsList;
