import { h, FunctionComponent, Fragment } from "preact";
import { Columns, Form, Icon } from "react-bulma-components";
const { Input, Field, Label, Control, Radio } = Form;

import { InductionTypes, Vehicle } from "../../types/shared";
import { OnChange } from "../../types/inputs";
import { MakeAndModel } from "../../types/models";
import DropdownInput from "./DropdownInput";
import { distinct } from "../../lib/array";
import FormColumn from "./FormColumn";

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
  const makes = distinct(makeAndModels.map(({ make }) => make));
  const models = distinct(
    makeAndModels
      .filter(({ make }) => make === vehicle.make)
      .map(({ model }) => model),
  );

  const setInduction = (e: OnChange) =>
    setField({
      ...vehicle,
      induction: Number.parseInt(e.target.value) as InductionTypes,
    });
  return (
    <Fragment>
      <Field kind="group">
        <Control fullwidth={true}>
          <Label>Make</Label>
          <DropdownInput
            required
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
            required
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
      <Columns>
        <FormColumn>
          <Control>
            <Label>Registration</Label>
            <Input
              required
              fullwidth
              value={vehicle.registration}
              onChange={(e: OnChange) =>
                setField({
                  ...vehicle,
                  registration: e.target.value.toLocaleUpperCase(),
                })
              }
            />
          </Control>
        </FormColumn>
        <FormColumn>
          <Control>
            <Label>Displacement (CC)</Label>
            <Control>
              <Input
                required
                fullwidth
                type="number"
                step={1}
                value={vehicle.displacement}
                onChange={(e: OnChange) =>
                  setField({
                    ...vehicle,
                    displacement: Math.floor(e.target.valueAsNumber),
                  })
                }
                min={1}
              />
              <Icon align="right" size="small">
                CC
              </Icon>
            </Control>
          </Control>
        </FormColumn>
      </Columns>
      <Field>
        <Label>Induction</Label>
        <Radio
          checked={vehicle.induction === InductionTypes.NA}
          onChange={setInduction}
          value={InductionTypes.NA}
        >
          NA
        </Radio>
        <Radio
          checked={vehicle.induction === InductionTypes.Forced}
          onChange={setInduction}
          value={InductionTypes.Forced}
        >
          Forced
        </Radio>
      </Field>
    </Fragment>
  );
};

export default VehicleEditor;
