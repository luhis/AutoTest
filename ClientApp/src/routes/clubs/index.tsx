import { FunctionalComponent, h } from "preact";
import { useCallback, useEffect, useState } from "preact/hooks";
import { Button, Heading } from "react-bulma-components";
import UUID from "uuid-int";
import { useSelector } from "react-redux";

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
import { selectAccess, selectAccessToken } from "../../store/profile/selectors";
import { useThunkDispatch } from "../../store";

const uid = UUID(keySeed);

const ClubComponent: FunctionalComponent = () => {
  const auth = useSelector(selectAccessToken);
  const thunkDispatch = useThunkDispatch();
  const clubs = useSelector(selectClubs);
  const [editingClub, setEditingClub] = useState<EditingClub | undefined>(
    undefined,
  );
  useEffect(() => {
    thunkDispatch(GetClubsIfRequired(getAccessToken(auth)));
  }, [auth, thunkDispatch]);

  const save = useCallback(async () => {
    if (editingClub) {
      await thunkDispatch(
        AddClub(editingClub, getAccessToken(auth), () =>
          setEditingClub(undefined),
        ),
      );
    }
  }, [auth, thunkDispatch, editingClub]);

  const deleteClub = useCallback(
    (club: Club) =>
      thunkDispatch(DeleteClub(club.clubId, getAccessToken(auth))),
    [auth, thunkDispatch],
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
    [],
  );
  const clearEditingClub = useCallback(() => setEditingClub(undefined), []);
  const setCurrentEditingClub = useCallback(
    (a: Club) => setEditingClub({ ...a, isNew: false }),
    [],
  );
  const { adminClubs, isRootAdmin } = useSelector(selectAccess);
  const isClubAdmin = useCallback(
    (club: Club) => adminClubs.includes(club.clubId) || isRootAdmin,
    [adminClubs, isRootAdmin],
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
      <Button color="primary" onClick={newClub} disabled={!isRootAdmin}>
        Add Club
      </Button>
      {editingClub ? (
        <Modal
          club={editingClub}
          setField={(a: Partial<Club>) =>
            setEditingClub((b) => ({ ...b, ...a }) as EditingClub)
          }
          cancel={clearEditingClub}
          save={save}
        />
      ) : null}
    </div>
  );
};

export default ClubComponent;
