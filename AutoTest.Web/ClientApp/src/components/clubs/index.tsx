import { FunctionComponent, h } from "preact";
import { Title, Column, Button, Modal } from "rbx";
import { route } from "preact-router";
import UUID from "uuid-int";

import { LoadingState, Club } from "../../types/models";
import ifSome from "../shared/isSome";
import { StateUpdater } from "preact/hooks";

const uid = UUID(Number.parseInt(process.env.PREACT_APP_KEY_SEED as string));

interface Props {
    clubs: LoadingState<readonly Club[]>;
    editingClub: Club | undefined;
    save: () => Promise<void>;
    setEditingClub: StateUpdater<Club | undefined>;
}
const Clubs: FunctionComponent<Props> = ({
    clubs,
    editingClub,
    setEditingClub,
    save,
}) => (
    <div>
        <Title>Clubs</Title>
        {ifSome(clubs, (a) => (
            <Column.Group key={a.clubId}>
                <Column>
                    {a.clubName}
                    &nbsp;
                    {a.website !== "" ? <a href={a.website}>Homepage</a> : null}
                </Column>
                <Column>
                    <Button.Group>
                        <Button
                            onClick={() => route(`/events?clubId=${a.clubId}`)}
                        >
                            Events
                        </Button>
                        <Button onClick={() => setEditingClub(a)}>Edit</Button>
                    </Button.Group>
                </Column>
            </Column.Group>
        ))}
        <Button
            onClick={() =>
                setEditingClub({
                    clubId: uid.uuid(),
                    clubName: "",
                    website: "",
                    clubPaymentAddress: "",
                })
            }
        >
            Add Club
        </Button>
        {editingClub ? (
            <Modal
                club={editingClub}
                setField={(a: Partial<Club>) =>
                    setEditingClub((b) => ({ ...b, ...a } as Club))
                }
                cancel={() => setEditingClub(undefined)}
                save={save}
            />
        ) : null}
    </div>
);

export default Clubs;
