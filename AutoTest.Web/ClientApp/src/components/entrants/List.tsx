import { FunctionalComponent, h } from "preact";
import { Column, Button, Numeric } from "rbx";

import ifSome from "../shared/ifSome";
import { Entrant } from "../../types/models";
import { LoadingState } from "../../types/loadingState";

interface Props {
    readonly entrants: LoadingState<readonly Entrant[]>;
    readonly setEditingEntrant: (entrant: Entrant) => void;
    readonly markPaid: (entrant: Entrant, isPaid: boolean) => void;
}

const List: FunctionalComponent<Props> = ({
    entrants,
    setEditingEntrant,
    markPaid,
}) =>
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
                <Column>{a.isPaid ? "Paid" : "Unpaid"}</Column>
                <Column>
                    <Button.Group>
                        {a.isPaid ? (
                            <Button onClick={() => markPaid(a, false)}>
                                Mark Unpaid
                            </Button>
                        ) : (
                            <Button onClick={() => markPaid(a, true)}>
                                Mark Paid
                            </Button>
                        )}
                        <Button onClick={() => setEditingEntrant(a)}>
                            Edit
                        </Button>
                    </Button.Group>
                </Column>
            </Column.Group>
        )
    );

export default List;
