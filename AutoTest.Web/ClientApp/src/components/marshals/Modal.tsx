import { h, FunctionComponent } from "preact";
import { Modal, Button, Form } from "react-bulma-components";
const { Control, Field, Label, Input, Help, Checkbox } = Form;
import { useState, useEffect } from "preact/hooks";
import { newValidDate } from "ts-date";
import { useSelector } from "react-redux";

import { EditingMarshal } from "../../types/models";
import { OnChange } from "../../types/inputs";
import { EmergencyContact } from "../../types/shared";
import EmergencyContactEditor from "../shared/EmergencyContactEditor";
import { addPreventDefault } from "../../lib/form";
import DropdownInput from "../shared/DropdownInput";
import {
  selectAccessToken,
  selectProfile,
} from "../../store/profile/selectors";
import { useThunkDispatch } from "../../store";
import { getAccessToken } from "../../api/api";
import { GetProfileIfRequired } from "../../store/profile/actions";
import { MarshalAgreement } from "../../settings";

interface Props {
  readonly marshal: EditingMarshal;
  readonly allRoles: readonly string[];
  readonly save: () => Promise<void>;
  readonly cancel: () => void;
  readonly setField: (k: Partial<EditingMarshal>) => void;
  readonly fillFromProfile: () => void;
  readonly isClubAdmin: boolean;
}

const MarshalsModal: FunctionComponent<Props> = ({
  save,
  cancel,
  marshal,
  allRoles,
  setField,
  fillFromProfile,
  isClubAdmin,
}) => {
  const auth = useSelector(selectAccessToken);
  const thunkDispatch = useThunkDispatch();
  const profile = useSelector(selectProfile);
  useEffect(() => {
    void thunkDispatch(GetProfileIfRequired(getAccessToken(auth)));
  }, [thunkDispatch, auth]);

  const [saving, setSaving] = useState(false);
  const formSave = addPreventDefault(save, setSaving);

  return (
    <Modal show={true} showClose={false}>
      <Modal.Card renderAs="form" onSubmit={formSave}>
        <Modal.Card.Header showClose={false}>
          <Modal.Card.Title>
            {marshal.isNew ? "Add" : "Edit"} Marshal
          </Modal.Card.Title>
        </Modal.Card.Header>
        <Modal.Card.Body>
          <Field kind="group">
            <Control fullwidth={true}>
              <Label>Given Name</Label>
              <Input
                required
                value={marshal.givenName}
                onChange={(e: OnChange) =>
                  setField({
                    givenName: e.target.value,
                  })
                }
              />
            </Control>
            <Control fullwidth={true}>
              <Label>Family Name</Label>
              <Input
                required
                value={marshal.familyName}
                onChange={(e: OnChange) =>
                  setField({ familyName: e.target.value })
                }
              />
            </Control>
          </Field>
          <Field>
            <Label>Email</Label>
            <Input
              readOnly={!isClubAdmin}
              required
              value={marshal.email}
              type="email"
              onChange={(e: OnChange) => {
                setField({
                  email: e.target.value,
                });
              }}
            />
            <Help color="danger">
              This address is used to manage access to the system, be careful!
            </Help>
          </Field>
          <EmergencyContactEditor
            emergencyContact={marshal.emergencyContact}
            setField={(e: EmergencyContact) =>
              setField({
                emergencyContact: e,
              })
            }
          />
          <Field>
            <Label>Registration No</Label>
            <Input
              required
              type="number"
              value={marshal.registrationNumber}
              onChange={({ target }: OnChange) =>
                setField({
                  ...marshal,
                  registrationNumber: Math.floor(target.valueAsNumber),
                })
              }
            />
          </Field>
          <Field>
            <Label>Role</Label>
            <DropdownInput
              required
              options={allRoles}
              value={marshal.role}
              setValue={(e) =>
                setField({
                  ...marshal,
                  role: e,
                })
              }
            />
          </Field>
          <Field>
            <Control>
              <Checkbox
                checked={marshal.acceptDeclaration !== null}
                onClick={() =>
                  setField(
                    marshal.acceptDeclaration === null
                      ? {
                          acceptDeclaration: {
                            timeStamp: newValidDate(),
                            isAccepted: true,
                            email:
                              profile.tag === "Loaded"
                                ? profile.value.emailAddress
                                : "",
                          },
                        }
                      : { acceptDeclaration: null },
                  )
                }
              >
                {"  "}I agree to the{" "}
                <a target="_blank" href={MarshalAgreement} rel="noreferrer">
                  Motorsport UK Pre-Event Declaration Form
                </a>
              </Checkbox>
            </Control>
          </Field>
        </Modal.Card.Body>
        <Modal.Card.Footer>
          <Button loading={saving} type="submit" color="primary">
            Save changes
          </Button>
          {marshal.isNew ? (
            <Button color="secondary" type="button" onClick={fillFromProfile}>
              Fill from Profile
            </Button>
          ) : null}
          <Button type="button" color="secondary" onClick={cancel}>
            Cancel
          </Button>
        </Modal.Card.Footer>
      </Modal.Card>
    </Modal>
  );
};

export default MarshalsModal;
