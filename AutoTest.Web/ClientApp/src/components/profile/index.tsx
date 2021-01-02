import { h, FunctionComponent, Fragment } from "preact";
import { Button, Label, Input, Field, Title } from "rbx";

import { Profile } from "../../types/profileModels";
import { OnChange } from "../../types/inputs";
import EmergencyContactEditor from "../shared/EmergencyContactEditor";
import { EmergencyContact, Vehicle } from "../../types/shared";
import VehicleEditor from "../shared/VehicleEditor";
import MembershipList from "../shared/MembershipList";

interface Props {
    readonly profile: Profile;
    readonly save: () => void;
    readonly cancel: () => void;
    readonly setField: (k: Partial<Profile>) => void;
}

const ProfileComp: FunctionComponent<Props> = ({
    save,
    cancel,
    profile,
    setField,
}) => {
    return (
        <Fragment>
            <Title>Profile</Title>
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
                <Button color="secondary" onClick={cancel}>
                    Close
                </Button>
            </Button.Group>
        </Fragment>
    );
};

export default ProfileComp;
