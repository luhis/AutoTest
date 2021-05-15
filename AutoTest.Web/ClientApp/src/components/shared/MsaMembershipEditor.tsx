import { h, FunctionComponent, Fragment } from "preact";
import { Form } from "react-bulma-components";
const { Input, Field, Label, Control } = Form;

import { MsaMembership } from "src/types/shared";
import { OnChange } from "../../types/inputs";

interface Props {
    readonly membership: MsaMembership;
    readonly setField: (k: MsaMembership) => void;
}

const MsaMembershipEditor: FunctionComponent<Props> = ({
    membership,
    setField,
}) => {
    return (
        <Fragment>
            <Field kind="group">
                <Control fullwidth={true}>
                    <Label>MSA License</Label>
                    <Input
                        required
                        type="number"
                        min={0}
                        value={membership.msaLicense}
                        onChange={(e: OnChange): void =>
                            setField({
                                ...membership,
                                msaLicense: Number.parseInt(e.target.value),
                            })
                        }
                    />
                </Control>
                <Control fullwidth={true}>
                    <Label>MSA License Type</Label>
                    <Input
                        required
                        value={membership.msaLicenseType}
                        onChange={(e: OnChange): void =>
                            setField({
                                ...membership,
                                msaLicenseType: e.target.value,
                            })
                        }
                    />
                </Control>
            </Field>
        </Fragment>
    );
};

export default MsaMembershipEditor;
