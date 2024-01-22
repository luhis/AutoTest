import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
const { Label, Field, Input, Select } = Form;
import { StateUpdater, useState } from "preact/hooks";

import {
  PublicEntrant,
  PenaltyType,
  TestRunFromServer,
  Penalty,
} from "../../types/models";
import { addPreventDefault } from "../../lib/form";
import { OnChange } from "../../types/inputs";
import Penalties from "../marshal/Penalties";

interface Props {
  readonly run: TestRunFromServer;
  readonly entrants: readonly PublicEntrant[];
  readonly save: () => Promise<void>;
  readonly cancel: () => void;
  readonly setField: StateUpdater<TestRunFromServer>;
}

const findByPenaltyType = (
  penalties: readonly Penalty[],
  penaltyType: PenaltyType,
) => penalties.find((penalty) => penalty.penaltyType === penaltyType);

const increase = (
  setField: StateUpdater<TestRunFromServer>,
  penaltyType: PenaltyType,
) => {
  setField((a) => {
    const found = findByPenaltyType(a.penalties, penaltyType);
    return {
      ...a,
      penalties: a.penalties
        .filter((penalty) => penalty.penaltyType !== penaltyType)
        .concat(
          found !== undefined
            ? {
                ...found,
                instanceCount: found.instanceCount + 1,
              }
            : {
                penaltyType: penaltyType,
                instanceCount: 1,
              },
        ),
    };
  });
};
const decrease = (
  setField: StateUpdater<TestRunFromServer>,
  penaltyType: PenaltyType,
) => {
  setField((a) => {
    const found = findByPenaltyType(a.penalties, penaltyType);
    return {
      ...a,
      penalties: a.penalties
        .filter((penalty) => penalty.penaltyType !== penaltyType)
        .concat(
          found !== undefined && found.instanceCount > 2
            ? {
                ...found,
                instanceCount: found.instanceCount - 1,
              }
            : [],
        ),
    };
  });
};

const EditRunModal: FunctionComponent<Props> = ({
  save,
  run,
  entrants,
  cancel,
  setField,
}) => {
  const [saving, setSaving] = useState(false);
  const formSave = addPreventDefault(save, setSaving);

  return (
    <Modal show={true} showClose={false}>
      <Modal.Card renderAs="form" onSubmit={formSave}>
        <Modal.Card.Header showClose={false}>
          <Modal.Card.Title>Edit Test Run</Modal.Card.Title>
        </Modal.Card.Header>
        <Modal.Card.Body>
          <Field>
            <Label>Entrant</Label>
            <Select
              required
              value={run.entrantId}
              onChange={(e: OnChange) =>
                setField((a) => ({
                  ...a,
                  entrantId: Number.parseInt(e.target.value),
                }))
              }
            >
              {entrants.map((a) => (
                <option value={a.entrantId} key={a.entrantId}>
                  {a.driverNumber} {a.givenName} {a.familyName}
                </option>
              ))}
            </Select>
          </Field>
          <Field>
            <Label>Time (Secs)</Label>
            <Input
              required
              type="number"
              min="0"
              step="0.01"
              value={
                run.timeInMS === undefined
                  ? ""
                  : (run.timeInMS / 1000).toFixed(2)
              }
              onChange={(e: OnChange) =>
                setField((a) => ({
                  ...a,
                  timeInMS:
                    Number.parseFloat(e.target.valueAsNumber.toFixed(2)) * 1000,
                }))
              }
            />
          </Field>
          <Penalties
            penalties={run.penalties}
            increase={(a) => increase(setField, a)}
            decrease={(a) => decrease(setField, a)}
          />
        </Modal.Card.Body>
        <Modal.Card.Footer>
          <Button loading={saving} type="submit" color="primary">
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

export default EditRunModal;
