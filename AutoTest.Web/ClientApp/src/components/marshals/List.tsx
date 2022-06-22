import { FunctionalComponent, h } from "preact";
import { Columns, Button, Form } from "react-bulma-components";
const { Field, Control } = Form;

import ifSome from "../shared/ifSome";
import { PublicMarshal } from "../../types/models";
import { LoadingState } from "../../types/loadingState";
import DeleteButton from "../shared/DeleteButton";

interface Props {
    readonly marshals: LoadingState<readonly PublicMarshal[], number>;
    readonly setEditingMarshal: (marshal: PublicMarshal) => Promise<void>;
    readonly deleteMarshal: (marshal: PublicMarshal) => Promise<void>;
    readonly canEditMarshal: (marshalId: number) => boolean;
}

const List: FunctionalComponent<Props> = ({
    marshals,
    setEditingMarshal,
    deleteMarshal,
    canEditMarshal,
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
                            <Button
                                disabled={!canEditMarshal(marshal.marshalId)}
                                onClick={() => setEditingMarshal(marshal)}
                            >
                                Edit
                            </Button>
                        </Control>

                        <Control>
                            <DeleteButton
                                deleteFunc={() => deleteMarshal(marshal)}
                                disabled={!canEditMarshal(marshal.marshalId)}
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
