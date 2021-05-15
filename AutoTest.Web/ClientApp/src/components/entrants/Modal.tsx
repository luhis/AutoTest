import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
import { useSelector } from "react-redux";
const { Control, Field, Label, Input } = Form;

import { EditingEntrant } from "../../types/models";
import { OnChange } from "../../types/inputs";
import {
    selectClassOptions,
    selectMakeModelOptions,
} from "../../store/event/selectors";
import { MsaMembership, EmergencyContact, Vehicle } from "src/types/shared";
import EmergencyContactEditor from "../shared/EmergencyContactEditor";
import VehicleEditor from "../shared/VehicleEditor";
import DropdownInput from "../shared/DropdownInput";
import { startsWithIgnoreCase } from "../../lib/string";
import MsaMembershipEditor from "../shared/MsaMembershipEditor";
import { addPreventDefault } from "../../lib/form";

interface Props {
    readonly entrant: EditingEntrant;
    readonly save: () => void;
    readonly cancel: () => void;
    readonly setField: (
        k: Partial<Omit<EditingEntrant, "driverNumber">>
    ) => void;
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

    const formSave = addPreventDefault(save);

    return (
        <Modal show={true} showClose={false}>
            <Modal.Card>
                <Modal.Card.Header showClose={false}>
                    {entrant.isNew ? "Add" : "Edit"} Entrant
                </Modal.Card.Header>
                <form onSubmit={formSave}>
                    <Modal.Card.Body>
                        <Field kind="group">
                            <Control fullwidth={true}>
                                <Label>Given Name</Label>
                                <Control>
                                    <Input
                                        required
                                        value={entrant.givenName}
                                        onChange={(e: OnChange): void =>
                                            setField({
                                                givenName: e.target.value,
                                            })
                                        }
                                    />
                                </Control>
                            </Control>
                            <Control fullwidth={true}>
                                <Label>Family Name</Label>
                                <Input
                                    required
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
                                    required
                                    value={entrant.club}
                                    onChange={(e: OnChange): void =>
                                        setField({ club: e.target.value })
                                    }
                                />
                            </Control>
                            <Control fullwidth={true}>
                                <Label>Club Number</Label>
                                <Input
                                    required
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
                        <MsaMembershipEditor
                            membership={entrant.msaMembership}
                            setField={(e: MsaMembership): void =>
                                setField({
                                    msaMembership: e,
                                })
                            }
                        />
                        <Field>
                            <Label>Class</Label>
                            <DropdownInput
                                required
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
                        <Button type="submit" color="primary">
                            Save changes
                        </Button>
                        {entrant.isNew ? (
                            <Button
                                color="secondary"
                                type="button"
                                onClick={fillFromProfile}
                            >
                                Fill from Profile
                            </Button>
                        ) : null}
                        <Button color="secondary" onClick={cancel}>
                            Close
                        </Button>
                    </Modal.Card.Footer>
                </form>
            </Modal.Card>
        </Modal>
    );
};

export default EntrantsModal;
