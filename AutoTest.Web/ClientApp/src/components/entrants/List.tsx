import { FunctionalComponent, h } from "preact";
import { Columns, Button, Form } from "react-bulma-components";
import { FaMoneyBill } from "react-icons/fa";
const { Field, Control } = Form;

import ifSome from "../shared/ifSome";
import { PublicEntrant } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import NumberPlate from "../shared/NumberPlate";
import DeleteButton from "../shared/DeleteButton";
import DriverNumber from "../shared/DriverNumber";

interface Props {
    readonly entrants: LoadingState<readonly PublicEntrant[], number>;
    readonly setEditingEntrant: (entrant: PublicEntrant) => Promise<void>;
    readonly markPaid: (entrant: PublicEntrant, isPaid: boolean) => void;
    readonly deleteEntrant: (entrant: PublicEntrant) => void;
    readonly isClubAdmin: boolean;
    readonly canEditEntrant: (entrantId: number) => boolean;
}

const List: FunctionalComponent<Props> = ({
    entrants,
    setEditingEntrant,
    markPaid,
    deleteEntrant,
    isClubAdmin,
    canEditEntrant,
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
                                    disabled={!isClubAdmin}
                                    onClick={() => markPaid(entrant, false)}
                                >
                                    <FaMoneyBill />
                                    &nbsp; Mark Unpaid
                                </Button>
                            </Control>
                        ) : (
                            <Control>
                                <Button
                                    disabled={!isClubAdmin}
                                    onClick={() => markPaid(entrant, true)}
                                >
                                    <FaMoneyBill />
                                    &nbsp; Mark Paid
                                </Button>
                            </Control>
                        )}
                        <Control>
                            <Button
                                disabled={!canEditEntrant(entrant.entrantId)}
                                onClick={() => setEditingEntrant(entrant)}
                            >
                                Edit
                            </Button>
                        </Control>
                        <Control>
                            <DeleteButton
                                disabled={!canEditEntrant(entrant.entrantId)}
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
