import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
const { Input, Field, Label } = Form;

import { Club, EditingClub } from "../../types/models";
import { OnChange } from "../../types/inputs";
import EmailList from "../shared/EmailList";
import { addPreventDefault } from "../../lib/form";

interface Props {
    readonly club: EditingClub;
    readonly save: () => void;
    readonly cancel: () => void;
    readonly setField: (k: Partial<Club>) => void;
}

const ModalX: FunctionComponent<Props> = ({ save, cancel, club, setField }) => {
    const formSave = addPreventDefault(save);

    return (
        <Modal show={true} showClose={false}>
            <Modal.Card>
                <Modal.Card.Header showClose={false}>
                    {club.isNew ? "Add" : "Edit"} Club
                </Modal.Card.Header>
                <form onSubmit={formSave}>
                    <Modal.Card.Body>
                        <Field>
                            <Label>Name</Label>
                            <Input
                                required
                                value={club.clubName}
                                onChange={(e: OnChange): void =>
                                    setField({ clubName: e.target.value })
                                }
                            />
                        </Field>
                        <Field>
                            <Label>Payment Address</Label>
                            <Input
                                value={club.clubPaymentAddress}
                                onChange={(e: OnChange): void =>
                                    setField({
                                        clubPaymentAddress: e.target.value,
                                    })
                                }
                            />
                        </Field>
                        <Field>
                            <Label>Website</Label>
                            <Input
                                value={club.website}
                                type="url"
                                onChange={(e: OnChange): void =>
                                    setField({
                                        website: e.target.value,
                                    })
                                }
                            />
                        </Field>
                        <Field>
                            <Label>Admin Emails</Label>
                            <EmailList
                                emails={club.adminEmails}
                                addNew={(s) =>
                                    setField({
                                        adminEmails: club.adminEmails.concat(s),
                                    })
                                }
                                remove={(removeIndex) =>
                                    setField({
                                        adminEmails: club.adminEmails.filter(
                                            (_, i) => i !== removeIndex
                                        ),
                                    })
                                }
                            />
                        </Field>
                    </Modal.Card.Body>
                    <Modal.Card.Footer>
                        <Button color="primary" type="submit">
                            Save changes
                        </Button>
                        <Button color="secondary" onClick={cancel}>
                            Close
                        </Button>
                    </Modal.Card.Footer>
                </form>
            </Modal.Card>
        </Modal>
    );
};

export default ModalX;
