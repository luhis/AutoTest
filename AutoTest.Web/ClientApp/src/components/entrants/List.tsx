import { FunctionComponent, h } from "preact";
import { Columns, Button, Form, Dropdown } from "react-bulma-components";
import { FaMoneyBill } from "react-icons/fa";
const { Field, Control } = Form;

import ifSome from "../shared/ifSome";
import {
  EntrantStatus,
  Payment,
  PaymentMethod,
  PublicEntrant,
} from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import NumberPlate from "../shared/NumberPlate";
import DeleteButton from "../shared/DeleteButton";
import DriverNumber from "../shared/DriverNumber";
import { startCase } from "../../lib/string";
import TimeAgo from "../shared/TimeAgo";
import Pay from "./subComponents/Pay";

interface Props {
  readonly entrants: LoadingState<readonly PublicEntrant[], number>;
  readonly setEditingEntrant: (entrant: PublicEntrant) => Promise<void>;
  readonly markPaid: (
    entrant: PublicEntrant,
    payment: Payment | null,
  ) => Promise<void>;
  readonly deleteEntrant: (entrant: PublicEntrant) => Promise<void>;
  readonly isClubAdmin: boolean;
  readonly canEditEntrant: (entrantId: number) => boolean;
}

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
        <Columns.Column>{EntrantStatus[entrant.entrantStatus]}</Columns.Column>
        <Columns.Column>
          {entrant.payment !== null
            ? `Paid (${startCase(PaymentMethod[entrant.payment.method])} ${TimeAgo(entrant.payment.timestamp)})`
            : "Unpaid"}
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
                  &nbsp; Mark Unpaid
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
                    <Pay entrant={entrant} markPaid={markPaid} />
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
    ),
  );

export default List;
