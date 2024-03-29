import { FunctionalComponent, h, Fragment } from "preact";
import { useCallback, useEffect, useState } from "preact/hooks";
import { useSelector } from "react-redux";

import { getAccessToken } from "../../api/api";
import { GetProfileIfRequired, SaveProfile } from "../../store/profile/actions";
import ProfileModal from "../../components/profile/index";
import {
  selectAccessToken,
  selectProfile,
} from "../../store/profile/selectors";
import { Profile } from "../../types/profileModels";
import { useThunkDispatch } from "../../store";

interface Props {
  readonly profile: Profile;
}

const ProfileEditor: FunctionalComponent<Readonly<Props>> = ({ profile }) => {
  const auth = useSelector(selectAccessToken);
  const thunkDispatch = useThunkDispatch();
  const [editingProfile, setEditingProfile] = useState<Profile>(profile);
  const save = useCallback(async () => {
    await thunkDispatch(SaveProfile(editingProfile, getAccessToken(auth)));
  }, [auth, thunkDispatch, editingProfile]);
  return (
    <ProfileModal
      profile={editingProfile}
      save={save}
      setField={(a: Partial<Profile>) => {
        setEditingProfile((b) => {
          return { ...b, ...a };
        });
      }}
    />
  );
};

const ProfileRoute: FunctionalComponent = () => {
  const auth = useSelector(selectAccessToken);
  const thunkDispatch = useThunkDispatch();
  const profile = useSelector(selectProfile);
  useEffect(() => {
    void thunkDispatch(GetProfileIfRequired(getAccessToken(auth)));
  }, [thunkDispatch, auth]);

  return profile.tag === "Loaded" ? (
    <ProfileEditor profile={profile.value} />
  ) : (
    <Fragment>Loading...</Fragment>
  );
};

export default ProfileRoute;
