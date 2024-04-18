import { h, FunctionComponent } from "preact";
import { Modal, Button, Form, Columns } from "react-bulma-components";
import { useSelector } from "react-redux";
const { Control, Field, Label, Input, Help, Checkbox, Radio, Select } = Form;
import { useState, useEffect } from "preact/hooks";
import { newValidDate } from "ts-date";

import { EditingEntrant, EventType } from "../../types/models";
import { OnChange, OnSelectChange } from "../../types/inputs";
import {
  selectClassOptions,
  selectClubOptions,
  selectEntrants,
  selectLicenseTypeOptions,
  selectMakeModelOptions,
} from "../../store/event/selectors";
import {
  MsaMembership,
  EmergencyContact,
  Vehicle,
  ClubMembership,
} from "src/types/shared";
import EmergencyContactEditor from "../shared/EmergencyContactEditor";
import VehicleEditor from "../shared/VehicleEditor";
import DropdownInput from "../shared/DropdownInput";
import MsaMembershipEditor from "../shared/MsaMembershipEditor";
import { addPreventDefault } from "../../lib/form";
import { Age } from "../../types/profileModels";
import {
  selectAccessToken,
  selectProfile,
} from "../../store/profile/selectors";
import { getAccessToken } from "../../api/api";
import { useThunkDispatch } from "../../store";
import { GetProfileIfRequired } from "../../store/profile/actions";
import { EntrantAgreement } from "../../settings";
import { FaInfoCircle } from "react-icons/fa";
import FormColumn from "../shared/FormColumn";
import ifSome from "../shared/ifSome";
import FillProfileButton from "./subComponents/FillProfileButton";

interface Props {
  readonly entrant: EditingEntrant;
  readonly save: () => Promise<void>;
  readonly cancel: () => void;
  readonly setField: (k: Partial<EditingEntrant>) => void;
  readonly fillFromProfile: (membership: ClubMembership | undefined) => void;
  readonly isClubAdmin: boolean;
  readonly clubMemberships: readonly ClubMembership[];
  readonly eventTypes: readonly EventType[];
}

const EntrantsModal: FunctionComponent<Props> = ({
  save,
  cancel,
  entrant,
  setField,
  fillFromProfile,
  isClubAdmin,
  clubMemberships,
}) => {
  const auth = useSelector(selectAccessToken);
  const thunkDispatch = useThunkDispatch();
  const classesInUse = useSelector(selectClassOptions);
  const entrants = useSelector(selectEntrants);
  const makeAndModels = useSelector(selectMakeModelOptions);
  const licenseTypes = useSelector(selectLicenseTypeOptions);
  const clubOptions = useSelector(selectClubOptions);
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
            {entrant.isNew ? "Add" : "Edit"} Entrant
          </Modal.Card.Title>
        </Modal.Card.Header>
        <Modal.Card.Body>
          <Field kind="group">
            <Control fullwidth={true}>
              <Label>Given Name</Label>
              <Control>
                <Input
                  required
                  value={entrant.givenName}
                  onChange={(e: OnChange) =>
                    setField({
                      givenName: e.target.value,
                    })
                  }
                />
              </Control>
            </Control>
            <Control fullwidth={true}>
              <Label>Family Name</Label>
              <Input
                required
                value={entrant.familyName}
                onChange={(e: OnChange) =>
                  setField({ familyName: e.target.value })
                }
              />
            </Control>
          </Field>
          <Columns>
            <FormColumn>
              <Control fullwidth={true}>
                <Label>Age</Label>
                <Radio
                  checked={entrant.age === Age.Junior}
                  onChange={() => setField({ age: Age.Junior })}
                >
                  Junior
                </Radio>
                <Radio
                  checked={entrant.age === Age.Senior}
                  onChange={() => setField({ age: Age.Senior })}
                >
                  Senior
                </Radio>
              </Control>
            </FormColumn>
            <FormColumn>
              <Control fullwidth={true}>
                <Label>Lady?</Label>
                <Checkbox
                  checked={entrant.isLady}
                  onChange={() => setField({ isLady: !entrant.isLady })}
                >
                  Is Lady{" "}
                  <FaInfoCircle title="Used for fastest lady awards (if given)"></FaInfoCircle>
                </Checkbox>
              </Control>
            </FormColumn>
          </Columns>
          <Field>
            <Label>Email</Label>
            <Input
              readOnly={!isClubAdmin}
              required
              value={entrant.email}
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
          <Field kind="group">
            <Control fullwidth={true}>
              <Label>Club</Label>
              <DropdownInput
                required
                value={entrant.entrantClub.club}
                options={clubOptions}
                setValue={(e) =>
                  setField({
                    entrantClub: {
                      ...entrant.entrantClub,
                      club: e,
                    },
                  })
                }
              />
            </Control>
            <Control fullwidth={true}>
              <Label>Club Number</Label>
              <Input
                required
                value={entrant.entrantClub.clubNumber}
                onChange={(e: OnChange) =>
                  setField({
                    entrantClub: {
                      ...entrant.entrantClub,
                      clubNumber: e.target.value,
                    },
                  })
                }
              />
            </Control>
          </Field>
          <MsaMembershipEditor
            licenseTypes={licenseTypes}
            membership={entrant.msaMembership}
            setField={(e: MsaMembership) =>
              setField({
                msaMembership: e,
              })
            }
          />
          <Field>
            <Control fullwidth>
              <Label>Class</Label>
              <DropdownInput
                required
                value={entrant.class}
                options={classesInUse}
                setValue={(e) => {
                  setField({
                    class: e.toLocaleUpperCase(),
                  });
                }}
              />
            </Control>
          </Field>
          <Field>
            <Control fullwidth>
              <Label>Double Driven With</Label>
              <Select<number>
                required
                fullwidth
                onChange={(event: OnSelectChange) =>
                  setField({
                    doubleDrivenWith: Number.parseInt(event.target.value),
                  })
                }
                value={entrant.doubleDrivenWith}
              >
                <option value={undefined}>- None -</option>
                {ifSome(
                  entrants,
                  (a) => a.entrantId,
                  (a) => (
                    <option value={a.entrantId}>
                      {a.driverNumber}. {a.vehicle.registration} - {a.givenName}{" "}
                      {a.familyName}
                    </option>
                  ),
                )}
              </Select>
            </Control>
          </Field>
          <VehicleEditor
            vehicle={entrant.vehicle}
            setField={(e: Vehicle) =>
              setField({
                vehicle: e,
              })
            }
            makeAndModels={makeAndModels}
          />
          <EmergencyContactEditor
            emergencyContact={entrant.emergencyContact}
            setField={(e: EmergencyContact) =>
              setField({
                emergencyContact: e,
              })
            }
          />
          <Field>
            <Control>
              <Checkbox
                checked={entrant.acceptDeclaration !== null}
                onClick={() =>
                  setField(
                    entrant.acceptDeclaration === null
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
                <a target="_blank" href={EntrantAgreement} rel="noreferrer">
                  Motorsport UK Pre-Event Declaration
                </a>
              </Checkbox>
            </Control>
          </Field>
        </Modal.Card.Body>
        <Modal.Card.Footer>
          <Button loading={saving} type="submit" color="primary">
            Save changes
          </Button>
          {entrant.isNew ? (
            <FillProfileButton
              clubMemberships={clubMemberships}
              fillFromProfile={fillFromProfile}
            />
          ) : null}
          <Button type="button" color="secondary" onClick={cancel}>
            Cancel
          </Button>
        </Modal.Card.Footer>
      </Modal.Card>
    </Modal>
  );
};

export default EntrantsModal;
