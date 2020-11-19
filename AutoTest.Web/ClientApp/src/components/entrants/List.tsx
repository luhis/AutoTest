import { FunctionalComponent, h } from "preact";
import { Column, Button, Numeric } from "rbx";

import ifSome from "../shared/ifSome";
import { Entrant } from "../../types/models";
import { LoadingState } from "../../types/loadingState";

interface Props {
    readonly entrants: LoadingState<readonly Entrant[]>;
    readonly setEditingEntrant: (entrant: Entrant) => void;
}

const List: FunctionalComponent<Props> = ({ entrants, setEditingEntrant }) =>
    ifSome(
        entrants,
        (a) => a.entrantId,
        (a) => (
            <Column.Group>
                <Column>
                    <Numeric>{a.driverNumber}</Numeric>
                </Column>
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
