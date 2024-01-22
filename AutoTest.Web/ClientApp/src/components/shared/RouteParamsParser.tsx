/* eslint-disable @typescript-eslint/ban-types */
import { h, FunctionComponent } from "preact";

import { Override } from "../../types/models";

export default <TOuterProps extends object, TInnerProps extends object>(
        paramsMapper: (_: TOuterProps) => TInnerProps,
    ) =>
    (Component: FunctionComponent<TInnerProps>) => {
        const RouteParamsParserWrapper = (
            props: Override<TInnerProps, TOuterProps>,
        ) => {
            const finalProps = paramsMapper(props);
            return <Component {...finalProps} />;
        };

        return RouteParamsParserWrapper;
    };
