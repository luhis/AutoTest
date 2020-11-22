import { h, FunctionComponent, Fragment } from "preact";
import { Button, Input, Control, Delete, Field, Level } from "rbx";
import { useState } from "preact/hooks";

import { OnChange } from "../../types/inputs";
import { AuthorisationEmail } from "../../types/models";

interface Props {
    readonly emails: readonly AuthorisationEmail[];
    readonly addNew: (_: AuthorisationEmail) => void;
    readonly remove: (_: number) => void;
}

const EmailList: FunctionComponent<Props> = ({ emails, addNew, remove }) => {
    const [newEmail, setNewEmail] = useState("");
    return (
        <Fragment>
            {emails.map(({ email }, i) => (
                <Level key={email}>
                    <Level.Item align="left">{email}</Level.Item>
                    <Level.Item align="right">
                        <Delete onClick={() => remove(i)} />
                    </Level.Item>
                </Level>
            ))}
            <Field kind="addons">
                <Control expanded>
                    <Input
                        type="email"
                        value={newEmail}
                        onChange={(e: OnChange): void =>
                            setNewEmail(e.target.value)
                        }
                    />
                </Control>
                <Control>
                    <Button
                        onClick={() => {
                            if (newEmail !== "") {
                                addNew({ email: newEmail });
                                setNewEmail("");
                            }
                        }}
                    >
                        Add
                    </Button>
                </Control>
            </Field>
        </Fragment>
    );
};

export default EmailList;
