import { startCase } from "@s-libs/micro-dash";
import { Fragment, FunctionComponent, h } from "preact";
import { useState } from "preact/hooks";
import { Button, Form } from "react-bulma-components";
import { FaMoneyBill } from "react-icons/fa";
import { newValidDate, parseIsoOrThrow } from "ts-date";

import { getDateString } from "../../../lib/date";
import { OnChange, OnSelectChange } from "../../../types/inputs";
import { PublicEntrant, Payment, PaymentMethod } from "../../../types/models";

const { Field, Input, Select, Label } = Form;

const paymentMethods = Object.keys(PaymentMethod)
  .map((a) => Number.parseInt(a))
  .filter((key) => !isNaN(key));

const Pay: FunctionComponent<{
  readonly entrant: PublicEntrant;
  readonly markPaid: (
    entrant: PublicEntrant,
    payment: Payment | null,
  ) => Promise<void>;
}> = ({ entrant, markPaid }) => {
  const [date, setDate] = useState(getDateString(new Date()));
  const [paymentMethod, setPaymentMethod] = useState<PaymentMethod>(
    PaymentMethod.Bacs,
  );
  return (
    <Fragment>
      <Field>
        <Label>Paid Date</Label>
        <Input
          required
          type="date"
          value={date}
          onChange={({ target }: OnChange) => {
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
          fullwidth
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

export default Pay;
