declare module "react-redux" {
    import { FunctionComponent } from "preact";
    export const useDispatch: () => (a: any) => void;
    export const Provider: FunctionComponent<{ store: any }>;
    export const useSelector: <TT, T>(f: (_: TT) => T) => T;
}
