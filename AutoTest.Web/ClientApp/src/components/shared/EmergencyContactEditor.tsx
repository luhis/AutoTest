import { h, FunctionComponent, Fragment } from "preact";
import { Form } from "react-bulma-components";
const { Input, Field, Label, Control } = Form;

import { EmergencyContact } from "../../types/shared";
import { OnChange } from "../../types/inputs";

interface Props {
    readonly emergencyContact: EmergencyContact;
    readonly setField: (k: EmergencyContact) => void;
}

const EmergencyContactEditor: FunctionComponent<Props> = ({
    emergencyContact,
    setField,
}) => {
    return (
        <Fragment>
            <Field kind="group">
                <Control fullwidth={true}>
                    <Label>Emergency Contact Name</Label>
                    <Input
                        required
                        value={emergencyContact.name}
                        onChange={({ target }: OnChange) => {
                            setField({
                                ...emergencyContact,
                                name: target.value,
                            });
                        }}
                    />
                </Control>
                <Control fullwidth={true}>
                    <Label>Emergency Contact Number</Label>
                    <Input
                        required
                        type="tel"
                        value={emergencyContact.phone}
                        onChange={({ target }: OnChange) =>
                            setField({
                                ...emergencyContact,
                                phone: target.value,
                            })
                        }
                    />
                </Control>
            </Field>
        </Fragment>
    );
};

export default EmergencyContactEditor;
