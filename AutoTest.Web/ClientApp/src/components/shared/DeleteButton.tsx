import { h, FunctionComponent, Fragment } from "preact";
import { useCallback, useState } from "preact/hooks";
import { Button, Modal } from "rbx";

const DeleteModal: FunctionComponent<{
    readonly deleteFunc: () => void;
    readonly cancel: () => void;
}> = ({ deleteFunc, cancel }) => (
    <Modal active={true}>
        <Modal.Background />
        <Modal.Card>
            <Modal.Card.Head>Delete?</Modal.Card.Head>
            <Modal.Card.Body>Are you sure you want to delete?</Modal.Card.Body>
            <Modal.Card.Foot>
                <Button color="primary" onClick={deleteFunc}>
                    Delete
                </Button>
                <Button color="secondary" onClick={cancel}>
                    Cancel
                </Button>
            </Modal.Card.Foot>
        </Modal.Card>
    </Modal>
);
interface Props {
    readonly deleteFunc: () => void;
}

const DeleteButton: FunctionComponent<Props> = ({ deleteFunc }) => {
    const [showModal, setShowModal] = useState(false);
    const show = useCallback(() => setShowModal(true), []);
    const hide = useCallback(() => setShowModal(false), []);
    return (
        <Fragment>
            <Button onClick={show}>Delete</Button>
            {showModal ? (
                <DeleteModal cancel={hide} deleteFunc={deleteFunc} />
            ) : null}
        </Fragment>
    );
};

export default DeleteButton;
