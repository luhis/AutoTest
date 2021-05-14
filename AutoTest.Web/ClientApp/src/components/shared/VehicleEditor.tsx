import { h, FunctionComponent, Fragment } from "preact";
import { Form } from "react-bulma-components";
const { Input, Field, Label, Control } = Form;

import { Vehicle } from "../../types/shared";
import { OnChange } from "../../types/inputs";
import { MakeAndModel } from "src/types/models";

interface Props {
    readonly vehicle: Vehicle;
    readonly makeAndModels: readonly MakeAndModel[];
    readonly setField: (k: Vehicle) => void;
}

const VehicleEditor: FunctionComponent<Props> = ({
    vehicle,
    makeAndModels,
    setField,
}) => {
    const makes = makeAndModels
        .map(({ make }) => make)
        .filter((c) => c.startsWith(vehicle.make) && c !== vehicle.make);
    const models = makeAndModels
        .filter(({ make }) => make === vehicle.make)
        .map(({ model }) => model)
        .filter((c) => c.startsWith(vehicle.model) && c !== vehicle.model);
    return (
        <Fragment>
            <Field kind="group">
                <Control fullwidth={true}>
                    <Label>Make</Label>
                    <Input
                        list="makes"
                        value={vehicle.make}
                        onChange={(e: OnChange): void =>
                            setField({
                                ...vehicle,
                                make: e.target.value,
                            })
                        }
                    >
                        <datalist id="makes">
                            {makes.map((a) => (
                                <option
                                    key={a}
                                    value={a}
                                    onClick={() => {
                                        setField({
                                            ...vehicle,
                                            make: a,
                                        });
                                    }}
                                />
                            ))}
                        </datalist>
                    </Input>
                </Control>
                <Control fullwidth={true}>
                    <Label>Model</Label>
                    <Input
                        list="models"
                        value={vehicle.model}
                        onChange={(e: OnChange): void =>
                            setField({
                                ...vehicle,
                                model: e.target.value,
                            })
                        }
                    >
                        <datalist id="models">
                            {models.map((a) => (
                                <option
                                    key={a}
                                    value={a}
                                    onClick={() => {
                                        setField({
                                            ...vehicle,
                                            model: a,
                                        });
                                    }}
                                />
                            ))}
                        </datalist>
                    </Input>
                </Control>
            </Field>
            <Field kind="group">
                <Control fullwidth={true}>
                    <Label>Registration</Label>
                    <Input
                        value={vehicle.registration}
                        onChange={(e: OnChange): void =>
                            setField({
                                ...vehicle,
                                registration:
                                    e.target.value.toLocaleUpperCase(),
                            })
                        }
                    />
                </Control>
                <Control fullwidth={true}>
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
                </Control>
            </Field>
        </Fragment>
    );
};

export default VehicleEditor;
