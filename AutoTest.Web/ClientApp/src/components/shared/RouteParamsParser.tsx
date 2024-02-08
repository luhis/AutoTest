/* eslint-disable @typescript-eslint/ban-types */
import { h, FunctionComponent } from "preact";
import { useMemo } from "preact/hooks";

import { Override } from "../../types/models";

export default <TOuterProps extends object, TInnerProps extends object>(
    paramsMapper: (_: TOuterProps) => TInnerProps,
  ) =>
  (Component: FunctionComponent<TInnerProps>) => {
    const RouteParamsParserWrapper = (
      props: Override<TInnerProps, TOuterProps>,
    ) => {
      const finalProps = useMemo(() => paramsMapper(props), [props]);
      return <Component {...finalProps} />;
    };

    return RouteParamsParserWrapper;
  };
