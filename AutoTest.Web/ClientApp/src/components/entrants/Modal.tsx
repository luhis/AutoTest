import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
import { useSelector } from "react-redux";
const { Control, Field, Label, Input, Help, Checkbox } = Form;

import { EditingEntrant } from "../../types/models";
import { OnChange } from "../../types/inputs";
import {
    selectClassOptions,
    selectClubOptions,
    selectLicenseTypeOptions,
    selectMakeModelOptions,
} from "../../store/event/selectors";
import { MsaMembership, EmergencyContact, Vehicle } from "src/types/shared";
import EmergencyContactEditor from "../shared/EmergencyContactEditor";
import VehicleEditor from "../shared/VehicleEditor";
import DropdownInput from "../shared/DropdownInput";
import MsaMembershipEditor from "../shared/MsaMembershipEditor";
import { addPreventDefault } from "../../lib/form";

interface Props {
    readonly entrant: EditingEntrant;
    readonly save: () => void;
    readonly cancel: () => void;
    readonly setField: (k: Partial<EditingEntrant>) => void;
    readonly fillFromProfile: () => void;
}

const EntrantsModal: FunctionComponent<Props> = ({
    save,
    cancel,
    entrant,
    setField,
    fillFromProfile,
}) => {
    const classesInUse = useSelector(selectClassOptions);
    const makeAndModels = useSelector(selectMakeModelOptions);
    const licenseTypes = useSelector(selectLicenseTypeOptions);
    const clubOptions = useSelector(selectClubOptions);

    const formSave = addPreventDefault(save);

    return (
        <Modal show={true} showClose={false}>
            <Modal.Card renderAs="form" onSubmit={formSave}>
                <Modal.Card.Header showClose={false}>
                    <Modal.Card.Title>
                        {entrant.isNew ? "Add" : "Edit"} Entrant
                    </Modal.Card.Title>
                </Modal.Card.Header>
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
                    <Field>
                        <Label>Email</Label>
                        <Input
                            required
                            value={entrant.email}
                            type="email"
                            onChange={(e: OnChange) => {
                                setField({
                                    email: e.target.value,
                                });
                            }}
                        />
                        <Help color="danger">
                            This address is used to manage access to the system,
                            be careful!
                        </Help>
                    </Field>
                    <Field kind="group">
                        <Control fullwidth={true}>
                            <Label>Club</Label>
                            <DropdownInput
                                required
                                value={entrant.club}
                                options={clubOptions}
                                setValue={(e): void => setField({ club: e })}
                            />
                        </Control>
                        <Control fullwidth={true}>
                            <Label>Club Number</Label>
                            <Input
                                required
                                type="number"
                                min={0}
                                step={1}
                                value={entrant.clubNumber}
                                onChange={(e: OnChange): void =>
                                    setField({
                                        clubNumber: Math.floor(
                                            e.target.valueAsNumber
                                        ),
                                    })
                                }
                            />
                        </Control>
                    </Field>
                    <MsaMembershipEditor
                        licenseTypes={licenseTypes}
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
                    <Field>
                        <Control>
                            <Checkbox>
                                {"  "}I agree to the{" "}
                                <a
                                    target="_blank"
                                    href="https://www.motorsportuk.org/wp-content/uploads/2020/06/2021-04-26-signing-on-declaration-competitor-pdf.pdf"
                                    rel="noreferrer"
                                >
                                    Motorsport UK Pre-Event Declaration
                                </a>
                            </Checkbox>
                        </Control>
                    </Field>
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
            </Modal.Card>
        </Modal>
    );
};

export default EntrantsModal;
