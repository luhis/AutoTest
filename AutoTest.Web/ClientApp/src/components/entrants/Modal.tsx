import { h, FunctionComponent } from "preact";
import { Modal, Button, Label, Input, Field } from "rbx";
import { DeepPartial } from "tsdef";

import { Entrant, EditingEntrant } from "../../types/models";
import { OnChange } from "../../types/inputs";
import { useSelector } from "react-redux";
import { selectClassOptions } from "../../store/event/selectors";

interface Props {
    entrant: EditingEntrant;
    save: () => Promise<void>;
    cancel: () => void;
    setField: (k: DeepPartial<Omit<Entrant, "driverNumber">>) => void;
}

const EntrantsModal: FunctionComponent<Readonly<Props>> = ({
    save,
    cancel,
    entrant,
    setField,
}) => {
    const classesInUse = useSelector(selectClassOptions).filter(
        (c) => c.startsWith(entrant.class) && c !== entrant.class
    );
    return (
        <Modal active={true}>
            <Modal.Background />
            <Modal.Card>
                <Modal.Card.Head>
                    {entrant.isNew ? "Add" : "Edit"} Entrant
                </Modal.Card.Head>
                <Modal.Card.Body>
                    <Field>
                        <Label>Given Name</Label>
                        <Input
                            value={entrant.givenName}
                            onChange={(e: OnChange): void =>
                                setField({ givenName: e.target.value })
                            }
                        />
                    </Field>
                    <Field>
                        <Label>Family Name</Label>
                        <Input
                            value={entrant.familyName}
                            onChange={(e: OnChange): void =>
                                setField({ familyName: e.target.value })
                            }
                        />
                    </Field>
                    <Field>
                        <Label>Class</Label>
                        <Input
                            list="classes"
                            value={entrant.class}
                            onChange={(e: OnChange): void => {
                                setField({
                                    class: e.target.value.toLocaleUpperCase(),
                                });
                            }}
                        >
                            <datalist id="classes">
                                {classesInUse.map((a) => (
                                    <option
                                        key={a}
                                        value={a}
                                        onClick={() => {
                                            setField({
                                                class: a,
                                            });
                                        }}
                                    />
                                ))}
                            </datalist>
                        </Input>
                    </Field>
                    <Field>
                        <Label>Registration</Label>
                        <Input
                            value={entrant.vehicle.registration}
                            onChange={(e: OnChange): void =>
                                setField({
                                    vehicle: {
                                        registration: e.target.value.toLocaleUpperCase(),
                                    },
                                })
                            }
                        />
                    </Field>
                    <Field>
                        <Label>Displacement</Label>
                        <Input
                            type="number"
                            value={entrant.vehicle.displacement}
                            onChange={(e: OnChange): void =>
                                setField({
                                    vehicle: {
                                        displacement: Number.parseInt(
                                            e.target.value
                                        ),
                                    },
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

export default EntrantsModal;
