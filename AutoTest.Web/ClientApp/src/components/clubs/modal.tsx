import { h, FunctionComponent } from "preact";
import { Modal, Button, Label, Input, Field } from "rbx";

import { EditableClub } from "../../types/models";
import { OnChange } from "../../types/inputs";

interface Props {
    club: EditableClub;
    save: () => Promise<void>;
    cancel: () => void;
    setField: (k: Partial<EditableClub>) => void;
}

const ModalX: FunctionComponent<Props> = ({ save, cancel, club, setField }) => {
    return (
        <Modal active={true}>
            <Modal.Background />
            <Modal.Card>
                <Modal.Card.Head>
                    {club.clubId === undefined ? "Add" : "Edit"} Club
                </Modal.Card.Head>
                <Modal.Card.Body>
                    <Field>
                        <Label>Name</Label>
                        <Input
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
                                setField({ clubPaymentAddress: e.target.value })
                            }
                        />
                    </Field>
                    <Field>
                        <Label>Website</Label>
                        <Input
                            value={club.website}
                            onChange={(e: OnChange): void =>
                                setField({
                                    website: e.target.value,
                                })
                            }
                        />
                    </Field>
                </Modal.Card.Body>
                <Modal.Card.Foot>
                    <Button color="primary" onClick={save}>
                        Save changes
                    </Button>
                    <Button color="secondary" onClick={cancel}>
                        Close
                    </Button>
                </Modal.Card.Foot>
            </Modal.Card>
        </Modal>
    );
};

export default ModalX;
