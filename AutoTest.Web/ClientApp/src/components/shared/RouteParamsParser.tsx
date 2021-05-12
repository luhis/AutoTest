import { createElement, FunctionComponent } from "react";

import { Override } from "../../types/models";

// eslint-disable-next-line @typescript-eslint/ban-types
export default <TOuterProps extends object, TInnerProps extends object>(
        paramsMapper: (_: TOuterProps) => TInnerProps
    ) =>
    (component: FunctionComponent<TInnerProps>) => {
        const RouteParamsParserWrapper = (
            props: Override<TInnerProps, TOuterProps>
        ) => {
            const finalProps = paramsMapper(props);
            return createElement(component, finalProps);
        };

        return RouteParamsParserWrapper;
    };
