import { FunctionalComponent, h } from "preact";
import { useCallback, useEffect, useState } from "preact/hooks";
import { Heading, Button } from "react-bulma-components";
import UUID from "uuid-int";
import { useDispatch, useSelector } from "react-redux";

import { Entrant, EditingEntrant, Override } from "../../types/models";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import List from "../../components/entrants/List";
import EntrantsModal from "../../components/entrants/Modal";
import {
    GetEntrantsIfRequired,
    GetClubsIfRequired,
    SetPaid,
    DeleteEntrant,
    AddEntrant,
    GetEventsIfRequired,
} from "../../store/event/actions";
import {
    selectEntrants,
    selectEvents,
    selectClubs,
} from "../../store/event/selectors";
import { keySeed } from "../../settings";
import { findIfLoaded, mapOrDefault } from "../../types/loadingState";
import { selectProfile } from "../../store/profile/selectors";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import Breadcrumbs from "../../components/shared/Breadcrumbs";

interface Props {
    readonly eventId: number;
}
const uid = UUID(keySeed);

const Events: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const entrants = useSelector(selectEntrants);
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
        useState<EditingEntrant | undefined>(undefined);
    const auth = useGoogleAuth();
    const dispatch = useDispatch();
    const save = useCallback(() => {
        if (editingEntrant) {
            dispatch(
                AddEntrant(
                    {
                        ...editingEntrant,
                        driverNumber: mapOrDefault(
                            entrants,
                            (x) =>
                                Math.max(
                                    ...x
                                        .map((entrant) => entrant.driverNumber)
                                        .concat(0)
                                ) + 1,
                            -1
                        ),
                    },
                    getAccessToken(auth),
                    () => setEditingEntrant(undefined)
                )
            );
        }
    }, [auth, dispatch, editingEntrant, entrants]);
    const fillFromProfile = useCallback(() => {
        if (profile.tag === "Loaded") {
            const {
                familyName,
                givenName,
                msaLicense,
                emergencyContact,
                vehicle,
                clubMemberships,
            } = profile.value;
            const validMemberships = clubMemberships.filter(
                (a) =>
                    currentEvent == undefined ||
                    a.expiry >= currentEvent.startTime
            );
            const club = validMemberships.length
                ? validMemberships[0].clubName
                : "";
            setEditingEntrant((e) =>
                e
                    ? {
                          ...e,
                          familyName,
                          givenName,
                          msaLicense,
                          emergencyContact,
                          vehicle,
                          club,
                      }
                    : undefined
            );
        }
    }, [currentEvent, profile]);
    const setPaid = (entrant: Entrant, isPaid: boolean) => {
        dispatch(SetPaid(entrant, isPaid, getAccessToken(auth)));
    };
    const deleteEntrant = (entrant: Entrant) => {
        dispatch(DeleteEntrant(entrant, getAccessToken(auth)));
    };
    useEffect(() => {
        dispatch(GetEventsIfRequired());
    }, [dispatch]);
    useEffect(() => {
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
        dispatch(GetEntrantsIfRequired(eventId, getAccessToken(auth)));
    }, [eventId, dispatch, auth]);
    const clearEditingEntrant = () => setEditingEntrant(undefined);

    const newEntrant = useCallback(
        () =>
            setEditingEntrant({
                entrantId: uid.uuid(),
                eventId: eventId,
                class: "",
                givenName: "",
                familyName: "",
                msaLicense: "",
                vehicle: {
                    make: "",
                    model: "",
                    year: 0,
                    displacement: 0,
                    registration: "",
                },
                isNew: true,
                emergencyContact: {
                    name: "",
                    phone: "",
                },
                club: "",
                isPaid: false,
            }),
        [eventId]
    );
    const setCurrentEditingEntrant = useCallback(
        (entrant: Entrant) => setEditingEntrant({ ...entrant, isNew: false }),
        []
    );
    const setField = useCallback(
        (a: Partial<Entrant>) =>
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
            <Heading>Entrants</Heading>
            <List
                entrants={entrants}
                setEditingEntrant={setCurrentEditingEntrant}
                markPaid={setPaid}
                deleteEntrant={deleteEntrant}
            />
            <Button onClick={newEntrant}>Add Entrant</Button>
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
    Events
);
