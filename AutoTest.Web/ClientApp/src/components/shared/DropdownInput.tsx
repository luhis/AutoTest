import { uniqueId } from "@s-libs/micro-dash";
import { h, FunctionComponent } from "preact";
import { Form } from "react-bulma-components";
import { OnChange } from "src/types/inputs";
const { Input } = Form;

const DropdownInput: FunctionComponent<{
    readonly value: string;
    readonly options: readonly string[];
    readonly setValue: (s: string) => void;
    readonly required: boolean;
}> = ({ value, options, required, setValue }) => {
    const id = uniqueId("DropdownInput-");
    return (
        <Input
            required={required}
            list={id}
            value={value}
            onChange={(e: OnChange) => setValue(e.target.value)}
        >
            <datalist id={id}>
                {options.map((a) => (
                    <option key={a} value={a} onClick={() => setValue(a)} />
                ))}
            </datalist>
        </Input>
    );
};

export default DropdownInput;
