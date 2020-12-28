import { h, FunctionComponent } from "preact";
import { Modal, Button, Title } from "rbx";
import TimeAgo from "timeago-react";

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
                            <li key={a.notificationId}>
                                <Title size={6}>
                                    <TimeAgo datetime={a.created} />
                                </Title>
                                <p>{a.message}</p>
                            </li>
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
