import { FunctionalComponent, h, Fragment } from "preact";
import { Loader, Notification, Tag } from "react-bulma-components";

import { LoadingState } from "../../types/loadingState";

const ifSome = <T, TT>(
  arr: LoadingState<readonly T[], TT>,
  getKey: (t: T) => string | number,
  IfIs: FunctionalComponent<T>,
  filter: (t: T) => boolean = (_: T) => true,
) => {
  switch (arr.tag) {
    case "Loaded": {
      if (arr.value.length) {
        return (
          <Fragment>
            {arr.value.filter(filter).map((a) => (
              <IfIs key={getKey(a)} {...a} />
            ))}
          </Fragment>
        );
      } else {
        return (
          <Notification color="light" class="has-text-centered">
            <Tag color="info" size="medium">
              No Data
            </Tag>
          </Notification>
        );
      }
    }
    case "Loading":
      return (
        <div class="has-text-centered p-4">
          <Loader size="medium" />
          <p class="mt-2 has-text-grey">Loading...</p>
        </div>
      );
    case "Error":
      return (
        <Notification color="danger">
          <strong>Error:</strong> {arr.value}
        </Notification>
      );
    case "Idle":
      return (
        <Notification color="light" class="has-text-centered">
          <Tag color="grey" size="medium">
            Idle
          </Tag>
        </Notification>
      );
  }
};

export default ifSome;
