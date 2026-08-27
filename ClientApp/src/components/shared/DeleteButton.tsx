import { h, FunctionComponent, Fragment } from "preact";
import { useCallback, useState } from "preact/hooks";
import { Button, Modal } from "react-bulma-components";

const DeleteModal: FunctionComponent<{
  readonly deleteFunc: () => Promise<void> | void;
  readonly cancel: () => void;
}> = ({ deleteFunc, cancel }) => (
  <Modal show={true} showClose={false}>
    <Modal.Card>
      <Modal.Card.Header showClose={false}>
        <Modal.Card.Title>Confirm Delete</Modal.Card.Title>
      </Modal.Card.Header>
      <Modal.Card.Body>
        <p class="has-text-grey-dark">
          Are you sure you want to delete this item? This action cannot be
          undone.
        </p>
      </Modal.Card.Body>
      <Modal.Card.Footer>
        <Button color="danger" onClick={deleteFunc}>
          Delete
        </Button>
        <Button color="light" onClick={cancel}>
          Cancel
        </Button>
      </Modal.Card.Footer>
    </Modal.Card>
  </Modal>
);
interface Props {
  readonly deleteFunc: () => Promise<void> | void;
  readonly disabled: boolean;
}

const DeleteButton: FunctionComponent<Props> = ({ deleteFunc, disabled }) => {
  const [showModal, setShowModal] = useState(false);
  const show = useCallback(() => setShowModal(true), []);
  const hide = useCallback(() => setShowModal(false), []);
  return (
    <Fragment>
      <Button color="danger is-outlined" disabled={disabled} onClick={show}>
        Delete
      </Button>
      {showModal ? <DeleteModal cancel={hide} deleteFunc={deleteFunc} /> : null}
    </Fragment>
  );
};

export default DeleteButton;
