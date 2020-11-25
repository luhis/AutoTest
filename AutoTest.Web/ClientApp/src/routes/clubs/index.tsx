import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Button, Title } from "rbx";
import UUID from "uuid-int";
import { useDispatch, useSelector } from "react-redux";

import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import { Club, EditingClub } from "../../types/models";
import List from "../../components/clubs/List";
import Modal from "../../components/clubs/Modal";
import { keySeed } from "../../settings";
import {
    AddClub,
    DeleteClub,
    GetClubsIfRequired,
} from "../../store/event/actions";
import { selectClubs } from "../../store/event/selectors";

const uid = UUID(keySeed);

const ClubComponent: FunctionalComponent = () => {
    const auth = useGoogleAuth();
    const dispatch = useDispatch();
    const clubs = useSelector(selectClubs);
    const [editingClub, setEditingClub] = useState<EditingClub | undefined>(
        undefined
    );
    useEffect(() => {
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
    }, [auth, dispatch]);

    const save = () => {
        if (editingClub) {
            dispatch(
                AddClub(editingClub, getAccessToken(auth), () =>
                    setEditingClub(undefined)
                )
            );
        }
    };

    const deleteClub = (club: Club) => {
        dispatch(DeleteClub(club.clubId, getAccessToken(auth)));
    };
    return (
        <div>
            <Title>Clubs</Title>
            <List
                clubs={clubs}
                setEditingClub={(a) => setEditingClub({ ...a, isNew: false })}
                deleteClub={deleteClub}
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
                        setEditingClub((b) => ({ ...b, ...a } as EditingClub))
                    }
                    cancel={() => setEditingClub(undefined)}
                    save={save}
                />
            ) : null}
        </div>
    );
};

export default ClubComponent;
