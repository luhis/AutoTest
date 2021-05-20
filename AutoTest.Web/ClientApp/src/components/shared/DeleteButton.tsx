import { h, FunctionComponent, Fragment } from "preact";
import { useCallback, useState } from "preact/hooks";
import { Button, Modal } from "react-bulma-components";

const DeleteModal: FunctionComponent<{
    readonly deleteFunc: () => void;
    readonly cancel: () => void;
}> = ({ deleteFunc, cancel }) => (
    <Modal show={true} showClose={false}>
        <Modal.Card>
            <Modal.Card.Header showClose={false}>
                <Modal.Card.Title>Delete?</Modal.Card.Title>
            </Modal.Card.Header>
            <Modal.Card.Body>Are you sure you want to delete?</Modal.Card.Body>
            <Modal.Card.Footer>
                <Button color="primary" onClick={deleteFunc}>
                    Delete
                </Button>
                <Button color="secondary" onClick={cancel}>
                    Cancel
                </Button>
            </Modal.Card.Footer>
        </Modal.Card>
    </Modal>
);
interface Props {
    readonly deleteFunc: () => void;
    readonly disabled: boolean;
}

const DeleteButton: FunctionComponent<Props> = ({ deleteFunc, disabled }) => {
    const [showModal, setShowModal] = useState(false);
    const show = useCallback(() => setShowModal(true), []);
    const hide = useCallback(() => setShowModal(false), []);
    return (
        <Fragment>
            <Button disabled={disabled} color="danger" onClick={show}>
                Delete
            </Button>
            {showModal ? (
                <DeleteModal cancel={hide} deleteFunc={deleteFunc} />
            ) : null}
        </Fragment>
    );
};

export default DeleteButton;
