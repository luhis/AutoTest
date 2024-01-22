import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { useMemo } from "preact/hooks";

const getBaseConn = (accessToken: string) =>
  new HubConnectionBuilder()
    .withUrl("/authorisationHub", {
      accessTokenFactory: () => accessToken,
    })
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Error);

export const useConnection = (accessToken: string | undefined) => {
  return useMemo(
    () =>
      typeof window !== "undefined" && accessToken !== undefined
        ? getBaseConn(accessToken).build()
        : undefined,
    [accessToken],
  );
};

export const NewClubAdminTag = "NewClubAdmin";
export const RemoveClubAdminTag = "RemoveClubAdmin";
export const NewEventMarshalTag = "NewEventMarshal";
export const RemoveEventMarshalTag = "RemoveEventMarshal";
export const AddEditableMarshalTag = "AddEditableMarshal";
export const AddEditableEntrantTag = "AddEditableEntrant";
