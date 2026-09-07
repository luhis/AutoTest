import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { useMemo } from "preact/hooks";

const getBaseConnection = (eventId: number) =>
  new HubConnectionBuilder()
    .withUrl(`/resultsHub/${eventId}`)
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Error);

export const useConnection = (eventId: number | undefined) => {
  return useMemo(
    () =>
      typeof window !== "undefined" && eventId !== undefined
        ? getBaseConnection(eventId).build()
        : undefined,
    [eventId],
  );
};

export const NewNotification = "NewNotification";
export const NewResults = "NewResults";
export const NewTestRun = "NewTestRun";
