import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
import { useSelector } from "react-redux";

import { Entrant, EditingEntrant } from "../../types/models";
import { OnChange } from "../../types/inputs";
import { selectClassOptions } from "../../store/event/selectors";
import { EmergencyContact, Vehicle } from "src/types/shared";
import EmergencyContactEditor from "../shared/EmergencyContactEditor";
import VehicleEditor from "../shared/VehicleEditor";

interface Props {
    readonly entrant: EditingEntrant;
    readonly save: () => void;
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
        <Modal show={true} showClose={false}>
            <Modal.Card>
                <Modal.Card.Header showClose={false}>
                    {entrant.isNew ? "Add" : "Edit"} Entrant
                </Modal.Card.Header>
                <Modal.Card.Body>
                    <Form.Field horizontal>
                        <Form.Field>
                            <Form.Label>Given Name</Form.Label>
                            <Form.Input
                                value={entrant.givenName}
                                onChange={(e: OnChange): void =>
                                    setField({ givenName: e.target.value })
                                }
                            />
                        </Form.Field>
                        <Form.Field>
                            <Form.Label>Family Name</Form.Label>
                            <Form.Input
                                value={entrant.familyName}
                                onChange={(e: OnChange): void =>
                                    setField({ familyName: e.target.value })
                                }
                            />
                        </Form.Field>
                    </Form.Field>
                    <Form.Field>
                        <Form.Label>Club</Form.Label>
                        <Form.Input
                            value={entrant.club}
                            onChange={(e: OnChange): void =>
                                setField({ club: e.target.value })
                            }
                        />
                    </Form.Field>
                    <Form.Field>
                        <Form.Label>MSA License</Form.Label>
                        <Form.Input
                            value={entrant.msaLicense}
                            onChange={(e: OnChange): void =>
                                setField({ msaLicense: e.target.value })
                            }
                        />
                    </Form.Field>
                    <Form.Field>
                        <Form.Label>Class</Form.Label>
                        <Form.Input
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
                        </Form.Input>
                    </Form.Field>
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
                <Modal.Card.Footer>
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
                </Modal.Card.Footer>
            </Modal.Card>
        </Modal>
    );
};

export default EntrantsModal;
