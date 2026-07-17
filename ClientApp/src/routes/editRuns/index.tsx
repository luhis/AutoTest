import { FunctionalComponent, h } from "preact";
import {
  StateUpdater,
  useCallback,
  useEffect,
  useState,
  Dispatch,
} from "preact/hooks";
import { Form, Heading, Table } from "react-bulma-components";
import { useSelector } from "react-redux";
import { identity, range } from "@s-libs/micro-dash";

import { Override, TestRunFromServer } from "../../types/models";
import { findIfLoaded, mapOrDefault } from "../../types/loadingState";
import { getAccessToken } from "../../api/api";
import {
  selectEntrants,
  selectEvents,
  selectMarshals,
} from "../../store/event/selectors";
import {
  GetEntrantsIfRequired,
  GetEventsIfRequired,
  GetMarshalsIfRequired,
} from "../../store/event/actions";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import Breadcrumbs from "../../components/shared/Breadcrumbs";
import { selectClubs } from "../../store/clubs/selectors";
import { GetClubsIfRequired } from "../../store/clubs/actions";
import { OnSelectChange } from "../../types/inputs";
import ifSome from "../../components/shared/ifSome";
import Penalties from "../../components/shared/Penalties";
import Modal from "../../components/editRuns/Modal";
import { useThunkDispatch } from "../../store";
import { selectTestRunsFromServer } from "../../store/runs/selectors";
import { GetTestRunsIfRequired, UpdateTestRun } from "../../store/runs/actions";
import { selectAccessToken } from "../../store/profile/selectors";

interface Props {
  readonly eventId: number;
}

const EditRuns: FunctionalComponent<Props> = ({ eventId }) => {
  const thunkDispatch = useThunkDispatch();
  const auth = useSelector(selectAccessToken);
  const currentEvent = findIfLoaded(
    useSelector(selectEvents),
    (a) => a.eventId === eventId,
  );
  const currentClub = findIfLoaded(
    useSelector(selectClubs),
    (a) => a.clubId === currentEvent?.clubId,
  );
  useEffect(() => {
    void thunkDispatch(GetClubsIfRequired(getAccessToken(auth)));
    void thunkDispatch(GetEventsIfRequired());
  }, [thunkDispatch, auth]);

  const [ordinal, setSelectedOrdinal] = useState<number>(0);
  const testRuns = useSelector(selectTestRunsFromServer);
  useEffect(() => {
    void thunkDispatch(
      GetTestRunsIfRequired(eventId, ordinal, getAccessToken(auth)),
    );
    void thunkDispatch(GetEntrantsIfRequired(eventId));
    void thunkDispatch(GetMarshalsIfRequired(eventId));
  }, [auth, thunkDispatch, eventId, ordinal]);

  const entrants = useSelector(selectEntrants);

  const getEntrantName = (entrantId: number) => {
    const found = findIfLoaded(entrants, (a) => a.entrantId === entrantId);
    if (found) {
      return `${found.givenName} ${found.familyName}`;
    } else {
      return "Not Found";
    }
  };
  const currentMarshals = useSelector(selectMarshals);

  const getMarshalName = (marshalId: number) => {
    const found = findIfLoaded(
      currentMarshals,
      (a) => a.marshalId === marshalId,
    );
    return found ? `${found.givenName} ${found.familyName}` : "Not Found";
  };

  const [editing, setEditing] = useState<TestRunFromServer | undefined>(
    undefined,
  );
  const clearEditingRun = () => setEditing(undefined);
  const save = useCallback(async () => {
    if (editing) {
      await thunkDispatch(
        UpdateTestRun(editing, getAccessToken(auth), clearEditingRun),
      );
    }
  }, [auth, thunkDispatch, editing]);
  return (
    <div>
      <Breadcrumbs club={currentClub} event={currentEvent} />
      <Heading>Edit Runs</Heading>
      Test Number:
      <Form.Select<number>
        value={ordinal}
        onChange={(a: OnSelectChange) =>
          setSelectedOrdinal(Number.parseInt(a.target.value))
        }
      >
        {(currentEvent ? range(currentEvent.courseCount) : []).map((a) => (
          <option key={a} value={a}>
            {a + 1}
          </option>
        ))}
      </Form.Select>
      <Table>
        <thead>
          <tr>
            <th>Test Run ID</th>
            <th>Marshal</th>
            <th>Entrant</th>
            <th>Time</th>
            <th>Penalties</th>
            <th>Created</th>
          </tr>
        </thead>
        {ifSome(
          testRuns,
          (r) => r.testRunId,
          (result) => (
            <tr
              key={result.testRunId}
              onClick={() => setEditing(result)}
              class="is-clickable"
            >
              <td>{result.testRunId}</td>
              <td>{getMarshalName(result.marshalId)}</td>
              <td>{getEntrantName(result.entrantId)}</td>
              <td>{(result.timeInMS / 1000).toFixed(2)}s</td>
              <td>
                <Penalties penalties={result.penalties} />
              </td>
              <td>{result.created.toUTCString()}</td>
            </tr>
          ),
          (_) => true,
        )}
      </Table>
      {editing ? (
        <Modal
          run={editing}
          entrants={mapOrDefault(entrants, identity, [])}
          setField={setEditing as Dispatch<StateUpdater<TestRunFromServer>>}
          cancel={clearEditingRun}
          save={save}
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
>(({ eventId, ...props }) => ({
  ...props,
  eventId: Number.parseInt(eventId),
}))(EditRuns);
