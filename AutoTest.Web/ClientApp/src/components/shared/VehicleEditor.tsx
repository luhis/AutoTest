import { h, FunctionComponent, Fragment } from "preact";
import { Form } from "react-bulma-components";
const { Input, Field, Label } = Form;

import { Vehicle } from "../../types/shared";
import { OnChange } from "../../types/inputs";

interface Props {
    readonly vehicle: Vehicle;
    readonly setField: (k: Vehicle) => void;
}

const VehicleEditor: FunctionComponent<Props> = ({ vehicle, setField }) => {
    return (
        <Fragment>
            <Field horizontal>
                <Field>
                    <Label>Make</Label>
                    <Input
                        value={vehicle.make}
                        onChange={(e: OnChange): void =>
                            setField({
                                ...vehicle,
                                make: e.target.value,
                            })
                        }
                    />
                </Field>
                <Field>
                    <Label>Model</Label>
                    <Input
                        value={vehicle.model}
                        onChange={(e: OnChange): void =>
                            setField({
                                ...vehicle,
                                model: e.target.value,
                            })
                        }
                    />
                </Field>
            </Field>
            <Field horizontal>
                <Field>
                    <Label>Registration</Label>
                    <Input
                        value={vehicle.registration}
                        onChange={(e: OnChange): void =>
                            setField({
                                ...vehicle,
                                registration: e.target.value.toLocaleUpperCase(),
                            })
                        }
                    />
                </Field>
                <Field>
                    <Label>Displacement (CC)</Label>
                    <Input
                        type="number"
                        value={vehicle.displacement}
                        onChange={(e: OnChange): void =>
                            setField({
                                ...vehicle,
                                displacement: Number.parseInt(e.target.value),
                            })
                        }
                        min={0}
                    />
                </Field>
            </Field>
        </Fragment>
    );
};

export default VehicleEditor;
