import { h, FunctionComponent, Fragment } from "preact";
import { Button, Input, Control, Delete, Field, Level } from "rbx";
import { useCallback, useState } from "preact/hooks";

import { OnChange } from "../../types/inputs";
import { AuthorisationEmail } from "../../types/models";

interface Props {
    readonly emails: readonly AuthorisationEmail[];
    readonly addNew: (_: AuthorisationEmail) => void;
    readonly remove: (_: number) => void;
}

const ListItem: FunctionComponent<{
    readonly email: string;
    readonly i: number;
    readonly remove: (_: number) => void;
}> = ({ email, i, remove }) => {
    const removeItem = useCallback(() => remove(i), [i, remove]);
    return (
        <Level>
            <Level.Item align="left">{email}</Level.Item>
            <Level.Item align="right">
                <Delete onClick={removeItem} />
            </Level.Item>
        </Level>
    );
};

const EmailList: FunctionComponent<Props> = ({ emails, addNew, remove }) => {
    const [newEmail, setNewEmail] = useState("");
    const addNewEmail = useCallback(() => {
        if (newEmail !== "") {
            addNew({ email: newEmail });
            setNewEmail("");
        }
    }, [addNew, newEmail]);
    const setEmail = useCallback(
        (e: OnChange): void => setNewEmail(e.target.value),
        []
    );
    return (
        <Fragment>
            {emails.map(({ email }, i) => (
                <ListItem email={email} key={email} i={i} remove={remove} />
            ))}
            <Field kind="addons">
                <Control expanded>
                    <Input type="email" value={newEmail} onChange={setEmail} />
                </Control>
                <Control>
                    <Button onClick={addNewEmail}>Add</Button>
                </Control>
            </Field>
        </Fragment>
    );
};

export default EmailList;
