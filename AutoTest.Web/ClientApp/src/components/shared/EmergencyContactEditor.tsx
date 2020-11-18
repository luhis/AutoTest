import { h, FunctionComponent, Fragment } from "preact";
import { Input, Field, Label } from "rbx";

import { EmergencyContact } from "../../types/shared";
import { OnChange } from "../../types/inputs";

interface Props {
    emergencyContact: EmergencyContact;
    setField: (k: EmergencyContact) => void;
}

const EmergencyContactEditor: FunctionComponent<Props> = ({
    emergencyContact,
    setField,
}) => {
    return (
        <Fragment>
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
        </Fragment>
    );
};

export default EmergencyContactEditor;
