import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
const { Control, Field, Label, Input, Help, Checkbox } = Form;

import { EditingMarshal } from "../../types/models";
import { OnChange } from "../../types/inputs";

import { EmergencyContact } from "src/types/shared";
import EmergencyContactEditor from "../shared/EmergencyContactEditor";
import { addPreventDefault } from "../../lib/form";

interface Props {
    readonly marshal: EditingMarshal;
    readonly save: () => void;
    readonly cancel: () => void;
    readonly setField: (k: Partial<EditingMarshal>) => void;
    readonly fillFromProfile: () => void;
}

const MarshalsModal: FunctionComponent<Props> = ({
    save,
    cancel,
    marshal,
    setField,
    fillFromProfile,
}) => {
    const formSave = addPreventDefault(save);

    return (
        <Modal show={true} showClose={false}>
            <Modal.Card>
                <Modal.Card.Header showClose={false}>
                    {marshal.isNew ? "Add" : "Edit"} Marshal
                </Modal.Card.Header>
                <form onSubmit={formSave}>
                    <Modal.Card.Body>
                        <Field kind="group">
                            <Control fullwidth={true}>
                                <Label>Given Name</Label>
                                <Control>
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
                            <Help>
                                This address is used to manage access to the
                                system, be careful!
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
                                        registrationNumber:
                                            target.valueAsNumber,
                                    })
                                }
                            />
                        </Field>
                        <Field>
                            <Label>Role</Label>
                            <Input
                                required
                                value={marshal.role}
                                onChange={({ target }: OnChange): void =>
                                    setField({
                                        ...marshal,
                                        role: target.value,
                                    })
                                }
                            />
                        </Field>
                        <Field>
                            <Control>
                                <Checkbox>
                                    {"  "}I agree to the{" "}
                                    <a href="#">terms and conditions</a>
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
                </form>
            </Modal.Card>
        </Modal>
    );
};

export default MarshalsModal;
