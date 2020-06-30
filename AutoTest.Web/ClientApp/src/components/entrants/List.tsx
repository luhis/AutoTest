import { FunctionalComponent, h } from "preact";
import { Column, Button } from "rbx";

import ifSome from "../shared/isSome";
import { LoadingState, Entrant } from "../../types/models";

interface Props {
    entrants: LoadingState<readonly Entrant[]>;
    setEditingEntrant: (entrant: Entrant) => void;
}

const List: FunctionalComponent<Readonly<Props>> = ({
    entrants,
    setEditingEntrant,
}) =>
    ifSome(entrants, (a) => (
        <Column.Group>
            <Column>{a.registration}</Column>
            <Column>{`${a.givenName} ${a.familyName}`}</Column>
            <Column>
                <Button.Group>
                    <Button onClick={() => setEditingEntrant(a)}>Edit</Button>
                </Button.Group>
            </Column>
        </Column.Group>
    ));

export default List;
