import { h, FunctionComponent } from "preact";
import { Modal, Button } from "rbx";

import { Notification } from "../../types/models";

interface Props {
    readonly notifications: readonly Notification[];
    readonly cancel: () => void;
}

const NotificationsModal: FunctionComponent<Props> = ({
    notifications,
    cancel,
}) => {
    return (
        <Modal active={true}>
            <Modal.Background />
            <Modal.Card>
                <Modal.Card.Head>Notifications</Modal.Card.Head>
                <Modal.Card.Body>
                    <ul>
                        {notifications.map((a) => (
                            <li key={a.notificationId}>{a.message}</li>
                        ))}
                    </ul>
                </Modal.Card.Body>
                <Modal.Card.Foot>
                    <Button color="secondary" onClick={cancel}>
                        Close
                    </Button>
                </Modal.Card.Foot>
            </Modal.Card>
        </Modal>
    );
};

export default NotificationsModal;
