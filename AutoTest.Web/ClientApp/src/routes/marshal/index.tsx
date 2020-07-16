import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Select, Label, Field, Input, Button } from "rbx";
import UUID from "uuid-int";
import { useSelector, useDispatch } from "react-redux";
import { fromDateOrThrow } from "ts-date";

import { EditableTestRun, PenaltyType, TestRunTemp } from "../../types/models";
import ifSome from "../../components/shared/ifSome";
import { getAccessToken } from "../../api/api";
import { useGoogleAuth } from "../../components/app";
import Penalties from "../../components/marshal/Penalties";
import { OnChange, OnSelectChange } from "../../types/inputs";
import {
    GetEntrants,
    AddTestRun,
    SyncTestRuns,
    GetEventsIfRequired,
} from "../../store/event/actions";
import {
    selectEntrants,
    selectRequiresSync,
    selectTestRuns,
    selectEvents,
} from "../../store/event/selectors";
import { keySeed } from "../../settings";
import ExistingCount from "../../components/marshal/ExistingCount";

const getNewEditableTest = (testId: number): EditableTestRun => ({
    testRunId: uid.uuid(),
    testId: testId,
    timeInMS: undefined,
    penalties: [],
    entrantId: undefined,
});
interface Props {
    eventId: string;
    testId: string;
}

const uid = UUID(keySeed);

const SyncButton: FunctionalComponent<Readonly<{
    requiresSync: boolean;
    sync: () => void;
}>> = ({ requiresSync, sync }) =>
    requiresSync ? <Button onClick={sync}>Sync</Button> : null;

const Marshal: FunctionalComponent<Readonly<Props>> = ({ eventId, testId }) => {
    const dispatch = useDispatch();
    const entrants = useSelector(selectEntrants);
    const testRuns = useSelector(selectTestRuns);
    const requiresSync = useSelector(selectRequiresSync);
    const events = useSelector(selectEvents);
    const testIdNum = Number.parseInt(testId);
    const eventIdNum = Number.parseInt(eventId);

    const currentEvent =
        events.tag === "Loaded"
            ? events.value.find((a) => a.eventId === eventIdNum)
            : undefined;

    const [editing, setEditing] = useState<EditableTestRun>(
        getNewEditableTest(testIdNum)
    );
    const auth = useGoogleAuth();
    useEffect(() => {
        const token = getAccessToken(auth);
        dispatch(GetEntrants(eventIdNum, token));
        dispatch(GetEventsIfRequired());
    }, [auth, dispatch, eventIdNum]);

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
    return (
        <div>
            <Title>Marshal</Title>
            <Field>
                <Label>Entrant</Label>
                <Select.Container>
                    <Select
                        onChange={(event: OnSelectChange) =>
                            setEditing((e) => ({
                                ...e,
                                entrantId: Number.parseInt(event.target.value),
                            }))
                        }
                        value={editing.entrantId}
                    >
                        <Select.Option value={undefined}>
                            - Please Select -
                        </Select.Option>
                        {ifSome(
                            entrants,
                            (a) => a.entrantId,
                            (a) => (
                                <Select.Option value={a.entrantId}>
                                    {a.driverNumber}. {a.vehicle.registration} -{" "}
                                    {a.givenName} {a.familyName}
                                </Select.Option>
                            )
                        )}
                    </Select>
                </Select.Container>
            </Field>
            <Field>
                <Label>Existing Count</Label>
                <ExistingCount
                    entrantId={editing.entrantId}
                    testId={testIdNum}
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
                <Button
                    onClick={() => {
                        if (
                            editing.testId !== undefined &&
                            editing.timeInMS !== undefined
                        ) {
                            dispatch(
                                AddTestRun(
                                    {
                                        ...editing,
                                        created: fromDateOrThrow(new Date()),
                                        eventId: eventIdNum,
                                    } as TestRunTemp,
                                    getAccessToken(auth)
                                )
                            );
                            setEditing(
                                getNewEditableTest(Number.parseInt(testId))
                            );
                        }
                    }}
                >
                    Add
                </Button>
                <SyncButton
                    requiresSync={requiresSync}
                    sync={() => dispatch(SyncTestRuns(getAccessToken(auth)))}
                />
            </Button.Group>
        </div>
    );
};

export default Marshal;
