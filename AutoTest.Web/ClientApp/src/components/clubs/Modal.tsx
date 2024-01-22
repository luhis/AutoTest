import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
import { useState } from "preact/hooks";
const { Input, Field, Label, Help, Control } = Form;

import { Club, EditingClub } from "../../types/models";
import { OnChange } from "../../types/inputs";
import EmailList from "../shared/EmailList";
import { addPreventDefault } from "../../lib/form";

interface Props {
  readonly club: EditingClub;
  readonly save: () => Promise<void>;
  readonly cancel: () => void;
  readonly setField: (k: Partial<Club>) => void;
}

const ModalX: FunctionComponent<Props> = ({ save, cancel, club, setField }) => {
  const [saving, setSaving] = useState(false);
  const formSave = addPreventDefault(save, setSaving);

  return (
    <Modal show={true} showClose={false}>
      <Modal.Card renderAs="form" onSubmit={formSave}>
        <Modal.Card.Header showClose={false}>
          <Modal.Card.Title>
            {club.isNew ? "Add" : "Edit"} Club
          </Modal.Card.Title>
        </Modal.Card.Header>
        <Modal.Card.Body>
          <Field>
            <Label>Name</Label>
            <Control>
              <Input
                required
                value={club.clubName}
                onChange={(e: OnChange) =>
                  setField({ clubName: e.target.value })
                }
              />
            </Control>
          </Field>
          <Field>
            <Label>Payment Address</Label>
            <Control>
              <Input
                type="email"
                value={club.clubPaymentAddress}
                onChange={(e: OnChange) =>
                  setField({
                    clubPaymentAddress: e.target.value,
                  })
                }
              />
            </Control>
          </Field>
          <Field>
            <Label>Website</Label>
            <Control>
              <Input
                placeholder="https://motor-club.co.uk"
                value={club.website}
                type="url"
                onChange={(e: OnChange) =>
                  setField({
                    website: e.target.value,
                  })
                }
              />
            </Control>
          </Field>
          <Field>
            <Label>Admin Emails</Label>
            <Control>
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
                      (_, i) => i !== removeIndex,
                    ),
                  })
                }
              />
            </Control>
            <Help color="danger">
              These addresses are used to manage access to the system, be
              careful!
            </Help>
          </Field>
        </Modal.Card.Body>
        <Modal.Card.Footer>
          <Button loading={saving} color="primary" type="submit">
            Save changes
          </Button>
          <Button type="button" color="secondary" onClick={cancel}>
            Cancel
          </Button>
        </Modal.Card.Footer>
      </Modal.Card>
    </Modal>
  );
};

export default ModalX;
