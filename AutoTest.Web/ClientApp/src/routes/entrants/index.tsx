import { FunctionalComponent, h } from "preact";
import { useCallback, useEffect, useState } from "preact/hooks";
import { Heading, Button } from "react-bulma-components";
import UUID from "uuid-int";
import { useSelector } from "react-redux";
import { newValidDate } from "ts-date";

import {
  EditingEntrant,
  Override,
  Payment,
  PublicEntrant,
} from "../../types/models";
import { getAccessToken } from "../../api/api";
import List from "../../components/entrants/List";
import EntrantsModal from "../../components/entrants/Modal";
import {
  GetEntrantsIfRequired,
  SetPaid,
  DeleteEntrant,
  AddEntrant,
  GetEventsIfRequired,
} from "../../store/event/actions";
import { selectEntrants, selectEvents } from "../../store/event/selectors";
import { keySeed } from "../../settings";
import {
  findIfLoaded,
  LoadingState,
  mapOrDefault,
} from "../../types/loadingState";
import {
  selectAccess,
  selectAccessToken,
  selectProfile,
} from "../../store/profile/selectors";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import Breadcrumbs from "../../components/shared/Breadcrumbs";
import { GetProfileIfRequired } from "../../store/profile/actions";
import { selectClubs } from "../../store/clubs/selectors";
import { GetClubsIfRequired } from "../../store/clubs/actions";
import { getEntrant } from "../../api/entrants";
import { Age } from "../../types/profileModels";
import { ClubMembership, InductionTypes } from "../../types/shared";
import { useThunkDispatch } from "../../store";

interface Props {
  readonly eventId: number;
}
const uid = UUID(keySeed);

const getDriverNumber = (
  entrants: LoadingState<readonly PublicEntrant[], number>,
  entrant: EditingEntrant,
) => {
  return mapOrDefault(
    entrants,
    (x) => {
      const found = x.find(({ entrantId }) => entrantId === entrant.entrantId);
      if (found) {
        return found.driverNumber;
      }
      return (
        Math.max(...x.map(({ driverNumber }) => driverNumber).concat(0)) + 1
      );
    },
    -1,
  );
};

const blankEntrant = (eventId: number): EditingEntrant => ({
  entrantId: uid.uuid(),
  eventId: eventId,
  class: "",
  givenName: "",
  familyName: "",
  email: "",
  msaMembership: { msaLicense: Number.NaN, msaLicenseType: "" },
  age: Age.Senior,
  isLady: false,
  vehicle: {
    make: "",
    model: "",
    year: Number.NaN,
    displacement: Number.NaN,
    registration: "",
    induction: InductionTypes.NA,
  },
  isNew: true,
  emergencyContact: {
    name: "",
    phone: "",
  },
  entrantClub: { club: "", clubNumber: "" },
  payment: null,
  acceptDeclaration: null,
  doubleDrivenWith: null,
});

const Entrants: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
  const access = useSelector(selectAccess);
  const entrants = useSelector(selectEntrants);
  const profile = useSelector(selectProfile);
  const currentEvent = findIfLoaded(
    useSelector(selectEvents),
    (a) => a.eventId === eventId,
  );
  const currentClub = findIfLoaded(
    useSelector(selectClubs),
    (a) => a.clubId === currentEvent?.clubId,
  );
  const [editingEntrant, setEditingEntrant] = useState<
    EditingEntrant | undefined
  >(undefined);
  const auth = useSelector(selectAccessToken);
  const thunkDispatch = useThunkDispatch();

  const save = useCallback(async () => {
    if (editingEntrant) {
      await thunkDispatch(
        AddEntrant(
          {
            ...editingEntrant,
            driverNumber: getDriverNumber(entrants, editingEntrant),
          },
          getAccessToken(auth),
          clearEditingEntrant,
        ),
      );
    }
  }, [auth, thunkDispatch, editingEntrant, entrants]);

  const filterMemberships = (clubMemberships: readonly ClubMembership[]) =>
    clubMemberships.filter(
      (a) => currentEvent === undefined || a.expiry >= currentEvent.startTime,
    );
  const fillFromProfile = useCallback(
    (clubMembership: ClubMembership | undefined) => {
      if (profile.tag === "Loaded") {
        const {
          familyName,
          givenName,
          msaMembership,
          emergencyContact,
          vehicle,
          emailAddress,
        } = profile.value;
        setEditingEntrant((e) =>
          e
            ? {
                ...e,
                familyName,
                givenName,
                msaMembership,
                emergencyContact,
                vehicle,
                club: clubMembership?.clubName || "",
                clubNumber: clubMembership?.membershipNumber || "",
                email: emailAddress,
              }
            : undefined,
        );
      }
    },
    [profile],
  );
  const setPaid = (entrant: PublicEntrant, payment: Payment | null) =>
    thunkDispatch(SetPaid(entrant, payment, getAccessToken(auth)));

  const deleteEntrant = (entrant: PublicEntrant) =>
    thunkDispatch(DeleteEntrant(entrant, getAccessToken(auth)));

  useEffect(() => {
    void thunkDispatch(GetEventsIfRequired());
  }, [thunkDispatch]);
  useEffect(() => {
    thunkDispatch(GetClubsIfRequired(getAccessToken(auth)));
    void thunkDispatch(GetEntrantsIfRequired(eventId));
  }, [eventId, thunkDispatch, auth]);
  const clearEditingEntrant = () => setEditingEntrant(undefined);

  const newEntrant = useCallback(async () => {
    await thunkDispatch(GetProfileIfRequired(getAccessToken(auth)));
    setEditingEntrant(blankEntrant(eventId));
  }, [auth, thunkDispatch, eventId]);
  const setCurrentEditingEntrant = useCallback(
    async (entrant: PublicEntrant) => {
      const e = await getEntrant(
        entrant.eventId,
        entrant.entrantId,
        getAccessToken(auth),
      );
      setEditingEntrant({ ...e, isNew: false });
    },
    [auth],
  );
  const setField = useCallback(
    (a: Partial<EditingEntrant>) =>
      setEditingEntrant((b) => {
        if (b !== undefined) {
          return { ...b, ...a };
        } else {
          return b;
        }
      }),
    [],
  );
  const isClubAdmin =
    currentEvent !== undefined &&
    access.adminClubs.includes(currentEvent.clubId);
  const canEditEntrant = (entrantId: number) =>
    isClubAdmin || access.editableEntrants.includes(entrantId);
  const isTooEarly =
    currentEvent !== undefined && currentEvent.entryOpenDate > newValidDate();
  const isTooLate =
    currentEvent !== undefined && currentEvent.entryCloseDate < newValidDate();

  const addEntrantDisabled = !access.isLoggedIn || isTooEarly || isTooLate;

  const clauses = [
    { failed: !access.isLoggedIn, message: "Not Logged In" },
    { failed: isTooEarly, message: "Is too early" },
    { failed: isTooLate, message: "Is too late" },
  ];
  const addEntrantText = !addEntrantDisabled
    ? undefined
    : clauses
        .filter(({ failed }) => failed)
        .map(({ message }) => message)
        .join(", ");
  return (
    <div>
      <Breadcrumbs club={currentClub} event={currentEvent} />
      <Heading>Entrants</Heading>
      <List
        isClubAdmin={isClubAdmin}
        entrants={entrants}
        setEditingEntrant={setCurrentEditingEntrant}
        markPaid={setPaid}
        deleteEntrant={deleteEntrant}
        canEditEntrant={canEditEntrant}
      />
      <Button
        disabled={addEntrantDisabled}
        color="primary"
        onClick={newEntrant}
        title={addEntrantText}
      >
        Add Entrant
      </Button>
      {editingEntrant ? (
        <EntrantsModal
          clubMemberships={mapOrDefault(
            profile,
            (a) => filterMemberships(a.clubMemberships),
            [],
          )}
          isClubAdmin={isClubAdmin}
          entrant={editingEntrant}
          setField={setField}
          cancel={clearEditingEntrant}
          save={save}
          fillFromProfile={fillFromProfile}
          eventTypes={currentEvent?.eventTypes || []}
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
  Entrants,
);
