import { FunctionalComponent, h } from "preact";
import { Columns, Button, Form } from "react-bulma-components";
import { FaCar, FaMoneyBill } from "react-icons/fa";
const { Field } = Form;

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
            <Columns>
                <Columns.Column>
                    <p class="number">
                        <FaCar />
                        &nbsp;
                        {entrant.driverNumber}
                    </p>
                </Columns.Column>
                <Columns.Column>
                    <NumberPlate registration={entrant.vehicle.registration} />
                </Columns.Column>
                <Columns.Column>{`${entrant.givenName} ${entrant.familyName}`}</Columns.Column>
                <Columns.Column>
                    {entrant.isPaid ? "Paid" : "Unpaid"}
                </Columns.Column>
                <Columns.Column>
                    <Field kind="group">
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
                    </Field>
                </Columns.Column>
            </Columns>
        )
    );

export default List;
