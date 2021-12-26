import { h, FunctionComponent, Fragment } from "preact";
import { Button, Form, Level } from "react-bulma-components";
import { useState } from "preact/hooks";
import { addYear, newValidDate, parseIsoOrThrow } from "ts-date";
const { Input, Control, Field, Label } = Form;
import { FaPlus } from "react-icons/fa";

import { OnChange } from "../../types/inputs";
import { getDateString } from "../../lib/date";
import { ClubMembership } from "../../types/shared";

interface Props {
    readonly memberships: readonly ClubMembership[];
    readonly addNew: (_: ClubMembership) => void;
    readonly remove: (_: number) => void;
}

type EditingMembership = ClubMembership;

const blankState = (): EditingMembership => ({
    clubName: "",
    membershipNumber: Number.NaN,
    expiry: addYear(newValidDate(), 1),
});

const MembershipList: FunctionComponent<Props> = ({
    memberships,
    addNew,
    remove,
}) => {
    const [newMembership, setNewEmail] =
        useState<EditingMembership>(blankState);
    return (
        <Fragment>
            <Label>Memberships</Label>
            {memberships.map((a, i) => (
                <Level key={`${a.clubName}`}>
                    <Level.Item align="left">{a.clubName}</Level.Item>
                    <Level.Item align="left">{a.membershipNumber}</Level.Item>
                    <Level.Item
                        align="left"
                        textColor={
                            a.expiry < newValidDate() ? "danger" : undefined
                        }
                    >
                        {a.expiry.toLocaleDateString()}
                    </Level.Item>
                    <Level.Item align="right">
                        <Button remove onClick={() => remove(i)} />
                    </Level.Item>
                </Level>
            ))}
            <Field kind="group">
                <Control fullwidth={true}>
                    <Label>Club Name</Label>
                    <Input
                        value={newMembership.clubName}
                        onChange={({ target }: OnChange): void =>
                            setNewEmail((e) => ({
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
                        onChange={({ target }: OnChange): void =>
                            setNewEmail((e) => ({
                                ...e,
                                membershipNumber: Math.floor(
                                    target.valueAsNumber
                                ),
                            }))
                        }
                    />
                </Control>
                <Control fullwidth={true}>
                    <Label>Expiry</Label>
                    <Input
                        type="date"
                        value={getDateString(newMembership.expiry)}
                        onChange={({ target }: OnChange): void =>
                            setNewEmail((e) => ({
                                ...e,
                                expiry: parseIsoOrThrow(target.value),
                            }))
                        }
                    />
                </Control>
                <Control>
                    <Button
                        disabled={
                            newMembership.clubName === "" ||
                            Number.isNaN(newMembership.membershipNumber)
                        }
                        onClick={() => {
                            if (newMembership.clubName !== "") {
                                const externalMembership: ClubMembership = {
                                    ...newMembership,
                                    membershipNumber:
                                        newMembership.membershipNumber,
                                };
                                addNew(externalMembership);
                                setNewEmail(blankState);
                            }
                        }}
                    >
                        <FaPlus title="Add" />
                        Add
                    </Button>
                </Control>
            </Field>
        </Fragment>
    );
};

export default MembershipList;
