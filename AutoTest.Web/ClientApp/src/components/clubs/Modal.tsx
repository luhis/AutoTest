import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";

import { Club, EditingClub } from "../../types/models";
import { OnChange } from "../../types/inputs";
import EmailList from "../shared/EmailList";

interface Props {
    readonly club: EditingClub;
    readonly save: () => void;
    readonly cancel: () => void;
    readonly setField: (k: Partial<Club>) => void;
}

const ModalX: FunctionComponent<Props> = ({ save, cancel, club, setField }) => {
    return (
        <Modal show={true} showClose={false}>
            <Modal.Card>
                <Modal.Card.Header showClose={false}>
                    {club.isNew ? "Add" : "Edit"} Club
                </Modal.Card.Header>
                <Modal.Card.Body>
                    <Form.Field>
                        <Form.Label>Name</Form.Label>
                        <Form.Input
                            value={club.clubName}
                            onChange={(e: OnChange): void =>
                                setField({ clubName: e.target.value })
                            }
                        />
                    </Form.Field>
                    <Form.Field>
                        <Form.Label>Payment Address</Form.Label>
                        <Form.Input
                            value={club.clubPaymentAddress}
                            onChange={(e: OnChange): void =>
                                setField({ clubPaymentAddress: e.target.value })
                            }
                        />
                    </Form.Field>
                    <Form.Field>
                        <Form.Label>Website</Form.Label>
                        <Form.Input
                            value={club.website}
                            onChange={(e: OnChange): void =>
                                setField({
                                    website: e.target.value,
                                })
                            }
                        />
                    </Form.Field>
                    <Form.Field>
                        <Form.Label>Admin Emails</Form.Label>
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
                    </Form.Field>
                </Modal.Card.Body>
                <Modal.Card.Footer>
                    <Button color="primary" onClick={save}>
                        Save changes
                    </Button>
                    <Button color="secondary" onClick={cancel}>
                        Close
                    </Button>
                </Modal.Card.Footer>
            </Modal.Card>
        </Modal>
    );
};

export default ModalX;
