import { FunctionalComponent, h } from "preact";
import { Hero, Box, Tag, Button, Heading } from "react-bulma-components";
import { useEffect } from "preact/hooks";
import { useSelector } from "react-redux";
import { formatDateIso, newValidDate } from "ts-date";
import { Link } from "preact-router";
import preval from "preval.macro";

import { GetEventsIfRequired } from "../../store/event/actions";
import { get10LatestEvents, selectEvents } from "../../store/event/selectors";
import ifSome from "../../components/shared/ifSome";
import { ifLoaded } from "../../types/loadingState";
import { useThunkDispatch } from "../../store";

const buildDate = preval`module.exports = new Date().toISOString();` as string;

const EventCard: FunctionalComponent<{
  readonly eventId: number;
  readonly location: string;
  readonly startTime: Date;
  readonly isToday?: boolean;
}> = ({ eventId, location, startTime, isToday }) => (
  <Box key={eventId}>
    <div
      style={{
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
      }}
    >
      <div>
        {isToday ? (
          <Tag color="warning" size="small">
            Today
          </Tag>
        ) : null}
        <Heading size={5} style={{ marginBottom: "0.25rem" }}>
          {location}
        </Heading>
        <p class="has-text-grey">
          {startTime.toLocaleDateString()} at{" "}
          {startTime.toLocaleTimeString([], {
            hour: "2-digit",
            minute: "2-digit",
          })}
        </p>
      </div>
      <Button color="link" renderAs={Link} href={`/event/${eventId}`}>
        View Event
      </Button>
    </div>
  </Box>
);

const Home: FunctionalComponent = () => {
  const thunkDispatch = useThunkDispatch();
  useEffect(() => {
    void thunkDispatch(GetEventsIfRequired());
  }, [thunkDispatch]);
  const events = useSelector(selectEvents);
  const tenLatest = ifLoaded(events, (a) => get10LatestEvents(a));
  const today = ifLoaded(events, (a) =>
    a.filter(
      (event) =>
        formatDateIso(event.startTime) === formatDateIso(newValidDate()),
    ),
  );
  return (
    <div>
      <Hero color="primary" size="medium">
        <Hero.Body>
          <Heading class="has-text-white" spaced>
            AutoTest
          </Heading>
          <Heading subtitle class="has-text-white-ter">
            Manage your motorsport events, entrants, and results
          </Heading>
        </Hero.Body>
      </Hero>
      <div class="section">
        <Heading size={4} spaced>
          Today&apos;s Events
        </Heading>
        {ifSome(
          today,
          (a) => a.eventId,
          (event) => (
            <EventCard
              eventId={event.eventId}
              location={event.location}
              startTime={event.startTime}
              isToday
            />
          ),
        )}
        <Heading size={4} spaced>
          Newest Events
        </Heading>
        {ifSome(
          tenLatest,
          (a) => a.eventId,
          (event) => (
            <EventCard
              eventId={event.eventId}
              location={event.location}
              startTime={event.startTime}
            />
          ),
        )}
        <p class="has-text-grey-light is-size-7 mt-5">
          Build Date: {buildDate}
        </p>
      </div>
    </div>
  );
};

export default Home;
