import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Select, Label, Field, Input, Button } from "rbx";
import UUID from "uuid-int";

import {
    LoadingState,
    Entrant,
    TestRun,
    EditableTestRun,
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
    const [editing, setEditing] = useState<EditableTestRun>({
        testRunId: uid.uuid(),
        testId: testId,
        timeInMS: undefined,
        penalties: [],
    });
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
            </Field>
            <Button>Save</Button>
        </div>
    );
};

export default Marshal;
