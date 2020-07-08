import { FunctionComponent, h } from "preact";
import { Column, Button } from "rbx";
import { route } from "preact-router";

import { Club } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import ifSome from "../shared/ifSome";

interface Props {
    clubs: LoadingState<readonly Club[]>;
    setEditingClub: (club: Club) => void;
}

const ClubsList: FunctionComponent<Props> = ({ clubs, setEditingClub }) =>
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
                    </Button.Group>
                </Column>
            </Column.Group>
        )
    );

export default ClubsList;
