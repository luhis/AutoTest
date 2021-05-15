import { FunctionalComponent, h } from "preact";
import { Columns, Button, Form } from "react-bulma-components";
const { Field } = Form;

import ifSome from "../shared/ifSome";
import { Marshal } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import DeleteButton from "../shared/DeleteButton";

interface Props {
    readonly marshals: LoadingState<readonly Marshal[], number>;
    readonly setEditingEntrant: (entrant: Marshal) => void;
    readonly deleteEntrant: (entrant: Marshal) => void;
}

const List: FunctionalComponent<Props> = ({
    marshals,
    setEditingEntrant,
    deleteEntrant,
}) =>
    ifSome(
        marshals,
        (marshal) => marshal.marshalId,
        (marshal) => (
            <Columns>
                <Columns.Column>{`${marshal.givenName} ${marshal.familyName}`}</Columns.Column>
                <Columns.Column>
                    <Field kind="group">
                        <Button onClick={() => setEditingEntrant(marshal)}>
                            Edit
                        </Button>
                        <DeleteButton deleteFunc={() => deleteEntrant(marshal)}>
                            Delete
                        </DeleteButton>
                    </Field>
                </Columns.Column>
            </Columns>
        )
    );

export default List;
