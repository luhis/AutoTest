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
    GetClubsIfRequired,
    GetEventsIfRequired,
    DeleteMarshal,
    AddMarshal,
} from "../../store/event/actions";
import {
    selectEvents,
    selectClubs,
    selectMarshals,
} from "../../store/event/selectors";
import { keySeed } from "../../settings";
import { findIfLoaded } from "../../types/loadingState";
import { selectProfile } from "../../store/profile/selectors";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import Breadcrumbs from "../../components/shared/Breadcrumbs";
import { GetProfileIfRequired } from "../../store/profile/actions";

interface Props {
    readonly eventId: number;
}
const uid = UUID(keySeed);

const Marshals: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const marshals = useSelector(selectMarshals);
    const profile = useSelector(selectProfile);
    const currentEvent = findIfLoaded(
        useSelector(selectEvents),
        (a) => a.eventId === eventId
    );
    const currentClub = findIfLoaded(
        useSelector(selectClubs),
        (a) => a.clubId === currentEvent?.clubId
    );
    const [editingEntrant, setEditingEntrant] =
        useState<EditingMarshal | undefined>(undefined);
    const auth = useGoogleAuth();
    const dispatch = useDispatch();
    const save = useCallback(() => {
        if (editingEntrant) {
            dispatch(
                AddMarshal(
                    {
                        ...editingEntrant,
                    },
                    getAccessToken(auth),
                    () => setEditingEntrant(undefined)
                )
            );
        }
    }, [auth, dispatch, editingEntrant]);
    const fillFromProfile = useCallback(() => {
        dispatch(GetProfileIfRequired(getAccessToken(auth)));
        if (profile.tag === "Loaded") {
            const { familyName, givenName, msaMembership, emergencyContact } =
                profile.value;

            setEditingEntrant((e) =>
                e
                    ? {
                          ...e,
                          familyName,
                          givenName,
                          msaMembership,
                          emergencyContact,
                      }
                    : undefined
            );
        }
    }, [auth, dispatch, profile]);

    const deleteEntrant = (entrant: Marshal) => {
        dispatch(DeleteMarshal(entrant, getAccessToken(auth)));
    };
    useEffect(() => {
        dispatch(GetEventsIfRequired());
    }, [dispatch]);
    useEffect(() => {
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
        dispatch(GetMarshalsIfRequired(eventId, getAccessToken(auth)));
    }, [eventId, dispatch, auth]);
    const clearEditingEntrant = () => setEditingEntrant(undefined);

    const newEntrant = useCallback(
        () =>
            setEditingEntrant({
                marshalId: uid.uuid(),
                eventId: eventId,
                givenName: "",
                familyName: "",
                isNew: true,
                emergencyContact: {
                    name: "",
                    phone: "",
                },
                role: "",
            }),
        [eventId]
    );
    const setCurrentEditingEntrant = useCallback(
        (entrant: Marshal) => setEditingEntrant({ ...entrant, isNew: false }),
        []
    );
    const setField = useCallback(
        (a: Partial<EditingMarshal>) =>
            setEditingEntrant((b) => {
                if (b !== undefined) {
                    return { ...b, ...a };
                } else {
                    return b;
                }
            }),
        []
    );

    return (
        <div>
            <Breadcrumbs club={currentClub} event={currentEvent} />
            <Heading>Marshals</Heading>
            <List
                marshals={marshals}
                setEditingEntrant={setCurrentEditingEntrant}
                deleteEntrant={deleteEntrant}
            />
            <Button onClick={newEntrant}>Add Marshal</Button>
            {editingEntrant ? (
                <EntrantsModal
                    entrant={editingEntrant}
                    setField={setField}
                    cancel={clearEditingEntrant}
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
