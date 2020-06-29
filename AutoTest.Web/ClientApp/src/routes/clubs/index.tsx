import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";

import { getClubs, addClub } from "../../api/clubs";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import { Club, LoadingState } from "../../types/models";
import Clubs from "../../components/clubs";

const ClubComponent: FunctionalComponent = () => {
    const auth = useGoogleAuth();
    const [clubs, setClubs] = useState<LoadingState<readonly Club[]>>({
        tag: "Loading",
    });
    const [editingClub, setEditingClub] = useState<Club | undefined>(undefined);
    useEffect(() => {
        const fetchData = async () => {
            setClubs(await getClubs(getAccessToken(auth)));
        };
        void fetchData();
    }, [auth]);

    const save = async () => {
        if (editingClub) {
            await addClub(editingClub, getAccessToken(auth));
            setEditingClub(undefined);
            setClubs(await getClubs(getAccessToken(auth)));
        }
    };
    return (
        <Clubs
            clubs={clubs}
            editingClub={editingClub}
            save={save}
            setEditingClub={setEditingClub}
        />
    );
};

export default ClubComponent;
