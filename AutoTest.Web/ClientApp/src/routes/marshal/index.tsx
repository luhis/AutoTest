import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import {
    Title,
    Select,
    Label,
    Field,
    Input,
    Button,
    Breadcrumb,
    Tag,
} from "rbx";
import UUID from "uuid-int";
import { useSelector, useDispatch } from "react-redux";
import { newValidDate } from "ts-date";

import { EditableTestRun, PenaltyType, TestRunTemp } from "../../types/models";
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
    GetClubsIfRequired,
} from "../../store/event/actions";
import {
    selectEntrants,
    selectRequiresSync,
    selectTestRuns,
    selectEvents,
    selectClubs,
} from "../../store/event/selectors";
import { keySeed } from "../../settings";
import ExistingCount from "../../components/marshal/ExistingCount";
import { findIfLoaded } from "../../types/loadingState";

const getNewEditableTest = (ordinal: number): EditableTestRun => ({
    testRunId: uid.uuid(),
    ordinal: ordinal,
    timeInMS: undefined,
    penalties: [],
    entrantId: undefined,
});
interface Props {
    eventId: string;
    ordinal: string;
}

const uid = UUID(keySeed);

const SyncButton: FunctionalComponent<Readonly<{
    unSyncedCount: number;
    sync: () => void;
}>> = ({ unSyncedCount, sync }) =>
    unSyncedCount > 0 ? (
        <Button onClick={sync}>
            Sync <Tag color="danger">({unSyncedCount})</Tag>
        </Button>
    ) : null;

const Marshal: FunctionalComponent<Readonly<Props>> = ({
    eventId,
    ordinal,
}) => {
    const dispatch = useDispatch();
    const entrants = useSelector(selectEntrants);
    const testRuns = useSelector(selectTestRuns);
    const requiresSync = useSelector(selectRequiresSync);
    const ordinalNum = Number.parseInt(ordinal);
    const eventIdNum = Number.parseInt(eventId);

    const currentEvent = findIfLoaded(
        useSelector(selectEvents),
        (a) => a.eventId === eventIdNum
    );
    const currentClub = findIfLoaded(
        useSelector(selectClubs),
        (a) => a.clubId === currentEvent?.clubId
    );

    const [editing, setEditing] = useState<EditableTestRun>(
        getNewEditableTest(ordinalNum)
    );
    const auth = useGoogleAuth();
    useEffect(() => {
        const token = getAccessToken(auth);
        dispatch(GetEntrantsIfRequired(eventIdNum, token));
        dispatch(GetEventsIfRequired());
        dispatch(GetClubsIfRequired(token));
        dispatch(GetTestRunsIfRequired(eventIdNum, token));
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
            <Breadcrumb>
                <Breadcrumb.Item
                    href={`/events?clubId=${currentClub?.clubId || 0}`}
                >
                    {currentClub?.clubName}
                </Breadcrumb.Item>
                <Breadcrumb.Item href={`/tests/${currentEvent?.eventId || 0}`}>
                    {currentEvent?.location}
                </Breadcrumb.Item>
                <Breadcrumb.Item>Test No. {ordinalNum + 1}</Breadcrumb.Item>
            </Breadcrumb>
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
                        <Select.Option value={-1}>
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
                    ordinal={ordinalNum}
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
                            editing.ordinal !== undefined &&
                            editing.timeInMS !== undefined
                        ) {
                            dispatch(
                                AddTestRun(
                                    {
                                        ...editing,
                                        created: newValidDate(),
                                        eventId: eventIdNum,
                                    } as TestRunTemp,
                                    getAccessToken(auth)
                                )
                            );
                            setEditing(getNewEditableTest(ordinalNum));
                        }
                    }}
                >
                    Add
                </Button>
                <SyncButton
                    unSyncedCount={requiresSync}
                    sync={() => dispatch(SyncTestRuns(getAccessToken(auth)))}
                />
            </Button.Group>
        </div>
    );
};

export default Marshal;
