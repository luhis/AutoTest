import { h, FunctionComponent } from "preact";
import { Modal, Button, Field, Input, Label } from "rbx";
import { OnChange } from "src/types/inputs";

import { Notification } from "../../types/models";

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
    return (
        <Modal active={true}>
            <Modal.Background />
            <Modal.Card>
                <Modal.Card.Head>Notifications</Modal.Card.Head>
                <Modal.Card.Body>
                    <Field>
                        <Label>Notification</Label>
                        <Input
                            value={notification.message}
                            onChange={(e: OnChange): void =>
                                setField({ message: e.target.value })
                            }
                        />
                    </Field>
                </Modal.Card.Body>
                <Modal.Card.Foot>
                    <Button color="primary" onClick={save}>
                        Save
                    </Button>
                    <Button color="secondary" onClick={cancel}>
                        Close
                    </Button>
                </Modal.Card.Foot>
            </Modal.Card>
        </Modal>
    );
};

export default AddNotificationModal;
