import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
import { useSelector } from "react-redux";
const { Control, Field, Label, Input } = Form;

import { Entrant, EditingEntrant } from "../../types/models";
import { OnChange } from "../../types/inputs";
import {
    selectClassOptions,
    selectMakeModelOptions,
} from "../../store/event/selectors";
import { EmergencyContact, Vehicle } from "src/types/shared";
import EmergencyContactEditor from "../shared/EmergencyContactEditor";
import VehicleEditor from "../shared/VehicleEditor";
import DropdownInput from "../shared/DropdownInput";
import { startsWithIgnoreCase } from "../../lib/string";

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
        (c) => startsWithIgnoreCase(c, entrant.class) && c !== entrant.class
    );
    const makeAndModels = useSelector(selectMakeModelOptions);

    return (
        <Modal show={true} showClose={false}>
            <Modal.Card>
                <Modal.Card.Header showClose={false}>
                    {entrant.isNew ? "Add" : "Edit"} Entrant
                </Modal.Card.Header>
                <Modal.Card.Body>
                    <Field kind="group">
                        <Control fullwidth={true}>
                            <Label>Given Name</Label>
                            <Control>
                                <Input
                                    value={entrant.givenName}
                                    onChange={(e: OnChange): void =>
                                        setField({ givenName: e.target.value })
                                    }
                                />
                            </Control>
                        </Control>
                        <Control fullwidth={true}>
                            <Label>Family Name</Label>
                            <Input
                                value={entrant.familyName}
                                onChange={(e: OnChange): void =>
                                    setField({ familyName: e.target.value })
                                }
                            />
                        </Control>
                    </Field>
                    <Field kind="group">
                        <Control fullwidth={true}>
                            <Label>Club</Label>
                            <Input
                                value={entrant.club}
                                onChange={(e: OnChange): void =>
                                    setField({ club: e.target.value })
                                }
                            />
                        </Control>
                        <Control fullwidth={true}>
                            <Label>Club Number</Label>
                            <Input
                                type="number"
                                min={0}
                                value={entrant.clubNumber}
                                onChange={(e: OnChange): void =>
                                    setField({
                                        clubNumber: Number.parseInt(
                                            e.target.value
                                        ),
                                    })
                                }
                            />
                        </Control>
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
                        <DropdownInput
                            value={entrant.class}
                            options={classesInUse}
                            setValue={(e) => {
                                setField({
                                    class: e.toLocaleUpperCase(),
                                });
                            }}
                        />
                    </Field>
                    <VehicleEditor
                        vehicle={entrant.vehicle}
                        setField={(e: Vehicle): void =>
                            setField({
                                vehicle: e,
                            })
                        }
                        makeAndModels={makeAndModels}
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
