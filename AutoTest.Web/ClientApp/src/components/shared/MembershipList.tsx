import { h, FunctionComponent, Fragment } from "preact";
import { Button, Form, Level } from "react-bulma-components";
import { useState } from "preact/hooks";
import { addYear, newValidDate, parseIsoOrThrow } from "ts-date";
const { Input, Control, Field, Label } = Form;

import { OnChange } from "../../types/inputs";
import { getDateString } from "../../lib/date";
import { ClubMembership } from "../../types/shared";
import { Override } from "src/types/models";

interface Props {
    readonly memberships: readonly ClubMembership[];
    readonly addNew: (_: ClubMembership) => void;
    readonly remove: (_: number) => void;
}

type EditingMembership = Override<
    ClubMembership,
    { readonly membershipNumber: number | "" }
>;

const blankState = (): EditingMembership => ({
    clubName: "",
    membershipNumber: "",
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
                        value={newMembership.membershipNumber}
                        onChange={({ target }: OnChange): void =>
                            setNewEmail((e) => ({
                                ...e,
                                membershipNumber: Number.parseInt(target.value),
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
                        onClick={() => {
                            if (newMembership.clubName !== "") {
                                const externalMembership: ClubMembership = {
                                    ...newMembership,
                                    membershipNumber:
                                        newMembership.membershipNumber === ""
                                            ? 0
                                            : newMembership.membershipNumber,
                                };
                                addNew(externalMembership);
                                setNewEmail(blankState);
                            }
                        }}
                    >
                        Add
                    </Button>
                </Control>
            </Field>
        </Fragment>
    );
};

export default MembershipList;
