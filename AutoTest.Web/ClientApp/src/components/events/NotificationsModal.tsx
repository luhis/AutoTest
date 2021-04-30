import { h, FunctionComponent } from "preact";
import { Modal, Button, Heading } from "react-bulma-components";
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
        <Modal show={true} showClose={false}>
            <Modal.Card>
                <Modal.Card.Header showClose={false}>
                    Notifications
                </Modal.Card.Header>
                <Modal.Card.Body>
                    <ul>
                        {notifications.map((a) => (
                            <li key={a.notificationId}>
                                <Heading size={6}>
                                    <TimeAgo datetime={a.created} />
                                </Heading>
                                <p>{a.message}</p>
                            </li>
                        ))}
                    </ul>
                </Modal.Card.Body>
                <Modal.Card.Footer>
                    <Button color="secondary" onClick={cancel}>
                        Close
                    </Button>
                </Modal.Card.Footer>
            </Modal.Card>
        </Modal>
    );
};

export default NotificationsModal;
