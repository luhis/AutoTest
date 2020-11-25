import { FunctionComponent, h } from "preact";
import { Column, Button } from "rbx";
import { route } from "preact-router";

import { Club } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import ifSome from "../shared/ifSome";

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
        (a) => a.clubId,
        (a) => (
            <Column.Group>
                <Column>
                    {a.clubName}
                    &nbsp;
                    {a.website !== "" ? <a href={a.website}>Homepage</a> : null}
                </Column>
                <Column>
                    <Button.Group>
                        <Button
                            onClick={() => route(`/events?clubId=${a.clubId}`)}
                        >
                            Events
                        </Button>
                        <Button onClick={() => setEditingClub(a)}>Edit</Button>
                        <Button onClick={() => deleteClub(a)}>Delete</Button>
                    </Button.Group>
                </Column>
            </Column.Group>
        )
    );

export default ClubsList;
