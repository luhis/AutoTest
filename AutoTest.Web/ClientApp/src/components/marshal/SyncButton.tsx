import { FunctionalComponent, h } from "preact";
import { Button, Tag } from "react-bulma-components";

const SyncButton: FunctionalComponent<{
    readonly unSyncedCount: number;
    readonly sync: () => void;
}> = ({ unSyncedCount, sync }) =>
    unSyncedCount > 0 ? (
        <Button onClick={sync}>
            Sync <Tag color="danger">({unSyncedCount})</Tag>
        </Button>
    ) : null;

export default SyncButton;
