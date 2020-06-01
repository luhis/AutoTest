/* eslint-disable @typescript-eslint/no-unsafe-return */
/* eslint-disable @typescript-eslint/explicit-module-boundary-types */
// The official types conflict with PReact
declare module "enzyme" {
    export const render = (f: FunctionalComponent<unknown>) => Element;
    export const shallow = (f: FunctionalComponent<unknown>) => Element;
    export const configure = (f: unknown) => undefined;
}
