import { FunctionalComponent, h } from "preact";
import { useCallback, useEffect, useState } from "preact/hooks";
import { Button, Heading } from "react-bulma-components";
import UUID from "uuid-int";
import { useDispatch, useSelector } from "react-redux";

import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import { Club, EditingClub } from "../../types/models";
import List from "../../components/clubs/List";
import Modal from "../../components/clubs/Modal";
import { keySeed } from "../../settings";
import { selectClubs } from "../../store/clubs/selectors";
import {
    AddClub,
    DeleteClub,
    GetClubsIfRequired,
} from "../../store/clubs/actions";
import { selectAccess } from "../../store/profile/selectors";

const uid = UUID(keySeed);

const ClubComponent: FunctionalComponent = () => {
    const auth = useGoogleAuth();
    const dispatch = useDispatch();
    const clubs = useSelector(selectClubs);
    const [editingClub, setEditingClub] =
        useState<EditingClub | undefined>(undefined);
    useEffect(() => {
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
    }, [auth, dispatch]);

    const save = useCallback(() => {
        if (editingClub) {
            dispatch(
                AddClub(editingClub, getAccessToken(auth), () =>
                    setEditingClub(undefined)
                )
            );
        }
    }, [auth, dispatch, editingClub]);

    const deleteClub = useCallback(
        (club: Club) => {
            dispatch(DeleteClub(club.clubId, getAccessToken(auth)));
        },
        [auth, dispatch]
    );
    const newClub = useCallback(
        () =>
            setEditingClub({
                clubId: uid.uuid(),
                clubName: "",
                website: "",
                clubPaymentAddress: "",
                adminEmails: [],
                isNew: true,
            }),
        []
    );
    const clearEditingClub = useCallback(() => setEditingClub(undefined), []);
    const setCurrentEditingClub = useCallback(
        (a: Club) => setEditingClub({ ...a, isNew: false }),
        []
    );
    const { adminClubs, isRootAdmin } = useSelector(selectAccess);
    const isClubAdmin = useCallback(
        (club: Club) => adminClubs.includes(club.clubId) || isRootAdmin,
        [adminClubs, isRootAdmin]
    );
    return (
        <div>
            <Heading>Clubs</Heading>
            <List
                isClubAdmin={isClubAdmin}
                clubs={clubs}
                setEditingClub={setCurrentEditingClub}
                deleteClub={deleteClub}
            />
            <Button color="primary" onClick={newClub}>
                Add Club
            </Button>
            {editingClub ? (
                <Modal
                    club={editingClub}
                    setField={(a: Partial<Club>) =>
                        setEditingClub((b) => ({ ...b, ...a } as EditingClub))
                    }
                    cancel={clearEditingClub}
                    save={save}
                />
            ) : null}
        </div>
    );
};

export default ClubComponent;
