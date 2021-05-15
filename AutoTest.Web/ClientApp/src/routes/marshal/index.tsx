import { FunctionalComponent, h } from "preact";
import { useCallback, useEffect, useState } from "preact/hooks";
import { Heading, Form, Button } from "react-bulma-components";
import UUID from "uuid-int";
import { useSelector, useDispatch } from "react-redux";
import { newValidDate } from "ts-date";
const { Select, Label, Field, Input } = Form;

import {
    EditableTestRun,
    Override,
    PenaltyType,
    TestRunTemp,
} from "../../types/models";
import ifSome from "../../components/shared/ifSome";
import { getAccessToken } from "../../api/api";
import { useGoogleAuth } from "../../components/app";
import Penalties from "../../components/marshal/Penalties";
import { OnChange, OnSelectChange } from "../../types/inputs";
import {
    AddTestRun,
    SyncTestRuns,
    GetEventsIfRequired,
    GetEntrantsIfRequired,
    GetTestRunsIfRequired,
} from "../../store/event/actions";
import {
    selectEntrants,
    selectRequiresSync,
    selectTestRuns,
    selectEvents,
} from "../../store/event/selectors";
import { keySeed } from "../../settings";
import ExistingCount from "../../components/marshal/ExistingCount";
import { findIfLoaded } from "../../types/loadingState";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import Breadcrumbs from "../../components/shared/Breadcrumbs";
import SyncButton from "../../components/marshal/SyncButton";
import { selectClubs } from "../../store/clubs/selectors";
import { GetClubsIfRequired } from "../../store/clubs/actions";

const getNewEditableTest = (ordinal: number): EditableTestRun => ({
    testRunId: uid.uuid(),
    ordinal: ordinal,
    timeInMS: undefined,
    penalties: [],
    entrantId: undefined,
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
    const dispatch = useDispatch();
    const entrants = useSelector(selectEntrants);
    const testRuns = useSelector(selectTestRuns);
    const requiresSync = useSelector(selectRequiresSync);

    const currentEvent = findIfLoaded(
        useSelector(selectEvents),
        (a) => a.eventId === eventId
    );
    const currentClub = findIfLoaded(
        useSelector(selectClubs),
        (a) => a.clubId === currentEvent?.clubId
    );

    const [editing, setEditing] = useState<EditableTestRun>(
        getNewEditableTest(ordinal)
    );
    const auth = useGoogleAuth();
    useEffect(() => {
        const token = getAccessToken(auth);
        dispatch(GetEntrantsIfRequired(eventId, token));
        dispatch(GetTestRunsIfRequired(eventId, ordinal, token));
    }, [auth, dispatch, eventId, ordinal]);
    useEffect(() => {
        dispatch(GetEventsIfRequired());
    }, [dispatch]);
    useEffect(() => {
        dispatch(GetClubsIfRequired(getAccessToken(auth)));
    }, [auth, dispatch]);

    const increase = (penaltyType: PenaltyType) => {
        setEditing((a) => {
            const found = a.penalties.find(
                (penalty) => penalty.penaltyType === penaltyType
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
                              }
                    ),
            };
        });
    };
    const decrease = (penaltyType: PenaltyType) => {
        setEditing((a) => {
            const found = a.penalties.find(
                (penalty) => penalty.penaltyType === penaltyType
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
                            : []
                    ),
            };
        });
    };
    const sync = useCallback(
        () => dispatch(SyncTestRuns(eventId, ordinal, getAccessToken(auth))),
        [auth, dispatch, eventId, ordinal]
    );
    const add = useCallback(() => {
        if (editing.ordinal !== undefined && editing.timeInMS !== undefined) {
            dispatch(
                AddTestRun(
                    {
                        ...editing,
                        created: newValidDate(),
                        eventId: eventId,
                    } as TestRunTemp,
                    getAccessToken(auth)
                )
            );
            setEditing(getNewEditableTest(ordinal));
        }
    }, [auth, dispatch, editing, eventId, ordinal]);
    const clearInputs = () => {
        setEditing((a) => getNewEditableTest(a.ordinal)); // todo
    };
    return (
        <div>
            <Breadcrumbs
                club={currentClub}
                event={currentEvent}
                test={ordinal}
            />
            <Heading>Marshal</Heading>
            <Field>
                <Label>Entrant</Label>
                <Select<number>
                    class="is-fullwidth"
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
                                {a.driverNumber}. {a.vehicle.registration} -{" "}
                                {a.givenName} {a.familyName}
                            </option>
                        )
                    )}
                </Select>
            </Field>
            <Field>
                <Label>Existing Count</Label>
                <ExistingCount
                    entrantId={editing.entrantId}
                    ordinal={ordinal}
                    currentEvent={currentEvent}
                    testRuns={testRuns}
                />
            </Field>
            <Field>
                <Label>Time (Secs)</Label>
                <Input
                    type="number"
                    min="0"
                    step="0.01"
                    value={
                        editing.timeInMS === undefined
                            ? ""
                            : (editing.timeInMS / 1000).toFixed(2)
                    }
                    onChange={(e: OnChange) =>
                        setEditing((a) => ({
                            ...a,
                            timeInMS:
                                Number.parseFloat(
                                    Number.parseFloat(e.target.value).toFixed(2)
                                ) * 1000,
                        }))
                    }
                />
            </Field>
            <Field>
                <Penalties
                    penalties={editing.penalties}
                    increase={increase}
                    decrease={decrease}
                />
            </Field>
            <Button.Group>
                <Button color="info" onClick={clearInputs}>
                    Clear
                </Button>
                <Button color="primary" onClick={add}>
                    Add
                </Button>
                <SyncButton unSyncedCount={requiresSync} sync={sync} />
            </Button.Group>
        </div>
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
