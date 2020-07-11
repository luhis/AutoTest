import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Select, Label, Field, Input, Button } from "rbx";
import UUID from "uuid-int";
import { useSelector, useDispatch } from "react-redux";
import { fromDateOrThrow } from "ts-date";

import { TestRun, EditableTestRun, PenaltyType } from "../../types/models";
import ifSome from "../../components/shared/ifSome";
import { getAccessToken } from "../../api/api";
import { useGoogleAuth } from "../../components/app";
import Penalties from "../../components/marshal/Penalties";
import { OnChange, OnSelectChange } from "../../types/inputs";
import {
    GetEntrants,
    AddTestRun,
    SyncTestRuns,
} from "../../store/event/actions";
import {
    selectEntrants,
    selectRequiresSync,
} from "../../store/event/selectors";
import { AppState } from "src/store";

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

const uid = UUID(Number.parseInt(process.env.PREACT_APP_KEY_SEED as string));

const Marshal: FunctionalComponent<Readonly<Props>> = ({ eventId, testId }) => {
    const dispatch = useDispatch();
    const entrants = useSelector(selectEntrants);
    const testRuns = useSelector((a: AppState) => a.event.testRuns);
    const requiresSync = useSelector(selectRequiresSync);
    const testIdNum = Number.parseInt(testId);
    const eventIdNum = Number.parseInt(eventId);

    const [editing, setEditing] = useState<EditableTestRun>(
        getNewEditableTest(testIdNum)
    );
    const auth = useGoogleAuth();
    useEffect(() => {
        dispatch(GetEntrants(eventIdNum, getAccessToken(auth)));
    }, [auth, dispatch, eventIdNum]);

    const increase = (penaltyType: PenaltyType) => {
        setEditing((a) => {
            const found = a.penalties.find(
                (a) => a.penaltyType === penaltyType
            );
            return {
                ...a,
                penalties: a.penalties
                    .filter((a) => a.penaltyType !== penaltyType)
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
                (a) => a.penaltyType === penaltyType
            );
            return {
                ...a,
                penalties: a.penalties
                    .filter((a) => a.penaltyType !== penaltyType)
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
                                    {a.vehicle.registration} - {a.givenName}{" "}
                                    {a.familyName}
                                </Select.Option>
                            )
                        )}
                    </Select>
                </Select.Container>
            </Field>
            <Field>
                <Label>Existing Count</Label>
                <span>
                    {
                        testRuns.filter(
                            (a) =>
                                a.entrantId === editing.entrantId &&
                                a.testId === testIdNum
                        ).length
                    }
                </span>
            </Field>
            <Field>
                <Label>Time (Secs)</Label>
                <Input
                    type="number"
                    value={
                        editing.timeInMS === undefined
                            ? ""
                            : editing.timeInMS / 1000
                    }
                    onChange={(e: OnChange) =>
                        setEditing((a) => ({
                            ...a,
                            timeInMS: Number.parseFloat(e.target.value) * 1000,
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
                                    } as TestRun,
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
                {requiresSync ? (
                    <Button
                        onClick={() =>
                            dispatch(SyncTestRuns(getAccessToken(auth)))
                        }
                    >
                        Sync
                    </Button>
                ) : null}
            </Button.Group>
        </div>
    );
};

export default Marshal;
