import { h, FunctionComponent } from "preact";
import { Modal, Button, Heading } from "react-bulma-components";
import TimeAgo from "javascript-time-ago";
import { ValidDate } from "ts-date";

import { EventNotification } from "../../types/models";

interface Props {
  readonly notifications: readonly EventNotification[];
  readonly cancel: () => void;
}

import en from "javascript-time-ago/locale/en";

TimeAgo.addDefaultLocale(en);

const timeAgo = new TimeAgo("en-US");

const formatX = (d: ValidDate): string => timeAgo.format(d);

const NotificationsModal: FunctionComponent<Props> = ({
  notifications,
  cancel,
}) => {
  return (
    <Modal show={true} showClose={false}>
      <Modal.Card>
        <Modal.Card.Header showClose={false}>
          <Modal.Card.Title>Notifications</Modal.Card.Title>
        </Modal.Card.Header>
        <Modal.Card.Body>
          <ul>
            {notifications.map((a) => (
              <li key={a.notificationId}>
                <Heading size={6}>{formatX(a.created)}</Heading>
                <p>{a.message}</p>
              </li>
            ))}
          </ul>
        </Modal.Card.Body>
        <Modal.Card.Footer>
          <Button type="button" color="secondary" onClick={cancel}>
            Cancel
          </Button>
        </Modal.Card.Footer>
      </Modal.Card>
    </Modal>
  );
};

export default NotificationsModal;
