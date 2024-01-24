import { h, FunctionComponent, Fragment } from "preact";
import { Button, Form, Level } from "react-bulma-components";
import { useState } from "preact/hooks";
import { addYear, newValidDate, parseIsoOrThrow } from "ts-date";
const { Input, Control, Field, Label } = Form;
import { FaPlus } from "react-icons/fa";

import { OnChange } from "../../types/inputs";
import { getDateString } from "../../lib/date";
import { ClubMembership } from "../../types/shared";
import { Override } from "src/types/models";

interface Props {
  readonly memberships: readonly ClubMembership[];
  readonly addNew: (_: ClubMembership) => void;
  readonly remove: (_: number) => void;
}

type EditingMembership = Override<ClubMembership, { readonly expiry: string }>;

const blankState = (): EditingMembership => ({
  clubName: "",
  membershipNumber: Number.NaN,
  expiry: getDateString(addYear(newValidDate(), 1)),
});

const MembershipList: FunctionComponent<Props> = ({
  memberships,
  addNew,
  remove,
}) => {
  const [newMembership, setNewMembership] =
    useState<EditingMembership>(blankState);
  const currentTime = newValidDate();
  return (
    <Fragment>
      <Label>Memberships</Label>
      {memberships.map((a, i) => (
        <Level key={`${a.clubName}`}>
          <Level.Item align="left">{a.clubName}</Level.Item>
          <Level.Item align="left">{a.membershipNumber}</Level.Item>
          <Level.Item
            align="left"
            textColor={a.expiry < currentTime ? "danger" : undefined}
          >
            {a.expiry.toLocaleDateString()}
          </Level.Item>
          <Level.Item align="right">
            <Button title="Delete" remove onClick={() => remove(i)} />
          </Level.Item>
        </Level>
      ))}
      <Field kind="group">
        <Control fullwidth={true}>
          <Label>Club Name</Label>
          <Input
            value={newMembership.clubName}
            onChange={({ target }: OnChange) =>
              setNewMembership((e) => ({
                ...e,
                clubName: target.value,
              }))
            }
          />
        </Control>
        <Control fullwidth={true}>
          <Label>Membership Number</Label>
          <Input
            type="number"
            step={1}
            value={newMembership.membershipNumber}
            onChange={({ target }: OnChange) =>
              setNewMembership((e) => ({
                ...e,
                membershipNumber: Math.floor(target.valueAsNumber),
              }))
            }
          />
        </Control>
        <Control fullwidth={true}>
          <Label>Expiry</Label>
          <Input
            type="date"
            value={newMembership.expiry}
            onChange={({ target }: OnChange) => {
              if (target.valueAsDate !== null) {
                setNewMembership((e) => ({
                  ...e,
                  expiry: target.value,
                }));
              }
            }}
          />
        </Control>
        <Control className="mt-5">
          <Button
            disabled={
              newMembership.clubName === "" ||
              Number.isNaN(newMembership.membershipNumber)
            }
            onClick={() => {
              if (newMembership.clubName !== "") {
                const externalMembership: ClubMembership = {
                  ...newMembership,
                  membershipNumber: newMembership.membershipNumber,
                  expiry: parseIsoOrThrow(newMembership.expiry),
                };
                addNew(externalMembership);
                setNewMembership(blankState);
              }
            }}
          >
            <FaPlus title="Add" />
            &nbsp; Add
          </Button>
        </Control>
      </Field>
    </Fragment>
  );
};

export default MembershipList;
