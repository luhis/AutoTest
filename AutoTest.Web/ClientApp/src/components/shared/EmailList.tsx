import { h, FunctionComponent } from "preact";
import { Menu, Button, Input, Control, Delete, Field } from "rbx";
import { useState } from "preact/hooks";

import { OnChange } from "../../types/inputs";

interface Props {
    emails: readonly string[];
    addNew: (_: string) => void;
    remove: (_: number) => void;
}

const EmailList: FunctionComponent<Props> = ({ emails, addNew, remove }) => {
    const [newEmail, setNewEmail] = useState("");
    return (
        <Menu.List>
            {emails.map((a, i) => (
                <Menu.List.Item key={a}>
                    {a}
                    <Delete onClick={() => remove(i)} />
                </Menu.List.Item>
            ))}
            <Menu.List.Item>
                <Field kind="addons">
                    <Control>
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
                                addNew(newEmail);
                                setNewEmail("");
                            }}
                        >
                            Add
                        </Button>
                    </Control>
                </Field>
            </Menu.List.Item>
        </Menu.List>
    );
};

export default EmailList;
