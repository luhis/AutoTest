import { FunctionalComponent, h } from "preact";
import { Column, Button, Numeric } from "rbx";
import { FaCar, FaMoneyBill } from "react-icons/fa";

import ifSome from "../shared/ifSome";
import { Entrant } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import NumberPlate from "../shared/NumberPlate";

interface Props {
    readonly entrants: LoadingState<readonly Entrant[], number>;
    readonly setEditingEntrant: (entrant: Entrant) => void;
    readonly markPaid: (entrant: Entrant, isPaid: boolean) => void;
    readonly deleteEntrant: (entrant: Entrant) => void;
}

const List: FunctionalComponent<Props> = ({
    entrants,
    setEditingEntrant,
    markPaid,
    deleteEntrant,
}) =>
    ifSome(
        entrants,
        (a) => a.entrantId,
        (a) => (
            <Column.Group>
                <Column>
                    <Numeric>
                        <FaCar />
                        &nbsp;
                        {a.driverNumber}
                    </Numeric>
                </Column>
                <Column>
                    <NumberPlate registration={a.vehicle.registration} />
                </Column>
                <Column>{`${a.givenName} ${a.familyName}`}</Column>
                <Column>{a.isPaid ? "Paid" : "Unpaid"}</Column>
                <Column>
                    <Button.Group>
                        {a.isPaid ? (
                            <Button onClick={() => markPaid(a, false)}>
                                <FaMoneyBill />
                                &nbsp; Mark Unpaid
                            </Button>
                        ) : (
                            <Button onClick={() => markPaid(a, true)}>
                                <FaMoneyBill />
                                &nbsp; Mark Paid
                            </Button>
                        )}
                        <Button onClick={() => setEditingEntrant(a)}>
                            Edit
                        </Button>
                        <Button onClick={() => deleteEntrant(a)}>Delete</Button>
                    </Button.Group>
                </Column>
            </Column.Group>
        )
    );

export default List;
