import { h, FunctionComponent } from "preact";
import { Modal, Button, Label, Input, Field } from "rbx";
import { newValidDateOrThrow } from "ts-date";

import { Event, EditingEvent } from "../../types/models";
import { OnChange } from "../../types/inputs";
import EmailList from "../shared/EmailList";

interface Props {
    event: EditingEvent;
    save: () => Promise<void>;
    cancel: () => void;
    setField: (k: Partial<Event>) => void;
}

const ModalX: FunctionComponent<Readonly<Props>> = ({
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
                    {event.isNew ? "Add" : "Edit"} Event
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
                            min="1"
                            value={event.testCount}
                            onChange={(e: OnChange): void =>
                                setField({
                                    testCount: Number.parseInt(e.target.value),
                                })
                            }
                        />
                    </Field>
                    <Field>
                        <Label>Attempts</Label>
                        <Input
                            type="number"
                            min="1"
                            value={event.maxAttemptsPerTest}
                            onChange={(e: OnChange): void =>
                                setField({
                                    maxAttemptsPerTest: Number.parseInt(
                                        e.target.value
                                    ),
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
                    <Field>
                        <Label>Marshal Emails</Label>
                        <EmailList
                            emails={event.marshalEmails}
                            addNew={(s) =>
                                setField({
                                    marshalEmails: event.marshalEmails.concat(
                                        s
                                    ),
                                })
                            }
                            remove={(removeIndex) =>
                                setField({
                                    marshalEmails: event.marshalEmails.filter(
                                        (_, i) => i !== removeIndex
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
