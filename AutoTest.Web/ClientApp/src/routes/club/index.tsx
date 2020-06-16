import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";

import { getClubs } from "../../api/clubs";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import { Club, LoadingState } from "../../types/models";

const ClubComponent: FunctionalComponent = () => {
    const auth = useGoogleAuth();
    const [clubs, setClubs] = useState<LoadingState<readonly Club[]>>({
        tag: "Loading",
    });
    useEffect(() => {
        const fetchData = async () => {
            setClubs(await getClubs(getAccessToken(auth)));
        };
        void fetchData();
    }, [auth]);
    return (
        <div>
            <h1>Club</h1>
            {clubs.tag === "Loaded"
                ? clubs.value.map((a) => <p key={a.clubId}>{a.clubName}</p>)
                : null}
            <p>This is the Club component.</p>
        </div>
    );
};

export default ClubComponent;
