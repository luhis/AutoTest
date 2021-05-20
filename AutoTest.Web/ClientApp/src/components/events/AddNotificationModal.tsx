import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
const { Input, Field, Label } = Form;

import { OnChange } from "src/types/inputs";
import { Notification } from "../../types/models";
import { addPreventDefault } from "../../lib/form";

interface Props {
    readonly notification: Notification;
    readonly setField: (k: Partial<Notification>) => void;
    readonly save: () => void;
    readonly cancel: () => void;
}

const AddNotificationModal: FunctionComponent<Props> = ({
    notification,
    setField,
    save,
    cancel,
}) => {
    const formSave = addPreventDefault(save);
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
                    <Button color="primary" type="submit">
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
