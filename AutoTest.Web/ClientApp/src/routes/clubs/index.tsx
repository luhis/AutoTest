import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Button, Title } from "rbx";
import UUID from "uuid-int";

import { getClubs, addClub } from "../../api/clubs";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import { Club, EditingClub } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import List from "../../components/clubs/List";
import Modal from "../../components/clubs/Modal";

const uid = UUID(Number.parseInt(process.env.PREACT_APP_KEY_SEED as string));

const ClubComponent: FunctionalComponent = () => {
    const auth = useGoogleAuth();
    const [clubs, setClubs] = useState<LoadingState<readonly Club[]>>({
        tag: "Loading",
        id: undefined,
    });
    const [editingClub, setEditingClub] = useState<EditingClub | undefined>(
        undefined
    );
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
            <List
                clubs={clubs}
                setEditingClub={(a) => setEditingClub({ ...a, isNew: true })}
            />
            <Button
                onClick={() =>
                    setEditingClub({
                        clubId: uid.uuid(),
                        clubName: "",
                        website: "",
                        clubPaymentAddress: "",
                        adminEmails: [],
                        isNew: true,
                    })
                }
            >
                Add Club
            </Button>
            {editingClub ? (
                <Modal
                    club={editingClub}
                    setField={(a: Partial<Club>) =>
                        setEditingClub(
                            (b) =>
                                ({ ...b, ...a } as Club & {
                                    readonly isNew: boolean;
                                })
                        )
                    }
                    cancel={() => setEditingClub(undefined)}
                    save={save}
                />
            ) : null}
        </div>
    );
};

export default ClubComponent;
