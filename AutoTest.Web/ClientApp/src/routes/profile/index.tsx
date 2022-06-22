import { FunctionalComponent, h } from "preact";
import { useCallback, useEffect, useState } from "preact/hooks";
import { useSelector } from "react-redux";

import { getAccessToken } from "../../api/api";
import { GetProfileIfRequired, SaveProfile } from "../../store/profile/actions";
import { useGoogleAuth } from "../../components/app";
import ProfileModal from "../../components/profile/index";
import { selectProfile } from "../../store/profile/selectors";
import { Profile } from "../../types/profileModels";
import { useThunkDispatch } from "../../store";

interface Props {
    readonly profile: Profile;
}

const ProfileEditor: FunctionalComponent<Readonly<Props>> = ({ profile }) => {
    const auth = useGoogleAuth();
    const thunkDispatch = useThunkDispatch();
    const [editingProfile, setEditingProfile] = useState<Profile>(profile);
    const save = useCallback(async () => {
        if (editingProfile) {
            await thunkDispatch(
                SaveProfile(editingProfile, getAccessToken(auth))
            );
        }
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
    const auth = useGoogleAuth();
    const thunkDispatch = useThunkDispatch();
    const profile = useSelector(selectProfile);
    useEffect(() => {
        void thunkDispatch(GetProfileIfRequired(getAccessToken(auth)));
    }, [thunkDispatch, auth]);

    return profile.tag === "Loaded" ? (
        <ProfileEditor profile={profile.value} />
    ) : null;
};

export default ProfileRoute;
