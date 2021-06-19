import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
const { Input, Field, Label } = Form;
import { useState } from "preact/hooks";

import { OnChange } from "src/types/inputs";
import { EventNotification } from "../../types/models";
import { addPreventDefault } from "../../lib/form";

interface Props {
    readonly notification: EventNotification;
    readonly setField: (k: Partial<EventNotification>) => void;
    readonly save: () => void;
    readonly cancel: () => void;
}

const AddNotificationModal: FunctionComponent<Props> = ({
    notification,
    setField,
    save,
    cancel,
}) => {
    const [saving, setSaving] = useState(false);
    const formSave = addPreventDefault(save, setSaving);
    return (
        <Modal show={true} showClose={false}>
            <Modal.Card renderAs="form" onSubmit={formSave}>
                <Modal.Card.Header showClose={false}>
                    <Modal.Card.Title>Notifications</Modal.Card.Title>
                </Modal.Card.Header>
                <Modal.Card.Body>
                    <Field>
                        <Label>Notification</Label>
                        <Input
                            required
                            value={notification.message}
                            onChange={(e: OnChange): void =>
                                setField({ message: e.target.value })
                            }
                        />
                    </Field>
                </Modal.Card.Body>
                <Modal.Card.Footer>
                    <Button loading={saving} color="primary" type="submit">
                        Save
                    </Button>
                    <Button color="secondary" onClick={cancel}>
                        Close
                    </Button>
                </Modal.Card.Footer>
            </Modal.Card>
        </Modal>
    );
};

export default AddNotificationModal;
