import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Select, Label, Field, Input, Button, List } from "rbx";
import UUID from "uuid-int";
import { FaPlus, FaMinus } from "react-icons/fa";

import {
    LoadingState,
    Entrant,
    TestRun,
    EditableTestRun,
    PenaltyType,
} from "../../types/models";
import ifSome from "../../components/shared/isSome";
import { getEntrants } from "../../api/entrants";
import { getAccessToken } from "../../api/api";
import { useGoogleAuth } from "../../components/app";
import { addTestRun } from "../../api/testRuns";

interface Props {
    eventId: number;
    testId: number;
}

const uid = UUID(Number.parseInt(process.env.PREACT_APP_KEY_SEED as string));

const Marshal: FunctionalComponent<Readonly<Props>> = ({ eventId, testId }) => {
    const [entrants, setEntrants] = useState<LoadingState<readonly Entrant[]>>({
        tag: "Loading",
    });
    // const [testRuns, setTestRuns] = useState<LoadingState<readonly TestRun[]>>({
    //     tag: "Loading",
    // });

    const getNewEditableTest = () => ({
        testRunId: uid.uuid(),
        testId: testId,
        timeInMS: undefined,
        penalties: [],
    });
    const [editing, setEditing] = useState<EditableTestRun>(
        getNewEditableTest()
    );
    const auth = useGoogleAuth();
    useEffect(() => {
        const fetchData = async () => {
            const entrants = await getEntrants(eventId, getAccessToken(auth));
            //const testRuns = await getTestRuns(eventId, getAccessToken(auth));
            setEntrants(entrants);
            //setTestRuns(testRuns);
        };
        void fetchData();
    }, [auth, eventId]);

    const getCount = (penaltyType: PenaltyType) => {
        const found = editing.penalties.find(
            (a) => a.penaltyType === penaltyType
        );
        if (found === undefined) {
            return 0;
        } else {
            return found.instanceCount;
        }
    };

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
                        onChange={(event: { target: { value: number } }) =>
                            setEditing((e) => ({
                                ...e,
                                entrantId: event.target.value,
                            }))
                        }
                    >
                        {ifSome(entrants, (a) => (
                            <Select.Option>
                                {a.registration} - {a.givenName} {a.familyName}
                            </Select.Option>
                        ))}
                    </Select>
                </Select.Container>
            </Field>
            <Field>
                <Label>Time</Label>
                <Input type="number" />
            </Field>
            <Field>
                <Label>Penalties</Label>
                <List>
                    {Object.keys(PenaltyType)
                        .filter((key) => !isNaN(Number(key)))
                        .map((key) => (
                            <List.Item key={key}>
                                {PenaltyType[Number(key)]}{" "}
                                {getCount(Number(key) as PenaltyType)}
                                <Button
                                    onClick={() =>
                                        increase(Number(key) as PenaltyType)
                                    }
                                >
                                    <FaPlus />
                                </Button>
                                <Button
                                    onClick={() =>
                                        decrease(Number(key) as PenaltyType)
                                    }
                                >
                                    <FaMinus />
                                </Button>
                            </List.Item>
                        ))}
                </List>
                <Button
                    onClick={() => {
                        if (
                            editing.testId !== undefined &&
                            editing.timeInMS !== undefined
                        ) {
                            void addTestRun(
                                { ...editing } as TestRun,
                                getAccessToken(auth)
                            );
                            setEditing(getNewEditableTest());
                        }
                    }}
                >
                    Add
                </Button>
            </Field>
            <Button>Save</Button>
        </div>
    );
};

export default Marshal;
