import { FunctionalComponent, h } from "preact";
import { Navbar, Button } from "react-bulma-components";
import { useDispatch, useSelector } from "react-redux";
import { useCallback, useEffect, useState } from "preact/hooks";
import { Link } from "preact-router";
import {
  CredentialResponse,
  GoogleLogin,
  googleLogout,
} from "@react-oauth/google";

import { selectAccess, selectAccessToken } from "../../store/profile/selectors";
import { ClearCache } from "../../store/event/actions";
import {
  AddClubAdmin,
  AddEditableEntrant,
  AddEditableMarshal,
  AddEventMarshal,
  GetAccess,
  RemoveClubAdmin,
  RemoveEventMarshal,
  ResetAccess,
  SetAccessToken,
} from "../../store/profile/actions";
import { getAccessToken } from "../../api/api";
import { useThunkDispatch } from "../../store";
import {
  AddEditableEntrantTag,
  AddEditableMarshalTag,
  NewClubAdminTag,
  NewEventMarshalTag,
  RemoveClubAdminTag,
  RemoveEventMarshalTag,
  useConnection,
} from "../../signalR/authorisationHub";

const Header: FunctionalComponent = () => {
  const access = useSelector(selectAccess);
  const auth = useSelector(selectAccessToken);

  const thunkDispatch = useThunkDispatch();
  const dispatch = useDispatch();

  useEffect(() => {
    void thunkDispatch(GetAccess(getAccessToken(auth)));
  }, [thunkDispatch, auth]);
  const clearCache = () => {
    dispatch(ClearCache());
  };
  const signOutAndClear = useCallback(() => {
    googleLogout();
    dispatch(ResetAccess());
  }, [dispatch]);

  const onLogin = useCallback(
    (credentialResponse: CredentialResponse) => {
      dispatch(SetAccessToken(credentialResponse));
      dispatch(GetAccess(credentialResponse.credential));
      setIsActive(false);
    },
    [dispatch],
  );
  const [isActive, setIsActive] = useState(false);
  const toggleActive = () => setIsActive((a) => !a);
  return (
    <Navbar active={isActive}>
      <Navbar.Brand>
        <Navbar.Burger onClick={toggleActive} />
      </Navbar.Brand>
      <Navbar.Menu>
        <Navbar.Container align="right">
          <Navbar.Item href="/" renderAs={Link} activeClassName="is-active">
            Home
          </Navbar.Item>
          {access.canViewClubs ? (
            <Navbar.Item
              href="/clubs"
              renderAs={Link}
              activeClassName="is-active"
            >
              Clubs
            </Navbar.Item>
          ) : null}
          <Navbar.Item
            href="/events"
            renderAs={Link}
            activeClassName="is-active"
          >
            Events
          </Navbar.Item>
          {access.canViewProfile ? (
            <Navbar.Item
              href="/profile"
              renderAs={Link}
              activeClassName="is-active"
            >
              Profile
            </Navbar.Item>
          ) : null}
          <Navbar.Item>
            <Button.Group>
              {!access.isLoggedIn ? (
                <GoogleLogin
                  onSuccess={onLogin}
                  onError={() => console.log("Auth Error")}
                />
              ) : (
                <Button onClick={signOutAndClear}>Sign out</Button>
              )}
              <Button onClick={clearCache}>Clear Cache</Button>
            </Button.Group>
          </Navbar.Item>
        </Navbar.Container>
      </Navbar.Menu>
    </Navbar>
  );
};

const SignalRWrapper: FunctionalComponent = () => {
  const auth = useSelector(selectAccessToken);
  const connection = useConnection(getAccessToken(auth));

  useEffect(() => {
    if (connection) {
      void connection.start().catch(console.error);
      return () => {
        const f = async () => {
          await connection.stop();
        };
        void f();
      };
    } else {
      return () => undefined;
    }
  }, [connection]);

  const dispatch = useDispatch();
  useEffect(() => {
    if (connection) {
      connection.on(NewClubAdminTag, (clubId: number) => {
        dispatch(AddClubAdmin(clubId));
      });
      connection.on(RemoveClubAdminTag, (clubId: number) => {
        dispatch(RemoveClubAdmin(clubId));
      });
      connection.on(NewEventMarshalTag, (eventId: number) => {
        dispatch(AddEventMarshal(eventId));
      });
      connection.on(RemoveEventMarshalTag, (eventId: number) => {
        dispatch(RemoveEventMarshal(eventId));
      });
      connection.on(AddEditableEntrantTag, (entrantId: number) => {
        dispatch(AddEditableEntrant(entrantId));
      });
      connection.on(AddEditableMarshalTag, (marshalId: number) => {
        dispatch(AddEditableMarshal(marshalId));
      });
    }
  }, [connection, dispatch]);
  return <Header />;
};

export default SignalRWrapper;
