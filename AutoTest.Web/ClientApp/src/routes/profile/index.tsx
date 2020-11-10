import { FunctionalComponent, h } from "preact";
import { useEffect } from "preact/hooks";
import { useDispatch, useSelector } from "react-redux";

import { getAccessToken } from "../../api/api";
import { GetProfile } from "../../store/profile/actions";
import { useGoogleAuth } from "../../components/app";
import ProfileModal from "../../components/profile/index";
import { selectProfile } from "../../store/profile/selectors";

interface Props {
    user: string;
}

const Profile: FunctionalComponent<Readonly<Props>> = () => {
    const auth = useGoogleAuth();
    const dispatch = useDispatch();
    const profile = useSelector(selectProfile);
    useEffect(() => {
        dispatch(GetProfile(getAccessToken(auth)));
    }, [dispatch, auth]);
    return profile.tag === "Loaded" ? (
        <ProfileModal
            profile={profile.value}
            save={() => Promise.resolve()}
            cancel={() => undefined}
            setField={() => undefined}
        />
    ) : null;
};

export default Profile;
