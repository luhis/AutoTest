import { h, FunctionComponent } from "preact";
import { Modal, Button, Label, Input, Field } from "rbx";
import { useSelector } from "react-redux";

import { Entrant, EditingEntrant } from "../../types/models";
import { OnChange } from "../../types/inputs";
import { selectClassOptions } from "../../store/event/selectors";
import { EmergencyContact, Vehicle } from "src/types/shared";
import EmergencyContactEditor from "../shared/EmergencyContactEditor";
import VehicleEditor from "../shared/VehicleEditor";

interface Props {
    readonly entrant: EditingEntrant;
    readonly save: () => Promise<void>;
    readonly cancel: () => void;
    readonly setField: (k: Partial<Omit<Entrant, "driverNumber">>) => void;
    readonly fillFromProfile: () => void;
}

const EntrantsModal: FunctionComponent<Props> = ({
    save,
    cancel,
    entrant,
    setField,
    fillFromProfile,
}) => {
    const classesInUse = useSelector(selectClassOptions).filter(
        (c) => c.startsWith(entrant.class) && c !== entrant.class
    );
    return (
        <Modal active={true}>
            <Modal.Background />
            <Modal.Card>
                <Modal.Card.Head>
                    {entrant.isNew ? "Add" : "Edit"} Entrant
                </Modal.Card.Head>
                <Modal.Card.Body>
                    <Field>
                        <Label>Given Name</Label>
                        <Input
                            value={entrant.givenName}
                            onChange={(e: OnChange): void =>
                                setField({ givenName: e.target.value })
                            }
                        />
                    </Field>
                    <Field>
                        <Label>Family Name</Label>
                        <Input
                            value={entrant.familyName}
                            onChange={(e: OnChange): void =>
                                setField({ familyName: e.target.value })
                            }
                        />
                    </Field>
                    <Field>
                        <Label>Club</Label>
                        <Input
                            value={entrant.club}
                            onChange={(e: OnChange): void =>
                                setField({ club: e.target.value })
                            }
                        />
                    </Field>
                    <Field>
                        <Label>MSA License</Label>
                        <Input
                            value={entrant.msaLicense}
                            onChange={(e: OnChange): void =>
                                setField({ msaLicense: e.target.value })
                            }
                        />
                    </Field>
                    <Field>
                        <Label>Class</Label>
                        <Input
                            list="classes"
                            value={entrant.class}
                            onChange={(e: OnChange): void => {
                                setField({
                                    class: e.target.value.toLocaleUpperCase(),
                                });
                            }}
                        >
                            <datalist id="classes">
                                {classesInUse.map((a) => (
                                    <option
                                        key={a}
                                        value={a}
                                        onClick={() => {
                                            setField({
                                                class: a,
                                            });
                                        }}
                                    />
                                ))}
                            </datalist>
                        </Input>
                    </Field>
                    <VehicleEditor
                        vehicle={entrant.vehicle}
                        setField={(e: Vehicle): void =>
                            setField({
                                vehicle: e,
                            })
                        }
                    />
                    <EmergencyContactEditor
                        emergencyContact={entrant.emergencyContact}
                        setField={(e: EmergencyContact): void =>
                            setField({
                                emergencyContact: e,
                            })
                        }
                    />
                </Modal.Card.Body>
                <Modal.Card.Foot>
                    <Button color="primary" onClick={save}>
                        Save changes
                    </Button>
                    {entrant.isNew ? (
                        <Button color="secondary" onClick={fillFromProfile}>
                            Fill from Profile
                        </Button>
                    ) : null}
                    <Button color="secondary" onClick={cancel}>
                        Close
                    </Button>
                </Modal.Card.Foot>
            </Modal.Card>
        </Modal>
    );
};

export default EntrantsModal;
