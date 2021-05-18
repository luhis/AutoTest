import { FunctionalComponent, h } from "preact";
import { useCallback, useEffect, useState } from "preact/hooks";
import { Heading, Button } from "react-bulma-components";
import UUID from "uuid-int";
import { useDispatch, useSelector } from "react-redux";

import { Override, Marshal, EditingMarshal } from "../../types/models";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import List from "../../components/marshals/List";
import EntrantsModal from "../../components/marshals/Modal";
import {
    GetMarshalsIfRequired,
    GetEventsIfRequired,
    DeleteMarshal,
    AddMarshal,
} from "../../store/event/actions";
import {
    selectAllRoles,
    selectEvents,
    selectMarshals,
} from "../../store/event/selectors";
import { keySeed } from "../../settings";
import { findIfLoaded } from "../../types/loadingState";
import { selectAccess, selectProfile } from "../../store/profile/selectors";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import Breadcrumbs from "../../components/shared/Breadcrumbs";
import { GetProfileIfRequired } from "../../store/profile/actions";
import { selectClubs } from "../../store/clubs/selectors";
import { GetClubsIfRequired } from "../../store/clubs/actions";

interface Props {
    readonly eventId: number;
}
const uid = UUID(keySeed);

const Marshals: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const marshals = useSelector(selectMarshals);
    const profile = useSelector(selectProfile);
    const allRoles = useSelector(selectAllRoles);
    const currentEvent = findIfLoaded(
        useSelector(selectEvents),
        (a) => a.eventId === eventId
    );
    const currentClub = findIfLoaded(
        useSelector(selectClubs),
        (a) => a.clubId === currentEvent?.clubId
    );
    const [editingMarshal, setEditingMarshal] =
        useState<EditingMarshal | undefined>(undefined);
    const auth = useGoogleAuth();
    const dispatch = useDispatch();
    const save = useCallback(() => {
        if (editingMarshal) {
            dispatch(
                AddMarshal(
                    {
                        ...editingMarshal,
                    },
                    getAccessToken(auth),
                    () => setEditingMarshal(undefined)
                )
            );
        }
    }, [auth, dispatch, editingMarshal]);
    const fillFromProfile = useCallback(() => {
        if (profile.tag === "Loaded") {
            const { familyName, givenName, emergencyContact, emailAddress } =
                profile.value;

            setEditingMarshal((e): EditingMarshal | undefined =>
                e
                    ? {
                          ...e,
                          familyName,
                          givenName,
                          emergencyContact,
                          email: emailAddress,
                      }
                    : undefined
            );
        }
    }, [profile]);

    const deleteMarshal = (marshal: Marshal) => {
        dispatch(DeleteMarshal(marshal, getAccessToken(auth)));
    };
    useEffect(() => {
        dispatch(GetEventsIfRequired());
    }, [dispatch]);
    useEffect(() => {
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
        dispatch(GetMarshalsIfRequired(eventId, getAccessToken(auth)));
    }, [eventId, dispatch, auth]);
    const clearEditingMarshal = () => setEditingMarshal(undefined);

    const newMarshal = useCallback(() => {
        dispatch(GetProfileIfRequired(getAccessToken(auth)));
        setEditingMarshal({
            marshalId: uid.uuid(),
            eventId: eventId,
            givenName: "",
            familyName: "",
            email: "",
            isNew: true,
            emergencyContact: {
                name: "",
                phone: "",
            },
            role: "",
            registrationNumber: Number.NaN,
        });
    }, [auth, dispatch, eventId]);
    const setCurrentEditingMarshal = useCallback(
        (marshal: Marshal) => setEditingMarshal({ ...marshal, isNew: false }),
        []
    );
    const setField = useCallback(
        (a: Partial<EditingMarshal>) =>
            setEditingMarshal((b): EditingMarshal | undefined => {
                if (b !== undefined) {
                    return { ...b, ...a };
                } else {
                    return b;
                }
            }),
        []
    );
    const isClubAdmin = useSelector(selectAccess).adminClubs.includes(
        currentEvent?.clubId || Number.NaN
    );
    return (
        <div>
            <Breadcrumbs club={currentClub} event={currentEvent} />
            <Heading>Marshals</Heading>
            <List
                isClubAdmin={isClubAdmin}
                marshals={marshals}
                setEditingMarshal={setCurrentEditingMarshal}
                deleteMarshal={deleteMarshal}
            />
            <Button color="primary" onClick={newMarshal}>
                Add Marshal
            </Button>
            {editingMarshal ? (
                <EntrantsModal
                    allRoles={allRoles}
                    marshal={editingMarshal}
                    setField={setField}
                    cancel={clearEditingMarshal}
                    save={save}
                    fillFromProfile={fillFromProfile}
                />
            ) : null}
        </div>
    );
};

export default RouteParamsParser<
    Override<
        Props,
        {
            readonly eventId: string;
        }
    >,
    Props
>(({ eventId, ...props }) => ({ ...props, eventId: Number.parseInt(eventId) }))(
    Marshals
);