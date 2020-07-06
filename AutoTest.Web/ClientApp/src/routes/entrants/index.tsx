import { FunctionalComponent, h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Title, Button } from "rbx";
import UUID from "uuid-int";
import { DeepPartial } from "tsdef";

import { addEntrant } from "../../api/entrants";
import { Entrant } from "../../types/models";
import { requiresLoading } from "../../types/loadingState";
import { useGoogleAuth } from "../../components/app";
import { getAccessToken } from "../../api/api";
import List from "../../components/entrants/List";
import EntrantsModal from "../../components/entrants/Modal";
import { GetEntrants } from "../../store/event/actions";
import { useDispatch, useSelector } from "react-redux";
import { selectEntrants } from "../../store/event/selectors";

interface Props {
    eventId: string;
}
const uid = UUID(Number.parseInt(process.env.PREACT_APP_KEY_SEED as string));

const Events: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
    const eventIdNum = Number.parseInt(eventId);
    const entrants = useSelector(selectEntrants);
    const [editingEntrant, setEditingEntrant] = useState<Entrant | undefined>(
        undefined
    );
    const auth = useGoogleAuth();
    const save = async () => {
        if (editingEntrant) {
            await addEntrant(editingEntrant, getAccessToken(auth));
            setEditingEntrant(undefined);
            dispatch(GetEntrants(eventIdNum, getAccessToken(auth))); // might not need this
        }
    };
    const dispatch = useDispatch();
    useEffect(() => {
        if (requiresLoading(entrants)) {
            dispatch(GetEntrants(eventIdNum, getAccessToken(auth)));
        }
    }, [eventIdNum, dispatch, auth, entrants]);

    return (
        <div>
            <Title>Entrants</Title>
            <List entrants={entrants} setEditingEntrant={setEditingEntrant} />
            <Button
                onClick={() =>
                    setEditingEntrant({
                        entrantId: uid.uuid(),
                        eventId: eventIdNum,
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
