import { FunctionalComponent, h } from "preact";
import { Column, Button, Numeric } from "rbx";
import { FaCar, FaMoneyBill } from "react-icons/fa";

import ifSome from "../shared/ifSome";
import { Entrant } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import NumberPlate from "../shared/NumberPlate";
import DeleteButton from "../shared/DeleteButton";

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
        (entrant) => entrant.entrantId,
        (entrant) => (
            <Column.Group>
                <Column>
                    <Numeric>
                        <FaCar />
                        &nbsp;
                        {entrant.driverNumber}
                    </Numeric>
                </Column>
                <Column>
                    <NumberPlate registration={entrant.vehicle.registration} />
                </Column>
                <Column>{`${entrant.givenName} ${entrant.familyName}`}</Column>
                <Column>{entrant.isPaid ? "Paid" : "Unpaid"}</Column>
                <Column>
                    <Button.Group>
                        {entrant.isPaid ? (
                            <Button onClick={() => markPaid(entrant, false)}>
                                <FaMoneyBill />
                                &nbsp; Mark Unpaid
                            </Button>
                        ) : (
                            <Button onClick={() => markPaid(entrant, true)}>
                                <FaMoneyBill />
                                &nbsp; Mark Paid
                            </Button>
                        )}
                        <Button onClick={() => setEditingEntrant(entrant)}>
                            Edit
                        </Button>
                        <DeleteButton deleteFunc={() => deleteEntrant(entrant)}>
                            Delete
                        </DeleteButton>
                    </Button.Group>
                </Column>
            </Column.Group>
        )
    );

export default List;
