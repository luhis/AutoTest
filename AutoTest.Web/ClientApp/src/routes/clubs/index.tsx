import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Link } from "preact-router/match";
import { Column, Title } from "rbx";

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
            <Title>Clubs</Title>
            {clubs.tag === "Loaded"
                ? clubs.value.map((a) => (
                      <Column.Group key={a.clubId}>
                          <Column>{a.clubName}</Column>
                          <Column>
                              <Link href={`/events?clubId=${a.clubId}`}>
                                  Events
                              </Link>
                          </Column>
                      </Column.Group>
                  ))
                : null}
        </div>
    );
};

export default ClubComponent;
