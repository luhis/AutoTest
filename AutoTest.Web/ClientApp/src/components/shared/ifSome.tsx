import { FunctionalComponent, h, Fragment } from "preact";
import { LoadingState } from "../../types/loadingState";

const ifSome = <T,>(
    arr: LoadingState<readonly T[]>,
    getKey: (t: T) => string | number,
    IfIs: FunctionalComponent<T>
) => {
    switch (arr.tag) {
        case "Loaded": {
            if (arr.value.length) {
                // bad idea maybe, maybe inject in a keyMap function
                return (
                    <Fragment>
                        {arr.value.map((a) => (
                            <IfIs key={getKey(a)} {...a} />
                        ))}
                    </Fragment>
                );
            } else {
                return <span>No Data</span>;
            }
        }
        case "Loading":
            return <span>Loading...</span>;
        case "Error":
            return <span>Error: {arr.value}</span>;
        case "Idle":
            return <span>Idle</span>;
    }
};

export default ifSome;
