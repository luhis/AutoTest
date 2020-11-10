import { h, FunctionComponent } from "preact";
import { Modal, Button, Label, Input, Field } from "rbx";
import { DeepPartial } from "tsdef";

import { Profile } from "../../types/models";
import { OnChange } from "../../types/inputs";

interface Props {
    profile: Profile;
    save: () => Promise<void>;
    cancel: () => void;
    setField: (k: DeepPartial<Omit<Profile, "driverNumber">>) => void;
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
                    <Field>
                        <Label>Registration</Label>
                        <Input
                            value={profile.vehicle.registration}
                            onChange={(e: OnChange): void =>
                                setField({
                                    vehicle: {
                                        registration: e.target.value.toLocaleUpperCase(),
                                    },
                                })
                            }
                        />
                    </Field>
                    <Field>
                        <Label>Displacement (CC)</Label>
                        <Input
                            type="number"
                            value={profile.vehicle.displacement}
                            onChange={(e: OnChange): void =>
                                setField({
                                    vehicle: {
                                        displacement: Number.parseInt(
                                            e.target.value
                                        ),
                                    },
                                })
                            }
                            min={0}
                        />
                    </Field>
                    <Field>
                        <Label>Emergency Contact Name</Label>
                        <Input
                            value={profile.emergencyContact.name}
                            onChange={(e: OnChange): void => {
                                setField({
                                    emergencyContact: {
                                        name: e.target.value,
                                    },
                                });
                            }}
                        />
                    </Field>
                    <Field>
                        <Label>Emergency Contact Number</Label>
                        <Input
                            type="tel"
                            value={profile.emergencyContact.phone}
                            onChange={(e: OnChange): void =>
                                setField({
                                    emergencyContact: {
                                        phone: e.target.value,
                                    },
                                })
                            }
                        />
                    </Field>
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
