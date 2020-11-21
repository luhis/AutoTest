import { h, FunctionComponent } from "preact";
import { Modal, Button } from "rbx";
import save from "save-file";

import { Event } from "../../types/models";

interface Props {
    readonly event: Event;
    readonly cancel: () => void;
}

const RegsModal: FunctionComponent<Props> = ({ event, cancel }) => {
    return (
        <Modal active={true}>
            <Modal.Background />
            <Modal.Card>
                <Modal.Card.Head>{event.location}</Modal.Card.Head>
                <Modal.Card.Body>
                    {event.regulations ? (
                        <Button
                            onClick={() => save(event.regulations, "regs.pdf")}
                        >
                            Download
                        </Button>
                    ) : null}
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

export default RegsModal;
