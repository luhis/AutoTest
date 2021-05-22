import { FunctionalComponent, h } from "preact";
import { Button } from "react-bulma-components";

const SyncButton: FunctionalComponent<{
    readonly unSyncedCount: number;
    readonly sync: () => void;
}> = ({ unSyncedCount, sync }) =>
    unSyncedCount > 0 ? (
        <Button type="button" onClick={sync} color="danger">
            Sync ({unSyncedCount})
        </Button>
    ) : null;

export default SyncButton;
