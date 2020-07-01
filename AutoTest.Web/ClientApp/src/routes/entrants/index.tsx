import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Button } from "rbx";
import UUID from "uuid-int";
import { DeepPartial } from "tsdef";

import { getEntrants, addEntrant } from "../../api/entrants";
import { LoadingState, Entrant } from "../../types/models";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import List from "../../components/entrants/List";
import EntrantsModal from "../../components/entrants/Modal";

interface Props {
    eventId: number;
}
const uid = UUID(Number.parseInt(process.env.PREACT_APP_KEY_SEED as string));

const Events: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const [entrants, setEntrants] = useState<LoadingState<readonly Entrant[]>>({
        tag: "Loading",
    });
    const [editingEntrant, setEditingEntrant] = useState<Entrant | undefined>(
        undefined
    );
    const auth = useGoogleAuth();
    const save = async () => {
        if (editingEntrant) {
            await addEntrant(editingEntrant, getAccessToken(auth));
            setEditingEntrant(undefined);
            setEntrants(await getEntrants(eventId, getAccessToken(auth)));
        }
    };
    useEffect(() => {
        const fetchData = async () => {
            setEntrants(await getEntrants(eventId, getAccessToken(auth)));
        };
        void fetchData();
    }, [auth, eventId]);

    return (
        <div>
            <Title>Entrants</Title>
            <List entrants={entrants} setEditingEntrant={setEditingEntrant} />
            <Button
                onClick={() =>
                    setEditingEntrant({
                        entrantId: uid.uuid(),
                        eventId: eventId,
                        class: "",
                        givenName: "",
                        familyName: "",
                        vehicle: {
                            make: "",
                            model: "",
                            year: 0,
                            displacement: 0,
                            registration: "",
                        },
                    })
                }
            >
                Add Entrant
            </Button>
            {editingEntrant ? (
                <EntrantsModal
                    entrant={editingEntrant}
                    setField={(a: DeepPartial<Entrant>) =>
                        setEditingEntrant((b) => ({ ...b, ...a } as Entrant))
                    }
                    cancel={() => setEditingEntrant(undefined)}
                    save={save}
                />
            ) : null}
        </div>
    );
};

export default Events;
