import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Column, Title, Button } from "rbx";
import { route } from "preact-router";
import UUID from "uuid-int";

import { getClubs, addClub } from "../../api/clubs";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import { Club, LoadingState } from "../../types/models";
import ifSome from "../../components/shared/isSome";
import Modal from "../../components/clubs/Modal";

const uid = UUID(Number.parseInt(process.env.PREACT_APP_KEY_SEED as string));

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
        <div>
            <Title>Clubs</Title>
            {ifSome(clubs, (a) => (
                <Column.Group key={a.clubId}>
                    <Column>
                        {a.clubName}
                        &nbsp;
                        {a.website !== "" ? (
                            <a href={a.website}>Homepage</a>
                        ) : null}
                    </Column>
                    <Column>
                        <Button.Group>
                            <Button
                                onClick={() =>
                                    route(`/events?clubId=${a.clubId}`)
                                }
                            >
                                Events
                            </Button>
                            <Button onClick={() => setEditingClub(a)}>
                                Edit
                            </Button>
                        </Button.Group>
                    </Column>
                </Column.Group>
            ))}
            <Button
                onClick={() =>
                    setEditingClub({
                        clubId: uid.uuid(),
                        clubName: "",
                        website: "",
                        clubPaymentAddress: "",
                    })
                }
            >
                Add Club
            </Button>
            {editingClub ? (
                <Modal
                    club={editingClub}
                    setField={(a: Partial<Club>) =>
                        setEditingClub((b) => ({ ...b, ...a } as Club))
                    }
                    cancel={() => setEditingClub(undefined)}
                    save={save}
                />
            ) : null}
        </div>
    );
};

export default ClubComponent;
