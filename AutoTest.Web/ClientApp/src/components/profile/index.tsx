import { h, FunctionComponent } from "preact";
import { Modal, Button, Label, Input, Field } from "rbx";

import { Profile } from "../../types/profileModels";
import { OnChange } from "../../types/inputs";
import EmergencyContactEditor from "../shared/EmergencyContactEditor";
import { EmergencyContact, Vehicle } from "../../types/shared";
import VehicleEditor from "../shared/VehicleEditor";
import MembershipList from "../shared/MembershipList";

interface Props {
    profile: Profile;
    save: () => Promise<void>;
    cancel: () => void;
    setField: (k: Partial<Profile>) => void;
}

const ProfileModal: FunctionComponent<Readonly<Props>> = ({
    save,
    cancel,
    profile,
    setField,
}) => {
    return (
        <Modal active={true}>
            <Modal.Background />
            <Modal.Card>
                <Modal.Card.Head>Profile</Modal.Card.Head>
                <Modal.Card.Body>
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
                                clubMemberships: profile.clubMemberships.concat(
                                    s
                                ),
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
                </Modal.Card.Body>
                <Modal.Card.Foot>
                    <Button color="primary" onClick={save}>
                        Save changes
                    </Button>
                    <Button color="secondary" onClick={cancel}>
                        Close
                    </Button>
                </Modal.Card.Foot>
            </Modal.Card>
        </Modal>
    );
};

export default ProfileModal;
