import { FunctionalComponent, h } from "preact";
import { Columns, Button, Form } from "react-bulma-components";
const { Field, Control } = Form;

import ifSome from "../shared/ifSome";
import { Marshal } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import DeleteButton from "../shared/DeleteButton";

interface Props {
    readonly marshals: LoadingState<readonly Marshal[], number>;
    readonly setEditingMarshal: (entrant: Marshal) => void;
    readonly deleteMarshal: (entrant: Marshal) => void;
}

const List: FunctionalComponent<Props> = ({
    marshals,
    setEditingMarshal,
    deleteMarshal,
}) =>
    ifSome(
        marshals,
        (marshal) => marshal.marshalId,
        (marshal) => (
            <Columns>
                <Columns.Column>{`${marshal.givenName} ${marshal.familyName}`}</Columns.Column>
                <Columns.Column>
                    <Field kind="group">
                        <Control>
                            <Button onClick={() => setEditingMarshal(marshal)}>
                                Edit
                            </Button>
                        </Control>

                        <Control>
                            <DeleteButton
                                deleteFunc={() => deleteMarshal(marshal)}
                            >
                                Delete
                            </DeleteButton>
                        </Control>
                    </Field>
                </Columns.Column>
            </Columns>
        )
    );

export default List;
