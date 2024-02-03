import { Fragment, FunctionalComponent, h } from "preact";
import { useCallback, useEffect, useState } from "preact/hooks";
import { Heading, Form, Button } from "react-bulma-components";
import UUID from "uuid-int";
import { useSelector } from "react-redux";
import { newValidDate } from "ts-date";
const { Select, Label, Field, Input } = Form;
import { identity } from "@s-libs/micro-dash";

import {
  EditableTestRun,
  Override,
  PenaltyType,
  TestRunUploadState,
  TimingSystem,
} from "../../types/models";
import ifSome from "../../components/shared/ifSome";
import { getAccessToken } from "../../api/api";
import Penalties from "../../components/marshal/Penalties";
import { OnChange, OnSelectChange } from "../../types/inputs";
import {
  GetEventsIfRequired,
  GetEntrantsIfRequired,
} from "../../store/event/actions";
import {
  AddTestRun,
  SyncTestRuns,
  GetTestRunsIfRequired,
} from "../../store/runs/actions";
import { selectEntrants, selectEvents } from "../../store/event/selectors";
import {
  selectRequiresSync,
  selectTestRuns,
  selectTestRunsFromServer,
} from "../../store/runs/selectors";
import { keySeed } from "../../settings";
import ExistingCount from "../../components/marshal/ExistingCount";
import { findIfLoaded, mapOrDefault } from "../../types/loadingState";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import Breadcrumbs from "../../components/shared/Breadcrumbs";
import SyncButton from "../../components/marshal/SyncButton";
import { selectClubs } from "../../store/clubs/selectors";
import { GetClubsIfRequired } from "../../store/clubs/actions";
import { addPreventDefault } from "../../lib/form";
import { useThunkDispatch } from "../../store";
import { selectAccessToken } from "../../store/profile/selectors";

const getNewEditableTest = (ordinal: number): EditableTestRun => ({
  testRunId: uid.uuid(),
  ordinal: ordinal,
  timeInMS: "0.00",
  penalties: [],
  entrantId: Number.NaN,
  state: TestRunUploadState.NotSent,
});
interface Props {
  readonly eventId: number;
  readonly ordinal: number;
}

const uid = UUID(keySeed);

const Marshal: FunctionalComponent<Readonly<Props>> = ({
  eventId,
  ordinal,
}) => {
  const thunkDispatch = useThunkDispatch();
  const entrants = useSelector(selectEntrants);
  const testRuns = useSelector(selectTestRuns);
  const testRunsFromServer = useSelector(selectTestRunsFromServer);
  const requiresSync = useSelector(selectRequiresSync);

  const currentEvent = findIfLoaded(
    useSelector(selectEvents),
    (a) => a.eventId === eventId,
  );
  const currentClub = findIfLoaded(
    useSelector(selectClubs),
    (a) => a.clubId === currentEvent?.clubId,
  );

  const [editing, setEditing] = useState<EditableTestRun>(
    getNewEditableTest(ordinal),
  );
  const auth = useSelector(selectAccessToken);
  useEffect(() => {
    const token = getAccessToken(auth);
    void thunkDispatch(GetEntrantsIfRequired(eventId));
    void thunkDispatch(GetTestRunsIfRequired(eventId, ordinal, token));
  }, [auth, thunkDispatch, eventId, ordinal]);
  useEffect(() => {
    void thunkDispatch(GetEventsIfRequired());
  }, [thunkDispatch]);
  useEffect(() => {
    thunkDispatch(GetClubsIfRequired(getAccessToken(auth)));
  }, [auth, thunkDispatch]);

  //todo can i improve this?
  const increase = (penaltyType: PenaltyType) => {
    setEditing((a) => {
      const found = a.penalties.find(
        (penalty) => penalty.penaltyType === penaltyType,
      );
      return {
        ...a,
        penalties: a.penalties
          .filter((penalty) => penalty.penaltyType !== penaltyType)
          .concat(
            found !== undefined
              ? {
                  ...found,
                  instanceCount: found.instanceCount + 1,
                }
              : {
                  penaltyType: penaltyType,
                  instanceCount: 1,
                },
          ),
      };
    });
  };
  const decrease = (penaltyType: PenaltyType) => {
    setEditing((a) => {
      const found = a.penalties.find(
        (penalty) => penalty.penaltyType === penaltyType,
      );
      return {
        ...a,
        penalties: a.penalties
          .filter((penalty) => penalty.penaltyType !== penaltyType)
          .concat(
            found !== undefined && found.instanceCount > 2
              ? {
                  ...found,
                  instanceCount: found.instanceCount - 1,
                }
              : [],
          ),
      };
    });
  };
  const sync = useCallback(
    () => thunkDispatch(SyncTestRuns(eventId, ordinal, getAccessToken(auth))),
    [auth, thunkDispatch, eventId, ordinal],
  );
  const add = useCallback(async () => {
    if (
      !Number.isNaN(editing.ordinal) &&
      editing.timeInMS !== "" &&
      editing.entrantId !== undefined
    ) {
      await thunkDispatch(
        AddTestRun(
          {
            ...editing,
            created: newValidDate(),
            eventId: eventId,
            timeInMS: Number.parseFloat(editing.timeInMS) * 1000,
            entrantId: editing.entrantId,
          },
          getAccessToken(auth),
        ),
      );
      setEditing(getNewEditableTest(ordinal));
    }
  }, [auth, thunkDispatch, editing, eventId, ordinal]);
  const clearInputs = () => {
    setEditing((a) => getNewEditableTest(a.ordinal)); // todo
  };
  const allTestRuns = mapOrDefault(testRunsFromServer, identity, [])
    .map(({ entrantId, testRunId }) => ({
      entrantId,
      testRunId,
    }))
    .concat(testRuns.filter((a) => a.ordinal === ordinal));

  const [saving, setSaving] = useState(false);
  const formSave = addPreventDefault(add, setSaving);
  return (
    <form onSubmit={formSave}>
      <Breadcrumbs club={currentClub} event={currentEvent} test={ordinal} />
      <Heading>Marshal</Heading>
      <Field>
        <Label>Entrant</Label>
        <Select<number>
          required
          fullwidth
          onChange={(event: OnSelectChange) =>
            setEditing((e) => ({
              ...e,
              entrantId: Number.parseInt(event.target.value),
            }))
          }
          value={editing.entrantId}
        >
          <option disabled value={undefined}>
            - Please Select -
          </option>
          {ifSome(
            entrants,
            (a) => a.entrantId,
            (a) => (
              <option value={a.entrantId}>
                {a.driverNumber}. {a.vehicle.registration} - {a.givenName}{" "}
                {a.familyName}
              </option>
            ),
          )}
        </Select>
      </Field>
      <Field>
        <Label>Existing Count</Label>
        <ExistingCount
          entrantId={editing.entrantId}
          currentEvent={currentEvent}
          testRuns={allTestRuns}
        />
      </Field>
      <Field>
        <Label>Time (Secs)</Label>
        {currentEvent?.timingSystem === TimingSystem.StopWatch ? (
          <Input
            required
            type="number"
            min="0"
            step="0.01"
            value={editing.timeInMS}
            onChange={(e: OnChange) =>
              setEditing((a) => ({
                ...a,
                timeInMS: e.target.value,
              }))
            }
          />
        ) : (
          <Fragment></Fragment>
        )}
      </Field>
      <Field>
        <Penalties
          penalties={editing.penalties}
          increase={increase}
          decrease={decrease}
        />
      </Field>
      <Button.Group>
        <Button color="info" type="button" onClick={clearInputs}>
          Clear
        </Button>
        <Button color="primary" loading={saving}>
          Add
        </Button>
        <SyncButton unSyncedCount={requiresSync} sync={sync} />
      </Button.Group>
    </form>
  );
};
export default RouteParamsParser<
  Override<
    Props,
    {
      readonly ordinal: string;
      readonly eventId: string;
    }
  >,
  Props
>(({ eventId, ordinal, ...props }) => ({
  ...props,
  eventId: Number.parseInt(eventId),
  ordinal: Number.parseInt(ordinal),
}))(Marshal);
