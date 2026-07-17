import { h, FunctionComponent, Fragment } from "preact";
import { Form } from "react-bulma-components";
const { Input, Field, Label, Control, Help } = Form;

import { MsaMembership } from "../../types/shared";
import { OnChange } from "../../types/inputs";
import DropdownInput from "./DropdownInput";
import { ClubmanLicenseLocation } from "../../settings";

interface Props {
  readonly membership: MsaMembership;
  readonly licenseTypes: readonly string[];
  readonly setField: (k: MsaMembership) => void;
}

const MsaMembershipEditor: FunctionComponent<Props> = ({
  membership,
  licenseTypes,
  setField,
}) => {
  return (
    <Fragment>
      <Field kind="group">
        <Control fullwidth={true}>
          <Label>MSA License Type</Label>
          <DropdownInput
            required
            value={membership.msaLicenseType}
            options={licenseTypes}
            setValue={(e) =>
              setField({
                ...membership,
                msaLicenseType: e,
              })
            }
          />
          <Help>
            <a target="_blank" href={ClubmanLicenseLocation} rel="noreferrer">
              Get a clubman license
            </a>
          </Help>
        </Control>
        <Control fullwidth={true}>
          <Label>MSA License</Label>
          <Input
            required
            type="number"
            min={0}
            step={1}
            value={membership.msaLicense}
            onChange={(e: OnChange) =>
              setField({
                ...membership,
                msaLicense: Math.floor(e.target.valueAsNumber),
              })
            }
          />
        </Control>
      </Field>
    </Fragment>
  );
};

export default MsaMembershipEditor;
