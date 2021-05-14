import { h, FunctionComponent, Fragment } from "preact";
import { Button, Form, Heading } from "react-bulma-components";
const { Label, Input, Field } = Form;

import { Profile } from "../../types/profileModels";
import { OnChange } from "../../types/inputs";
import EmergencyContactEditor from "../shared/EmergencyContactEditor";
import { EmergencyContact, Vehicle } from "../../types/shared";
import VehicleEditor from "../shared/VehicleEditor";
import MembershipList from "../shared/MembershipList";

interface Props {
    readonly profile: Profile;
    readonly save: () => void;
    readonly setField: (k: Partial<Profile>) => void;
}

const ProfileComp: FunctionComponent<Props> = ({ save, profile, setField }) => {
    return (
        <Fragment>
            <Heading>Profile</Heading>
            <Field>
                <Label>Given Name</Label>
                <Input
                    value={profile.givenName}
                    onChange={(e: OnChange): void =>
                        setField({ givenName: e.target.value })
                    }
                />
            </Field>
            <Field>
                <Label>Family Name</Label>
                <Input
                    value={profile.familyName}
                    onChange={(e: OnChange): void =>
                        setField({ familyName: e.target.value })
                    }
                />
            </Field>
            <Field>
                <Label>MSA License</Label>
                <Input
                    value={profile.msaLicense}
                    onChange={(e: OnChange): void =>
                        setField({ msaLicense: e.target.value })
                    }
                />
            </Field>
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
                <Button color="primary" onClick={save}>
                    Save changes
                </Button>
            </Button.Group>
        </Fragment>
    );
};

export default ProfileComp;
