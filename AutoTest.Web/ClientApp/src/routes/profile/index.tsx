import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { useDispatch, useSelector } from "react-redux";

import { getAccessToken } from "../../api/api";
import { GetProfile } from "../../store/profile/actions";
import { useGoogleAuth } from "../../components/app";
import ProfileModal from "../../components/profile/index";
import { selectProfile } from "../../store/profile/selectors";
import { saveProfile } from "../../api/user";
import { Profile } from "../../types/profileModels";

interface Props {
    readonly profile: Profile;
}

const ProfileEditor: FunctionalComponent<Readonly<Props>> = ({ profile }) => {
    const auth = useGoogleAuth();
    const dispatch = useDispatch();
    const [editingProfile, setEditingProfile] = useState<Profile>(profile);
    const save = async () => {
        if (editingProfile) {
            await saveProfile(editingProfile, getAccessToken(auth));
            setEditingProfile(editingProfile);
            dispatch(GetProfile(getAccessToken(auth)));
        }
    };
    return (
        <ProfileModal
            profile={editingProfile}
            save={save}
            cancel={() => undefined}
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
    const dispatch = useDispatch();
    const profile = useSelector(selectProfile);
    useEffect(() => {
        dispatch(GetProfile(getAccessToken(auth)));
    }, [dispatch, auth]);

    return profile.tag === "Loaded" ? (
        <ProfileEditor profile={profile.value} />
    ) : null;
};

export default ProfileRoute;
