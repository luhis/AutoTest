import { h, FunctionComponent } from "preact";
import { Modal, Button, Label, Input, Field } from "rbx";
import { DeepPartial } from "tsdef";

import { Entrant } from "../../types/models";
import { OnChange } from "../../types/inputs";

interface Props {
    entrant: Entrant;
    save: () => Promise<void>;
    cancel: () => void;
    setField: (k: DeepPartial<Entrant>) => void;
}

const EntrantsModal: FunctionComponent<Props> = ({
    save,
    cancel,
    entrant,
    setField,
}) => {
    return (
        <Modal active={true}>
            <Modal.Background />
            <Modal.Card>
                <Modal.Card.Head>
                    {entrant.entrantId === undefined ? "Add" : "Edit"} Entrant
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
                        <Label>Class</Label>
                        <Input
                            value={entrant.class}
                            onChange={(e: OnChange): void =>
                                setField({
                                    class: e.target.value,
                                })
                            }
                        />
                    </Field>
                    <Field>
                        <Label>Registration</Label>
                        <Input
                            value={entrant.vehicle.registration}
                            onChange={(e: OnChange): void =>
                                setField({
                                    vehicle: {
                                        registration: e.target.value.toLocaleUpperCase(),
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

export default EntrantsModal;
