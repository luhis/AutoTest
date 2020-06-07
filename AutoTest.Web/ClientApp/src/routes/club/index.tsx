import { FunctionalComponent, h } from "preact";
import { useEffect } from "preact/hooks";

import { getClubs } from "../../api/clubs";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";

const Club: FunctionalComponent = () => {
    const auth = useGoogleAuth();
    useEffect(() => void getClubs(getAccessToken(auth)), [auth]);
    return (
        <div>
            <h1>Club</h1>
            <p>This is the Club component.</p>
        </div>
    );
};

export default Club;
