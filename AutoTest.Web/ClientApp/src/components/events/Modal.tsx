import { h, FunctionComponent } from "preact";
import { Modal, Button, Label, Input, Field, Select } from "rbx";
import { newValidDateOrThrow } from "ts-date";
import PromiseFileReader from "promise-file-reader";

import { EditingEvent, Club } from "../../types/models";
import { OnChange, OnSelectChange } from "../../types/inputs";
import EmailList from "../shared/EmailList";
import { LoadingState } from "../../types/loadingState";
import ifSome from "../shared/ifSome";
import { getDateTimeString } from "../../lib/date";

interface Props {
    readonly event: EditingEvent;
    readonly clubs: LoadingState<readonly Club[]>;
    readonly save: () => Promise<void>;
    readonly cancel: () => void;
    readonly setField: (k: Partial<EditingEvent>) => void;
}

const ModalX: FunctionComponent<Props> = ({
    event,
    clubs,
    save,
    cancel,
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
                    {event.isClubEditable ? (
                        <Field>
                            <Label>Club</Label>
                            <Select.Container fullwidth>
                                <Select
                                    onChange={(evt: OnSelectChange) =>
                                        setField({
                                            clubId: Number.parseInt(
                                                evt.target.value
                                            ),
                                        })
                                    }
                                    value={event.clubId}
                                >
                                    <Select.Option value={-1}>
                                        - Please Select -
                                    </Select.Option>
                                    {ifSome(
                                        clubs,
                                        (a) => a.clubId,
                                        (a) => (
                                            <Select.Option value={a.clubId}>
                                                {a.clubName}
                                            </Select.Option>
                                        )
                                    )}
                                </Select>
                            </Select.Container>
                        </Field>
                    ) : null}
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
                            value={getDateTimeString(event.startTime)}
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
                        <Label>Regulations</Label>
                        <Input
                            type="file"
                            onChange={async (e: OnChange): Promise<void> => {
                                setField({
                                    regulations: e.target.files
                                        ? await PromiseFileReader.readAsDataURL(
                                              e.target.files[0]
                                          )
                                        : null,
                                });
                            }}
                            accept=".pdf"
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
