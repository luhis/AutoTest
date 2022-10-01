import { h, FunctionComponent } from "preact";
import { Button, Level, Form } from "react-bulma-components";
import { useCallback, useState } from "preact/hooks";
const { Input, Control, Field } = Form;
import { FaPlus } from "react-icons/fa";

import { OnChange } from "../../types/inputs";
import { AuthorisationEmail } from "../../types/models";
import { addPreventDefault } from "../../lib/form";

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
                <Button remove onClick={removeItem} />
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
        return Promise.resolve();
    }, [addNew, newEmail]);
    const setEmail = useCallback(
        (e: OnChange) => setNewEmail(e.target.value),
        []
    );
    const [saving, setSaving] = useState(false);
    const formSave = addPreventDefault(addNewEmail, setSaving);
    return (
        <form onSubmit={formSave}>
            {emails.map(({ email }, i) => (
                <ListItem email={email} key={email} i={i} remove={remove} />
            ))}
            <Field kind="addons">
                <Control fullwidth={true}>
                    <Input
                        placeholder="joe@bloggs.com"
                        required
                        type="email"
                        value={newEmail}
                        onChange={setEmail}
                    />
                </Control>
                <Control>
                    <Button
                        loading={saving}
                        disabled={newEmail === ""}
                        type="submit"
                    >
                        <FaPlus title="Add" />
                        &nbsp; Add
                    </Button>
                </Control>
            </Field>
        </form>
    );
};

export default EmailList;
