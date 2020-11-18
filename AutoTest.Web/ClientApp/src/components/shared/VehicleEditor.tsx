import { h, FunctionComponent, Fragment } from "preact";
import { Input, Field, Label } from "rbx";

import { Vehicle } from "../../types/shared";
import { OnChange } from "../../types/inputs";

interface Props {
    vehicle: Vehicle;
    setField: (k: Vehicle) => void;
}

const VehicleEditor: FunctionComponent<Props> = ({ vehicle, setField }) => {
    return (
        <Fragment>
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
        </Fragment>
    );
};

export default VehicleEditor;
