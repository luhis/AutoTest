import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
import PromiseFileReader from "promise-file-reader";
const { Label, Input, Field, Select, Help, Checkbox, Control } = Form;
import { isNil } from "@s-libs/micro-dash";
import { useState } from "preact/hooks";
import prettyBytes from "pretty-bytes";

import {
  EditingEvent,
  Club,
  EventType,
  TimingSystem,
} from "../../types/models";
import { OnChange, OnSelectChange } from "../../types/inputs";
import { LoadingState } from "../../types/loadingState";
import ifSome from "../shared/ifSome";
import { startCase } from "../../lib/string";
import { addPreventDefault, toggleValue } from "../../lib/form";

interface Props {
  readonly event: EditingEvent;
  readonly clubs: LoadingState<readonly Club[]>;
  readonly save: () => Promise<void>;
  readonly cancel: () => void;
  readonly setField: (k: Partial<EditingEvent>) => void;
}

const getFileLength = (data: string | null) =>
  data ? atob(data.split("base64,")[1]).length : 0;
const eventTypes = Object.keys(EventType)
  .map((a) => Number.parseInt(a))
  .filter((key) => !isNaN(key));
const timingSystems = Object.keys(TimingSystem)
  .map((a) => Number.parseInt(a))
  .filter((key) => !isNaN(key));

const ModalX: FunctionComponent<Props> = ({
  event,
  clubs,
  save,
  cancel,
  setField,
}) => {
  const [saving, setSaving] = useState(false);
  const formSave = addPreventDefault(save, setSaving);
  return (
    <Modal show={true} showClose={false}>
      <Modal.Card renderAs="form" onSubmit={formSave}>
        <Modal.Card.Header showClose={false}>
          <Modal.Card.Title>
            {event.isNew ? "Add" : "Edit"} Event
          </Modal.Card.Title>
        </Modal.Card.Header>
        <Modal.Card.Body>
          <Field>
            <Label>Location</Label>
            <Input
              required
              value={event.location}
              onChange={(e: OnChange) => setField({ location: e.target.value })}
            />
          </Field>
          <Field>
            <Label>Event Type</Label>
            {eventTypes.map((key) => (
              <Control key={key}>
                <Checkbox
                  checked={event.eventTypes.includes(key)}
                  onChange={() => {
                    return setField({
                      eventTypes: toggleValue(event.eventTypes, key),
                    });
                  }}
                >
                  {startCase(EventType[key])}
                </Checkbox>
              </Control>
            ))}
          </Field>
          {event.isClubEditable ? (
            <Field>
              <Label>Club</Label>
              <Select
                required
                fullwidth
                onChange={(evt: OnSelectChange) =>
                  setField({
                    clubId: Number.parseInt(evt.target.value),
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
                    <option value={a.clubId}>{a.clubName}</option>
                  ),
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
              value={event.courseCount}
              onChange={(e: OnChange) =>
                setField({
                  courseCount: Math.floor(e.target.valueAsNumber),
                })
              }
            />
          </Field>
          <Field>
            <Label>Timing System</Label>
            <Select
              required
              fullwidth
              onChange={(evt: OnSelectChange) =>
                setField({
                  timingSystem: Number.parseInt(evt.target.value),
                })
              }
              value={event.timingSystem}
            >
              <option disabled value={undefined}>
                - Please Select -
              </option>
              {timingSystems.map((a) => (
                <option value={a} key={a}>
                  {startCase(TimingSystem[a])}
                </option>
              ))}
            </Select>
          </Field>
          <Field>
            <Label>Attempts</Label>
            <Input
              required
              type="number"
              min={1}
              step={1}
              value={event.maxAttemptsPerCourse}
              onChange={(e: OnChange) =>
                setField({
                  maxAttemptsPerCourse: Math.floor(e.target.valueAsNumber),
                })
              }
            />
          </Field>
          <Field>
            <Label>Max Entrants</Label>
            <Input
              required
              type="number"
              min={1}
              step={1}
              value={event.maxEntrants}
              onChange={(e: OnChange) =>
                setField({
                  maxEntrants: Math.floor(e.target.valueAsNumber),
                })
              }
            />
          </Field>
          <Field>
            <Label>Start Time</Label>
            <Input
              required
              type="datetime-local"
              value={event.startTime}
              onChange={(e: OnChange) =>
                setField({
                  startTime: e.target.value,
                })
              }
            />
          </Field>
          <Field>
            <Label>Entry Open Date</Label>
            <Input
              required
              type="datetime-local"
              value={event.entryOpenDate}
              onChange={(e: OnChange) =>
                setField({
                  entryOpenDate: e.target.value,
                })
              }
            />
          </Field>
          <Field>
            <Label>Entry Close Date</Label>
            <Input
              required
              type="datetime-local"
              value={event.entryCloseDate}
              onChange={(e: OnChange) =>
                setField({
                  entryCloseDate: e.target.value,
                })
              }
            />
          </Field>
          <Field>
            <Label>Regulations</Label>
            <Input
              required={event.isNew}
              type="file"
              onChange={async (e: OnChange): Promise<void> => {
                setField({
                  regulations: e.target.files
                    ? await PromiseFileReader.readAsDataURL(e.target.files[0])
                    : null,
                });
              }}
              accept=".pdf"
            />
            <Help>
              {!isNil(event.maps)
                ? `Present: ${prettyBytes(getFileLength(event.regulations))}`
                : "Missing"}
            </Help>
          </Field>
          <Field>
            <Label>Maps</Label>
            <Input
              required={event.isNew}
              type="file"
              onChange={async (e: OnChange): Promise<void> => {
                setField({
                  maps: e.target.files
                    ? await PromiseFileReader.readAsDataURL(e.target.files[0])
                    : null,
                });
              }}
              accept=".pdf"
            />
            <Help>
              {!isNil(event.maps)
                ? `Present: ${prettyBytes(getFileLength(event.maps))}`
                : "Missing"}
            </Help>
          </Field>
        </Modal.Card.Body>
        <Modal.Card.Footer>
          <Button loading={saving} color="primary">
            Save changes
          </Button>
          <Button type="button" color="secondary" onClick={cancel}>
            Cancel
          </Button>
        </Modal.Card.Footer>
      </Modal.Card>
    </Modal>
  );
};

export default ModalX;
