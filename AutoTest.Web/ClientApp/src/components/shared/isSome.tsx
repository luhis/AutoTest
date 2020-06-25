import { FunctionalComponent, h } from "preact";
import { LoadingState } from "../../types/models";

const ifSome = <T,>(
    arr: LoadingState<readonly T[]>,
    IfIs: FunctionalComponent<T>
) => {
    switch (arr.tag) {
        case "Loaded": {
            if (arr.value.length) {
                // bad idea maybe, maybe inject in a keyMap function
                return arr.value.map((a, i) => <IfIs key={i} {...a} />);
            } else {
                return <span>No Data</span>;
            }
        }
        case "Loading":
            return <span>Loading...</span>;
        case "Error":
            return <span>Error: {arr.value}</span>;
    }
};

export default ifSome;
