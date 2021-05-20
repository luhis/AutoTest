import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
const { Control, Field, Label, Input, Help, Checkbox } = Form;

import { EditingMarshal } from "../../types/models";
import { OnChange } from "../../types/inputs";

import { EmergencyContact } from "src/types/shared";
import EmergencyContactEditor from "../shared/EmergencyContactEditor";
import { addPreventDefault } from "../../lib/form";
import DropdownInput from "../shared/DropdownInput";

interface Props {
    readonly marshal: EditingMarshal;
    readonly allRoles: readonly string[];
    readonly save: () => void;
    readonly cancel: () => void;
    readonly setField: (k: Partial<EditingMarshal>) => void;
    readonly fillFromProfile: () => void;
}

const MarshalsModal: FunctionComponent<Props> = ({
    save,
    cancel,
    marshal,
    allRoles,
    setField,
    fillFromProfile,
}) => {
    const formSave = addPreventDefault(save);

    return (
        <Modal show={true} showClose={false}>
            <Modal.Card renderAs="form" onSubmit={formSave}>
                <Modal.Card.Header showClose={false}>
                    <Modal.Card.Title>
                        {marshal.isNew ? "Add" : "Edit"} Marshal
                    </Modal.Card.Title>
                </Modal.Card.Header>
                <Modal.Card.Body>
                    <Field kind="group">
                        <Control fullwidth={true}>
                            <Label>Given Name</Label>
                            <Input
                                required
                                value={marshal.givenName}
                                onChange={(e: OnChange): void =>
                                    setField({
                                        givenName: e.target.value,
                                    })
                                }
                            />
                        </Control>
                        <Control fullwidth={true}>
                            <Label>Family Name</Label>
                            <Input
                                required
                                value={marshal.familyName}
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
                            value={marshal.email}
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
                    <EmergencyContactEditor
                        emergencyContact={marshal.emergencyContact}
                        setField={(e: EmergencyContact): void =>
                            setField({
                                emergencyContact: e,
                            })
                        }
                    />
                    <Field>
                        <Label>Registration</Label>
                        <Input
                            required
                            type="number"
                            value={marshal.registrationNumber}
                            onChange={({ target }: OnChange): void =>
                                setField({
                                    ...marshal,
                                    registrationNumber: Math.floor(
                                        target.valueAsNumber
                                    ),
                                })
                            }
                        />
                    </Field>
                    <Field>
                        <Label>Role</Label>
                        <DropdownInput
                            required
                            options={allRoles}
                            value={marshal.role}
                            setValue={(e): void =>
                                setField({
                                    ...marshal,
                                    role: e,
                                })
                            }
                        />
                    </Field>
                    <Field>
                        <Control>
                            <Checkbox>
                                {"  "}I agree to the{" "}
                                <a
                                    target="_blank"
                                    href="https://www.motorsportuk.org/wp-content/uploads/2020/06/2021-04-26-signing-on-declaration-officials-pdf.pdf"
                                    rel="noreferrer"
                                >
                                    Motorsport UK Pre-Event Declaration Form
                                </a>
                            </Checkbox>
                        </Control>
                    </Field>
                </Modal.Card.Body>
                <Modal.Card.Footer>
                    <Button type="submit" color="primary">
                        Save changes
                    </Button>
                    {marshal.isNew ? (
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

export default MarshalsModal;
