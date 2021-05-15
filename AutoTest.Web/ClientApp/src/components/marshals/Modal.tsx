import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
const { Control, Field, Label, Input } = Form;

import { EditingMarshal } from "../../types/models";
import { OnChange } from "../../types/inputs";

import { EmergencyContact } from "src/types/shared";
import EmergencyContactEditor from "../shared/EmergencyContactEditor";
import { addPreventDefault } from "../../lib/form";

interface Props {
    readonly entrant: EditingMarshal;
    readonly save: () => void;
    readonly cancel: () => void;
    readonly setField: (k: Partial<EditingMarshal>) => void;
    readonly fillFromProfile: () => void;
}

const EntrantsModal: FunctionComponent<Props> = ({
    save,
    cancel,
    entrant,
    setField,
    fillFromProfile,
}) => {
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
