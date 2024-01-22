import { FunctionalComponent, h, Fragment } from "preact";
import { LoadingState } from "../../types/loadingState";
import { Loader } from "react-bulma-components";

const ifSome = <T, TT>(
  arr: LoadingState<readonly T[], TT>,
  getKey: (t: T) => string | number,
  IfIs: FunctionalComponent<T>,
  filter: (t: T) => boolean = (_: T) => true,
) => {
  switch (arr.tag) {
    case "Loaded": {
      if (arr.value.length) {
        // bad idea maybe, maybe inject in a keyMap function
        return (
          <Fragment>
            {arr.value.filter(filter).map((a) => (
              <IfIs key={getKey(a)} {...a} />
            ))}
          </Fragment>
        );
      } else {
        return <div>No Data</div>;
      }
    }
    case "Loading":
      return (
        <span>
          Loading... <Loader />
        </span>
      );
    case "Error":
      return <span>Error: {arr.value}</span>;
    case "Idle":
      return <span>Idle</span>;
  }
};

export default ifSome;
