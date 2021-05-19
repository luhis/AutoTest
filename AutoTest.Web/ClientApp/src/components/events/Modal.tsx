import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
import { newValidDateOrThrow } from "ts-date";
import PromiseFileReader from "promise-file-reader";
const { Label, Input, Field, Select } = Form;

import { EditingEvent, Club, EventType } from "../../types/models";
import { OnChange, OnSelectChange } from "../../types/inputs";
import { LoadingState } from "../../types/loadingState";
import ifSome from "../shared/ifSome";
import { getDateTimeString } from "../../lib/date";
import { startCase } from "../../lib/string";
import { addPreventDefault } from "../../lib/form";

interface Props {
    readonly event: EditingEvent;
    readonly clubs: LoadingState<readonly Club[]>;
    readonly save: () => void;
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
    const eventTypes = Object.keys(EventType)
        .map((a) => Number.parseInt(a))
        .filter((key) => !isNaN(key));

    const formSave = addPreventDefault(save);
    return (
        <form onSubmit={formSave}>
            <Modal show={true} showClose={false}>
                <Modal.Card>
                    <Modal.Card.Header showClose={false}>
                        {event.isNew ? "Add" : "Edit"} Event
                    </Modal.Card.Header>
                    <Modal.Card.Body>
                        <Field>
                            <Label>Location</Label>
                            <Input
                                required
                                value={event.location}
                                onChange={(e: OnChange): void =>
                                    setField({ location: e.target.value })
                                }
                            />
                        </Field>
                        <Field>
                            <Label>Event Type</Label>
                            <Select<EventType>
                                required
                                class="is-fullwidth"
                                onChange={(evt: OnSelectChange) =>
                                    setField({
                                        eventType: Number.parseInt(
                                            evt.target.value
                                        ),
                                    })
                                }
                                value={event.eventType}
                            >
                                <option disabled value={Number.NaN}>
                                    - Please Select -
                                </option>
                                {eventTypes.map((key) => (
                                    <option key={key} value={key}>
                                        {startCase(EventType[key])}
                                    </option>
                                ))}
                            </Select>
                        </Field>
                        {event.isClubEditable ? (
                            <Field>
                                <Label>Club</Label>
                                <Select
                                    required
                                    class="is-fullwidth"
                                    onChange={(evt: OnSelectChange) =>
                                        setField({
                                            clubId: Number.parseInt(
                                                evt.target.value
                                            ),
                                        })
                                    }
                                    value={event.clubId}
                                >
                                    <option disabled value={undefined}>
                                        - Please Select -
                                    </option>
                                    {ifSome(
                                        clubs,
                                        (a) => a.clubId,
                                        (a) => (
                                            <option value={a.clubId}>
                                                {a.clubName}
                                            </option>
                                        )
                                    )}
                                </Select>
                            </Field>
                        ) : null}
                        <Field>
                            <Label>Test Count</Label>
                            <Input
                                required
                                type="number"
                                min={1}
                                step={1}
                                value={event.testCount}
                                onChange={(e: OnChange): void =>
                                    setField({
                                        testCount: Math.floor(
                                            e.target.valueAsNumber
                                        ),
                                    })
                                }
                            />
                        </Field>
                        <Field>
                            <Label>Attempts</Label>
                            <Input
                                required
                                type="number"
                                min={1}
                                step={1}
                                value={event.maxAttemptsPerTest}
                                onChange={(e: OnChange): void =>
                                    setField({
                                        maxAttemptsPerTest: Math.floor(
                                            e.target.valueAsNumber
                                        ),
                                    })
                                }
                            />
                        </Field>
                        <Field>
                            <Label>Start Time</Label>
                            <Input
                                required
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
                                required={event.isNew}
                                type="file"
                                onChange={async (
                                    e: OnChange
                                ): Promise<void> => {
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
                            <Label>Maps</Label>
                            <Input type="file" accept=".pdf" />
                        </Field>
                    </Modal.Card.Body>
                    <Modal.Card.Footer>
                        <Button color="primary" type="submit">
                            Save changes
                        </Button>
                        <Button color="secondary" onClick={cancel}>
                            Close
                        </Button>
                    </Modal.Card.Footer>
                </Modal.Card>
            </Modal>
        </form>
    );
};

export default ModalX;
