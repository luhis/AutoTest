import { type FunctionComponent, h } from "preact";
import { Button } from "react-bulma-components";

const SyncButton: FunctionComponent<{
  readonly unSyncedCount: number;
  readonly sync: () => Promise<void>;
}> = ({ unSyncedCount, sync }) =>
  unSyncedCount > 0 ? (
    <Button type="button" onClick={sync} color="danger">
      Sync ({unSyncedCount})
    </Button>
  ) : null;

export default SyncButton;
