import { h, FunctionComponent } from "preact";
import { Modal, Button, Label, Input, Field } from "rbx";
import { newValidDateOrThrow } from "ts-date";

import { Event } from "../../types/models";
import { OnChange } from "../../types/inputs";

interface Props {
    event: Event;
    save: () => Promise<void>;
    cancel: () => void;
    setField: (k: Partial<Event>) => void;
}

const ModalX: FunctionComponent<Props> = ({
    save,
    cancel,
    event,
    setField,
}) => {
    return (
        <Modal active={true}>
            <Modal.Background />
            <Modal.Card>
                <Modal.Card.Head>
                    {event.clubId === undefined ? "Add" : "Edit"} Event
                </Modal.Card.Head>
                <Modal.Card.Body>
                    <Field>
                        <Label>Location</Label>
                        <Input
                            value={event.location}
                            onChange={(e: OnChange): void =>
                                setField({ location: e.target.value })
                            }
                        />
                    </Field>
                    <Field>
                        <Label>Test Count</Label>
                        <Input
                            type="number"
                            value={event.testCount}
                            onChange={(e: OnChange): void =>
                                setField({ testCount: Number(e.target.value) })
                            }
                        />
                    </Field>
                    <Field>
                        <Label>Attempts</Label>
                        <Input
                            type="number"
                            value={event.maxAttemptsPerTest}
                            onChange={(e: OnChange): void =>
                                setField({
                                    maxAttemptsPerTest: Number(e.target.value),
                                })
                            }
                        />
                    </Field>
                    <Field>
                        <Label>Start Time</Label>
                        <Input
                            type="datetime-local"
                            value={event.startTime
                                .toISOString()
                                .substring(0, 16)}
                            onChange={(e: OnChange): void =>
                                setField({
                                    startTime: newValidDateOrThrow(
                                        e.target.value
                                    ),
                                })
                            }
                        />
                    </Field>
                </Modal.Card.Body>
                <Modal.Card.Foot>
                    <Button color="primary" onClick={save}>
                        Save changes
                    </Button>
                    <Button color="secondary" onClick={cancel}>
                        Close
                    </Button>
                </Modal.Card.Foot>
            </Modal.Card>
        </Modal>
    );
};

export default ModalX;
