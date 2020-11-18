import { h, FunctionComponent, Fragment } from "preact";
import { Button, Input, Control, Delete, Field, Level, Label } from "rbx";
import { useState } from "preact/hooks";
import { newValidDate, parseIsoOrThrow } from "ts-date";

import { OnChange } from "../../types/inputs";
import { ClubMembership } from "../../types/profileModels";
import { getDateString } from "../../lib/date";

interface Props {
    memberships: readonly ClubMembership[];
    addNew: (_: ClubMembership) => void;
    remove: (_: number) => void;
}

const blankState = {
    clubName: "",
    membershipNumber: "",
    expiry: newValidDate(), //todo maybe
};

const MembershipList: FunctionComponent<Props> = ({
    memberships,
    addNew,
    remove,
}) => {
    const [newMembership, setNewEmail] = useState<ClubMembership>(blankState);
    return (
        <Fragment>
            <Label>Memberships</Label>
            {memberships.map((a, i) => (
                <Level key={`${a.clubName}`}>
                    <Level.Item align="left">{a.clubName}</Level.Item>
                    <Level.Item align="left">{a.membershipNumber}</Level.Item>
                    <Level.Item align="left">
                        {a.expiry.toLocaleDateString()}
                    </Level.Item>
                    <Level.Item align="right">
                        <Delete onClick={() => remove(i)} />
                    </Level.Item>
                </Level>
            ))}
            <Field kind="addons">
                <Label>Club Name</Label>
                <Control>
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
                <Label>Membership Number</Label>
                <Control>
                    <Input
                        value={newMembership.membershipNumber}
                        onChange={({ target }: OnChange): void =>
                            setNewEmail((e) => ({
                                ...e,
                                membershipNumber: target.value,
                            }))
                        }
                    />
                </Control>
                <Label>Expiry</Label>
                <Control>
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
                            addNew(newMembership);
                            setNewEmail(blankState);
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
