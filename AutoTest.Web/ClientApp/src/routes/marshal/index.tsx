import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Select, Label, Field, Input, Button } from "rbx";
import UUID from "uuid-int";

import {
    LoadingState,
    Test,
    Entrant,
    TestRun,
    EditableTestRun,
} from "../../types/models";
import { getTests } from "../../api/tests";
import ifSome from "../../components/shared/isSome";
import { getEntrants } from "../../api/entrants";
import { getAccessToken } from "../../api/api";
import { useGoogleAuth } from "../../components/app";
import { addTestRun } from "../../api/testRuns";

interface Props {
    eventId: number;
}

const uid = UUID(42);

const Marshal: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const [tests, setTests] = useState<LoadingState<readonly Test[]>>({
        tag: "Loading",
    });
    const [entrants, setEntrants] = useState<LoadingState<readonly Entrant[]>>({
        tag: "Loading",
    });
    // const [testRuns, setTestRuns] = useState<LoadingState<readonly TestRun[]>>({
    //     tag: "Loading",
    // });
    const [editing, setEditing] = useState<EditableTestRun>({
        testRunId: uid.uuid(),
        testId: undefined,
        timeInMS: undefined,
    });
    const auth = useGoogleAuth();
    useEffect(() => {
        const fetchData = async () => {
            const tests = await getTests(eventId, getAccessToken(auth));
            const entrants = await getEntrants(eventId, getAccessToken(auth));
            //const testRuns = await getTestRuns(eventId, getAccessToken(auth));
            setTests(tests);
            setEntrants(entrants);
            //setTestRuns(testRuns);
        };
        void fetchData();
    }, [auth, eventId]);
    return (
        <div>
            <Title>Marshal</Title>
            <Field>
                <Label>Test</Label>
                <Select.Container>
                    <Select
                        onChange={(event: { target: { value: number } }) =>
                            setEditing((e) => ({
                                ...e,
                                testId: event.target.value,
                            }))
                        }
                    >
                        {ifSome(tests, (a) => (
                            <Select.Option
                                selected={a.testId === editing.testId}
                            >
                                {a.ordinal}
                            </Select.Option>
                        ))}
                    </Select>
                </Select.Container>
            </Field>
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
            </Field>
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
                    }
                }}
            >
                Add
            </Button>
        </div>
    );
};

export default Marshal;
