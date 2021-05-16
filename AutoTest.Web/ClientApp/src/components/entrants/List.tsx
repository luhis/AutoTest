import { FunctionalComponent, h } from "preact";
import { Columns, Button, Form } from "react-bulma-components";
import { FaMoneyBill } from "react-icons/fa";
const { Field, Control } = Form;

import ifSome from "../shared/ifSome";
import { Entrant } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import NumberPlate from "../shared/NumberPlate";
import DeleteButton from "../shared/DeleteButton";
import DriverNumber from "../shared/DriverNumber";

interface Props {
    readonly entrants: LoadingState<readonly Entrant[], number>;
    readonly setEditingEntrant: (entrant: Entrant) => void;
    readonly markPaid: (entrant: Entrant, isPaid: boolean) => void;
    readonly deleteEntrant: (entrant: Entrant) => void;
    readonly canSetPaid: boolean;
}

const List: FunctionalComponent<Props> = ({
    entrants,
    setEditingEntrant,
    markPaid,
    deleteEntrant,
    canSetPaid,
}) =>
    ifSome(
        entrants,
        (entrant) => entrant.entrantId,
        (entrant) => (
            <Columns>
                <Columns.Column>
                    <DriverNumber driverNumber={entrant.driverNumber} />
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
                            <Control>
                                <Button
                                    disabled={!canSetPaid}
                                    onClick={() => markPaid(entrant, false)}
                                >
                                    <FaMoneyBill />
                                    &nbsp; Mark Unpaid
                                </Button>
                            </Control>
                        ) : (
                            <Control>
                                <Button
                                    disabled={!canSetPaid}
                                    onClick={() => markPaid(entrant, true)}
                                >
                                    <FaMoneyBill />
                                    &nbsp; Mark Paid
                                </Button>
                            </Control>
                        )}
                        <Control>
                            <Button onClick={() => setEditingEntrant(entrant)}>
                                Edit
                            </Button>
                        </Control>
                        <Control>
                            <DeleteButton
                                deleteFunc={() => deleteEntrant(entrant)}
                            >
                                Delete
                            </DeleteButton>
                        </Control>
                    </Field>
                </Columns.Column>
            </Columns>
        )
    );

export default List;
