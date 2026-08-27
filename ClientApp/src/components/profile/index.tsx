import { h, FunctionComponent } from "preact";
import { Button, Box, Columns, Form, Heading } from "react-bulma-components";
const { Label, Input, Field, Radio, Checkbox, Control } = Form;
import { useState } from "preact/hooks";
import { useSelector } from "react-redux";

import { Age, Profile } from "../../types/profileModels";
import { OnChange } from "../../types/inputs";
import EmergencyContactEditor from "../shared/EmergencyContactEditor";
import { EmergencyContact, MsaMembership, Vehicle } from "../../types/shared";
import VehicleEditor from "../shared/VehicleEditor";
import MembershipList from "../shared/MembershipList";
import MsaMembershipEditor from "../shared/MsaMembershipEditor";
import { addPreventDefault } from "../../lib/form";
import FormColumn from "../shared/FormColumn";
import { selectLicenseTypeOptions } from "../../store/event/selectors";

interface Props {
  readonly profile: Profile;
  readonly save: () => Promise<void>;
  readonly setField: (k: Partial<Profile>) => void;
}

const ProfileComp: FunctionComponent<Props> = ({ save, profile, setField }) => {
  const [saving, setSaving] = useState(false);
  const formSave = addPreventDefault(save, setSaving);
  const licenseTypes = useSelector(selectLicenseTypeOptions);
  return (
    <form onSubmit={formSave}>
      <Heading>Profile</Heading>
      <Box>
        <Heading size={5} spaced>
          Personal Information
        </Heading>
        <Columns>
          <Columns.Column>
            <Field>
              <Label>Given Name</Label>
              <Input
                required
                value={profile.givenName}
                onChange={(e: OnChange) =>
                  setField({ givenName: e.target.value })
                }
              />
            </Field>
          </Columns.Column>
          <Columns.Column>
            <Field>
              <Label>Family Name</Label>
              <Input
                required
                value={profile.familyName}
                onChange={(e: OnChange) =>
                  setField({ familyName: e.target.value })
                }
              />
            </Field>
          </Columns.Column>
        </Columns>
        <Columns>
          <FormColumn>
            <Control>
              <Label>Age</Label>
              <Control>
                <Radio
                  checked={profile.age === Age.Junior}
                  onChange={() => setField({ age: Age.Junior })}
                >
                  Junior
                </Radio>
              </Control>
              <Control>
                <Radio
                  checked={profile.age === Age.Senior}
                  onChange={() => setField({ age: Age.Senior })}
                >
                  Senior
                </Radio>
              </Control>
            </Control>
          </FormColumn>
          <FormColumn>
            <Control>
              <Label>Lady?</Label>
              <Checkbox
                checked={profile.isLady}
                onChange={() => setField({ isLady: !profile.isLady })}
              >
                Is Lady
              </Checkbox>
            </Control>
          </FormColumn>
        </Columns>
      </Box>

      <Box>
        <Heading size={5} spaced>
          Vehicle Details
        </Heading>
        <VehicleEditor
          vehicle={profile.vehicle}
          setField={(e: Vehicle) =>
            setField({
              vehicle: e,
            })
          }
          makeAndModels={[]}
        />
      </Box>

      <Box>
        <Heading size={5} spaced>
          Emergency Contact
        </Heading>
        <EmergencyContactEditor
          emergencyContact={profile.emergencyContact}
          setField={(e: EmergencyContact) => {
            setField({
              emergencyContact: e,
            });
          }}
        />
      </Box>

      <Box>
        <Heading size={5} spaced>
          MSA Membership
        </Heading>
        <MsaMembershipEditor
          licenseTypes={licenseTypes}
          membership={profile.msaMembership}
          setField={(e: MsaMembership) =>
            setField({
              msaMembership: e,
            })
          }
        />
      </Box>

      <Box>
        <Heading size={5} spaced>
          Club Memberships
        </Heading>
        <MembershipList
          memberships={profile.clubMemberships}
          addNew={(s) => {
            setField({
              clubMemberships: profile.clubMemberships.concat(s),
            });
          }}
          remove={(removeIndex) => {
            setField({
              clubMemberships: profile.clubMemberships.filter(
                (_, i) => i !== removeIndex,
              ),
            });
          }}
        />
      </Box>

      <Button.Group>
        <Button loading={saving} color="primary" type="submit">
          Save changes
        </Button>
      </Button.Group>
    </form>
  );
};

export default ProfileComp;
