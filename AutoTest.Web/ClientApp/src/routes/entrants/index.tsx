import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Button, Breadcrumb } from "rbx";
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
} from "../../store/event/actions";
import {
    selectEntrants,
    selectEvents,
    selectClubs,
} from "../../store/event/selectors";
import { keySeed } from "../../settings";
import { findIfLoaded } from "../../types/loadingState";
import { selectProfile } from "../../store/profile/selectors";
import RouteParamsParser from "../../components/shared/RouteParamsParser";

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
    const [editingEntrant, setEditingEntrant] = useState<
        EditingEntrant | undefined
    >(undefined);
    const auth = useGoogleAuth();
    const dispatch = useDispatch();
    const save = () => {
        if (editingEntrant) {
            dispatch(
                AddEntrant(
                    {
                        ...editingEntrant,
                        driverNumber:
                            entrants.tag === "Loaded"
                                ? Math.max(
                                      ...entrants.value
                                          .map((a) => a.driverNumber)
                                          .concat(0)
                                  ) + 1
                                : -1,
                    },
                    getAccessToken(auth),
                    () => setEditingEntrant(undefined)
                )
            );
        }
    };
    const fillFromProfile = () => {
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
    };
    const setPaid = (entrant: Entrant, isPaid: boolean) => {
        dispatch(SetPaid(entrant, isPaid, getAccessToken(auth)));
    };
    const deleteEntrant = (entrant: Entrant) => {
        dispatch(DeleteEntrant(entrant, getAccessToken(auth)));
    };
    useEffect(() => {
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
        dispatch(GetEntrantsIfRequired(eventId, getAccessToken(auth)));
    }, [eventId, dispatch, auth]);

    return (
        <div>
            <Breadcrumb>
                <Breadcrumb.Item
                    href={`/events?clubId=${currentClub?.clubId || 0}`}
                >
                    {currentClub?.clubName}
                </Breadcrumb.Item>
                <Breadcrumb.Item>{currentEvent?.location}</Breadcrumb.Item>
            </Breadcrumb>
            <Title>Entrants</Title>
            <List
                entrants={entrants}
                setEditingEntrant={(a) =>
                    setEditingEntrant({ ...a, isNew: false })
                }
                markPaid={setPaid}
                deleteEntrant={deleteEntrant}
            />
            <Button
                onClick={() =>
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
                    })
                }
            >
                Add Entrant
            </Button>
            {editingEntrant ? (
                <EntrantsModal
                    entrant={editingEntrant}
                    setField={(a: Partial<Entrant>) => {
                        setEditingEntrant((b) => {
                            if (b !== undefined) {
                                return { ...b, ...a };
                            } else {
                                return b;
                            }
                        });
                    }}
                    cancel={() => setEditingEntrant(undefined)}
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
