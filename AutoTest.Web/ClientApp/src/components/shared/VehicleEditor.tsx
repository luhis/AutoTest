import { h, FunctionComponent, Fragment } from "preact";
import { Form } from "react-bulma-components";
const { Input, Field, Label, Control } = Form;

import { Vehicle } from "../../types/shared";
import { OnChange } from "../../types/inputs";
import { MakeAndModel } from "src/types/models";
import DropdownInput from "./DropdownInput";
import { startsWithIgnoreCase } from "../../lib/string";

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
        .filter(
            (c) => startsWithIgnoreCase(c, vehicle.make) && c !== vehicle.make
        );
    const models = makeAndModels
        .filter(({ make }) => make === vehicle.make)
        .map(({ model }) => model)
        .filter(
            (c) => startsWithIgnoreCase(c, vehicle.model) && c !== vehicle.model
        );
    return (
        <Fragment>
            <Field kind="group">
                <Control fullwidth={true}>
                    <Label>Make</Label>
                    <DropdownInput
                        value={vehicle.make}
                        options={makes}
                        setValue={(e) =>
                            setField({
                                ...vehicle,
                                make: e,
                            })
                        }
                    />
                </Control>
                <Control fullwidth={true}>
                    <Label>Model</Label>
                    <DropdownInput
                        value={vehicle.model}
                        options={models}
                        setValue={(e) =>
                            setField({
                                ...vehicle,
                                model: e,
                            })
                        }
                    />
                </Control>
            </Field>
            <Field kind="group">
                <Control fullwidth={true}>
                    <Label>Registration</Label>
                    <Input
                        class="is-warning"
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
