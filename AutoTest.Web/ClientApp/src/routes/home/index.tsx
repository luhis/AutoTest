import { FunctionalComponent, h } from "preact";
import { Columns, Heading } from "react-bulma-components";
import { useEffect } from "preact/hooks";
import { useDispatch, useSelector } from "react-redux";
import { formatDateIso, newValidDate } from "ts-date";
import { Link } from "preact-router";

import { GetEventsIfRequired } from "../../store/event/actions";
import { selectEvents } from "../../store/event/selectors";
import ifSome from "../../components/shared/ifSome";
import { ifLoaded } from "../../types/loadingState";

const Home: FunctionalComponent = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(GetEventsIfRequired());
  });
  const events = useSelector(selectEvents);
  //const toSorted = ifLoaded(events, (a) => sortBy(a, (a) => a.createdDate));
  const today = ifLoaded(events, (a) =>
    a.filter(
      (event) =>
        formatDateIso(event.startTime) === formatDateIso(newValidDate()),
    ),
  );
  return (
    <div>
      <Heading>Home</Heading>
      <p>This is the Mangaji AutoTest app.</p>
      <Heading>Today&apos;s Events</Heading>
      {ifSome(
        today,
        (a) => a.eventId,
        (event) => (
          <Columns>
            <Columns.Column>
              {event.startTime.toLocaleDateString()} {event.location}
            </Columns.Column>
            <Columns.Column>
              <Link href={`/event/${event.eventId}`}>View</Link>
            </Columns.Column>
          </Columns>
        ),
      )}
      <Heading>New Events</Heading>
    </div>
  );
};

export default Home;
