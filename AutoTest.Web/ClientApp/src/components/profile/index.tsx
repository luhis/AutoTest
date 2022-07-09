import { h, FunctionComponent } from "preact";
import { Button, Form, Heading } from "react-bulma-components";
const { Label, Input, Field, Radio } = Form;
import { useState } from "preact/hooks";

import { Age, Profile } from "../../types/profileModels";
import { OnChange } from "../../types/inputs";
import EmergencyContactEditor from "../shared/EmergencyContactEditor";
import { EmergencyContact, MsaMembership, Vehicle } from "../../types/shared";
import VehicleEditor from "../shared/VehicleEditor";
import MembershipList from "../shared/MembershipList";
import MsaMembershipEditor from "../shared/MsaMembershipEditor";
import { addPreventDefault } from "../../lib/form";

interface Props {
    readonly profile: Profile;
    readonly save: () => Promise<void>;
    readonly setField: (k: Partial<Profile>) => void;
}

const ProfileComp: FunctionComponent<Props> = ({ save, profile, setField }) => {
    const [saving, setSaving] = useState(false);
    const formSave = addPreventDefault(save, setSaving);
    return (
        <form onSubmit={formSave}>
            <Heading>Profile</Heading>
            <Field>
                <Label>Given Name</Label>
                <Input
                    required
                    value={profile.givenName}
                    onChange={(e: OnChange): void =>
                        setField({ givenName: e.target.value })
                    }
                />
            </Field>
            <Field>
                <Label>Family Name</Label>
                <Input
                    required
                    value={profile.familyName}
                    onChange={(e: OnChange): void =>
                        setField({ familyName: e.target.value })
                    }
                />
            </Field>
            <Field>
                <Label>Age</Label>
                <Radio
                    checked={profile.age === Age.Junior}
                    onChange={() => setField({ age: Age.Junior })}
                >
                    Junior
                </Radio>
                <Radio
                    checked={profile.age === Age.Senior}
                    onChange={() => setField({ age: Age.Senior })}
                >
                    Senior
                </Radio>
            </Field>
            <MsaMembershipEditor
                licenseTypes={[]}
                membership={profile.msaMembership}
                setField={(e: MsaMembership): void =>
                    setField({
                        msaMembership: e,
                    })
                }
            />

            <VehicleEditor
                vehicle={profile.vehicle}
                setField={(e: Vehicle): void =>
                    setField({
                        vehicle: e,
                    })
                }
                makeAndModels={[]}
            />
            <EmergencyContactEditor
                emergencyContact={profile.emergencyContact}
                setField={(e: EmergencyContact): void => {
                    setField({
                        emergencyContact: e,
                    });
                }}
            />
            <MembershipList
                memberships={profile.clubMemberships}
                addNew={(s) => {
                    setField({
                        clubMemberships: profile.clubMemberships.concat(s),
                    });
                }}
                remove={(removeIndex) => {
                    setField({
                        clubMemberships: profile.clubMemberships.filter(
                            (_, i) => i !== removeIndex
                        ),
                    });
                }}
            />
            <Button.Group>
                <Button loading={saving} color="primary" type="submit">
                    Save changes
                </Button>
            </Button.Group>
        </form>
    );
};

export default ProfileComp;
