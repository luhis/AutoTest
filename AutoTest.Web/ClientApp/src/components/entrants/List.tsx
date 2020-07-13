import { FunctionalComponent, h } from "preact";
import { Column, Button } from "rbx";

import ifSome from "../shared/ifSome";
import { Entrant } from "../../types/models";
import { LoadingState } from "../../types/loadingState";

interface Props {
    entrants: LoadingState<readonly Entrant[]>;
    setEditingEntrant: (entrant: Entrant) => void;
}

const List: FunctionalComponent<Readonly<Props>> = ({
    entrants,
    setEditingEntrant,
}) =>
    ifSome(
        entrants,
        (a) => a.entrantId,
        (a) => (
            <Column.Group>
                <Column>{a.driverNumber}</Column>
                <Column>{a.vehicle.registration}</Column>
                <Column>{`${a.givenName} ${a.familyName}`}</Column>
                <Column>
                    <Button.Group>
                        <Button onClick={() => setEditingEntrant(a)}>
                            Edit
                        </Button>
                    </Button.Group>
                </Column>
            </Column.Group>
        )
    );

export default List;
