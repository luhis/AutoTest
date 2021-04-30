import { h, FunctionComponent, Fragment } from "preact";
import { Form } from "react-bulma-components";
const { Input, Field, Label } = Form;

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
            <Field horizontal>
                <Field>
                    <Label>Emergency Contact Name</Label>
                    <Input
                        value={emergencyContact.name}
                        onChange={({ target }: OnChange): void => {
                            setField({
                                ...emergencyContact,
                                name: target.value,
                            });
                        }}
                    />
                </Field>
                <Field>
                    <Label>Emergency Contact Number</Label>
                    <Input
                        type="tel"
                        value={emergencyContact.phone}
                        onChange={({ target }: OnChange): void =>
                            setField({
                                ...emergencyContact,
                                phone: target.value,
                            })
                        }
                    />
                </Field>
            </Field>
        </Fragment>
    );
};

export default EmergencyContactEditor;
