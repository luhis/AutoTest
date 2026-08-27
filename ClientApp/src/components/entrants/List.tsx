import { FunctionComponent, h } from "preact";
import { Box, Button, Form, Dropdown, Tag } from "react-bulma-components";
import { FaMoneyBill } from "react-icons/fa";
import { startCase } from "@s-libs/micro-dash";
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
      <Box>
        <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            flexWrap: "wrap",
            gap: "0.5rem",
          }}
        >
          <div
            style={{
              display: "flex",
              alignItems: "center",
              gap: "0.75rem",
              flexWrap: "wrap",
            }}
          >
            <DriverNumber driverNumber={entrant.driverNumber} />
            <NumberPlate registration={entrant.vehicle.registration} />
            <span class="has-text-weight-semibold">{`${entrant.givenName} ${entrant.familyName}`}</span>
            <Tag
              color={entrant.entrantStatus === 0 ? "success" : "info"}
              size="small"
            >
              {EntrantStatus[entrant.entrantStatus]}
            </Tag>
            {entrant.payment !== null ? (
              <Tag color="success is-light" size="small">
                Paid ({startCase(PaymentMethod[entrant.payment.method])}{" "}
                {TimeAgo(entrant.payment.timestamp)})
              </Tag>
            ) : (
              <Tag color="warning is-light" size="small">
                Unpaid
              </Tag>
            )}
          </div>
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
        </div>
      </Box>
    ),
  );

export default List;
