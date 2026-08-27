import { FunctionalComponent, h } from "preact";
import { useEffect } from "preact/hooks";
import { route } from "preact-router";
import { Heading, Box, Button, Loader } from "react-bulma-components";
import { useSelector } from "react-redux";

import { getAccessToken } from "../../api/api";
import {
  GetEntrantsIfRequired,
  GetEventsIfRequired,
} from "../../store/event/actions";
import { selectEvents } from "../../store/event/selectors";
import { findIfLoaded } from "../../types/loadingState";
import RouteParamsParser from "../../components/shared/RouteParamsParser";
import { Override } from "../../types/models";
import Breadcrumbs from "../../components/shared/Breadcrumbs";
import { selectClubs } from "../../store/clubs/selectors";
import { GetClubsIfRequired } from "../../store/clubs/actions";
import { useThunkDispatch } from "../../store";
import { selectAccess, selectAccessToken } from "../../store/profile/selectors";

interface Props {
  readonly eventId: number;
}

const Tests: FunctionalComponent<Readonly<Props>> = ({ eventId }) => {
  const thunkDispatch = useThunkDispatch();
  const auth = useSelector(selectAccessToken);
  const currentEvent = findIfLoaded(
    useSelector(selectEvents),
    (a) => a.eventId === eventId,
  );
  const currentClub = findIfLoaded(
    useSelector(selectClubs),
    (a) => a.clubId === currentEvent?.clubId,
  );
  useEffect(() => {
    void thunkDispatch(GetEntrantsIfRequired(eventId));
  }, [eventId, thunkDispatch, auth]);
  useEffect(() => {
    thunkDispatch(GetClubsIfRequired(getAccessToken(auth)));
    void thunkDispatch(GetEventsIfRequired());
  }, [thunkDispatch, auth]);
  const access = useSelector(selectAccess);
  return (
    <div>
      <Breadcrumbs club={currentClub} event={currentEvent} />
      <Heading>Tests</Heading>
      {currentEvent ? (
        currentEvent.courses.map(({ ordinal }) => (
          <Box key={ordinal}>
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
              }}
            >
              <Heading size={5} style={{ marginBottom: 0 }}>
                Test {ordinal + 1}
              </Heading>
              <Button
                color="link"
                disabled={!access.marshalEvents.includes(eventId)}
                onClick={() => route(`/marshal/${eventId}/${ordinal}`)}
              >
                Marshal
              </Button>
            </div>
          </Box>
        ))
      ) : (
        <div>
          Loading... <Loader />
        </div>
      )}
    </div>
  );
};

export default RouteParamsParser<
  Override<
    Props,
    {
      readonly eventId: string;
    }
  >,
  Props
>(({ eventId, ...props }) => ({ ...props, eventId: Number.parseInt(eventId) }))(
  Tests,
);
