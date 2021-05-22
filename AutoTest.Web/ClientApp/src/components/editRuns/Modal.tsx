import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
const { Label, Field, Input } = Form;

import { TestRun } from "../../types/models";
import { addPreventDefault } from "../../lib/form";
import { OnChange } from "../../types/inputs";

interface Props {
    readonly run: TestRun;
    readonly save: () => void;
    readonly cancel: () => void;
    readonly setField: (k: Partial<TestRun>) => void;
}

const EditRunModal: FunctionComponent<Props> = ({
    save,
    run,
    cancel,
    setField,
}) => {
    const formSave = addPreventDefault(save);

    return (
        <Modal show={true} showClose={false}>
            <Modal.Card renderAs="form" onSubmit={formSave}>
                <Modal.Card.Header showClose={false}>
                    <Modal.Card.Title>Edit Test Run</Modal.Card.Title>
                </Modal.Card.Header>
                <Modal.Card.Body>
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
                                setField({
                                    timeInMS:
                                        Number.parseFloat(
                                            e.target.valueAsNumber.toFixed(2)
                                        ) * 1000,
                                })
                            }
                        />
                    </Field>
                </Modal.Card.Body>
                <Modal.Card.Footer>
                    <Button type="submit" color="primary">
                        Save changes
                    </Button>
                    <Button color="secondary" onClick={cancel}>
                        Close
                    </Button>
                </Modal.Card.Footer>
            </Modal.Card>
        </Modal>
    );
};

export default EditRunModal;
