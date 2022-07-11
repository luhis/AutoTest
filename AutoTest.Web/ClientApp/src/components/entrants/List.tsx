import { Fragment, FunctionComponent, h } from "preact";
import { Columns, Button, Form, Dropdown } from "react-bulma-components";
import { FaMoneyBill } from "react-icons/fa";
import { newValidDate, parseIsoOrThrow } from "ts-date";
import { useState } from "preact/hooks";
const { Field, Control, Input, Select, Label } = Form;

import ifSome from "../shared/ifSome";
import { Payment, PaymentMethod, PublicEntrant } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import NumberPlate from "../shared/NumberPlate";
import DeleteButton from "../shared/DeleteButton";
import DriverNumber from "../shared/DriverNumber";
import { startCase } from "../../lib/string";
import { OnChange, OnSelectChange } from "../../types/inputs";
import { getDateString } from "../../lib/date";

interface Props {
    readonly entrants: LoadingState<readonly PublicEntrant[], number>;
    readonly setEditingEntrant: (entrant: PublicEntrant) => Promise<void>;
    readonly markPaid: (
        entrant: PublicEntrant,
        payment: Payment | null
    ) => Promise<void>;
    readonly deleteEntrant: (entrant: PublicEntrant) => Promise<void>;
    readonly isClubAdmin: boolean;
    readonly canEditEntrant: (entrantId: number) => boolean;
}

const paymentMethods = Object.keys(PaymentMethod)
    .map((a) => Number.parseInt(a))
    .filter((key) => !isNaN(key));

const Pay: FunctionComponent<{
    readonly entrant: PublicEntrant;
    readonly markPaid: (
        entrant: PublicEntrant,
        payment: Payment | null
    ) => Promise<void>;
}> = ({ entrant, markPaid }) => {
    const [date, setDate] = useState(getDateString(new Date()));
    const [paymentMethod, setPaymentMethod] = useState<PaymentMethod>(
        PaymentMethod.Bacs
    );
    return (
        <Fragment>
            <Field>
                <Label>Paid Date</Label>
                <Input
                    required
                    type="date"
                    value={date}
                    onChange={({ target }: OnChange): void => {
                        if (target.valueAsDate !== null) {
                            setDate(target.value);
                        }
                    }}
                ></Input>
            </Field>
            <Field>
                <Label>Payment Method</Label>
                <Select<PaymentMethod>
                    required
                    class="is-fullwidth"
                    onChange={(evt: OnSelectChange) =>
                        setPaymentMethod(Number.parseInt(evt.target.value))
                    }
                    value={paymentMethod}
                >
                    <option disabled value={Number.NaN}>
                        - Please Select -
                    </option>
                    {paymentMethods.map((key) => (
                        <option key={key} value={key}>
                            {startCase(PaymentMethod[key])}
                        </option>
                    ))}
                </Select>
            </Field>
            <Button
                onClick={() =>
                    markPaid(entrant, {
                        timestamp: newValidDate(),
                        method: paymentMethod,
                        paidAt: parseIsoOrThrow(date),
                    })
                }
            >
                <FaMoneyBill />
                &nbsp; Mark Paid
            </Button>
        </Fragment>
    );
};

const List: FunctionComponent<Props> = ({
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
                    {entrant.payment !== null ? "Paid" : "Unpaid"}
                </Columns.Column>
                <Columns.Column>
                    <Field kind="group">
                        {entrant.payment !== null ? (
                            <Control>
                                <Button
                                    disabled={!isClubAdmin}
                                    onClick={() => markPaid(entrant, null)}
                                >
                                    <FaMoneyBill />
                                    &nbsp; Mark Unpaid (
                                    {startCase(
                                        PaymentMethod[entrant.payment.method]
                                    )}
                                    )
                                </Button>
                            </Control>
                        ) : (
                            <Control>
                                <Dropdown
                                    disabled={!isClubAdmin}
                                    label="Mark Paid"
                                    closeOnSelect={false}
                                >
                                    <Dropdown.Item value="mark paid">
                                        <Pay
                                            entrant={entrant}
                                            markPaid={markPaid}
                                        />
                                    </Dropdown.Item>
                                </Dropdown>
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
