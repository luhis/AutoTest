import { FunctionComponent, h } from "preact";
import { Event } from "src/types/models";

const EventTitle: FunctionComponent<{
    currentEvent: Event | undefined;
}> = ({ currentEvent }) =>
    currentEvent ? (
        <span>
            {currentEvent.location}{" "}
            {currentEvent.startTime.toLocaleDateString()}
        </span>
    ) : null;

export default EventTitle;
