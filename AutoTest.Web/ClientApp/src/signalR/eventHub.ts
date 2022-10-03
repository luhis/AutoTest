import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { useMemo } from "preact/hooks";

const getBaseConn = () =>
    new HubConnectionBuilder()
        .withUrl("/resultsHub")
        .withAutomaticReconnect()
        .configureLogging(LogLevel.Error);

export const useConnection = () => {
    return useMemo(
        () =>
            typeof window !== "undefined" ? getBaseConn().build() : undefined,
        []
    );
};

export const ListenToEvent = "ListenToEvent";
export const LeaveEvent = "LeaveEvent";
export const NewNotification = "NewNotification";
export const NewResults = "NewResults";
export const NewTestRun = "NewTestRun";
