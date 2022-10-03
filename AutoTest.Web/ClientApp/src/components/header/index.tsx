import { FunctionalComponent, h } from "preact";
import { Navbar, Button } from "react-bulma-components";
import { useDispatch, useSelector } from "react-redux";
import { useCallback, useEffect, useState } from "preact/hooks";

import { selectAccess } from "../../store/profile/selectors";
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
} from "../../store/profile/actions";
import { useGoogleAuth } from "../app";
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
    const auth = useGoogleAuth();

    const thunkDispatch = useThunkDispatch();
    const dispatch = useDispatch();

    useEffect(() => {
        void thunkDispatch(GetAccess(getAccessToken(auth)));
    }, [thunkDispatch, auth]);
    const clearCache = () => {
        dispatch(ClearCache());
    };
    const signOutAndClear = useCallback(async () => {
        await auth.signOut();
        dispatch(ResetAccess());
    }, [dispatch, auth]);
    const signInAndGetAccess = useCallback(async () => {
        await auth.signIn();
    }, [auth]);
    const [isActive, setIsActive] = useState(false);
    const toggleActive = () => setIsActive((a) => !a);
    return (
        <Navbar active={isActive}>
            <Navbar.Brand>
                <Navbar.Burger onClick={toggleActive} />
            </Navbar.Brand>
            <Navbar.Menu>
                <Navbar.Container align="right">
                    <Navbar.Item href="/">Home</Navbar.Item>
                    {access.canViewClubs ? (
                        <Navbar.Item href="/clubs">Clubs</Navbar.Item>
                    ) : null}
                    <Navbar.Item href="/events">Events</Navbar.Item>
                    {access.canViewProfile ? (
                        <Navbar.Item href="/profile">Profile</Navbar.Item>
                    ) : null}
                    <Navbar.Item>
                        <Button.Group>
                            {!access.isLoggedIn ? (
                                <Button onClick={signInAndGetAccess}>
                                    Sign in with Google
                                </Button>
                            ) : (
                                <Button onClick={signOutAndClear}>
                                    Sign out
                                </Button>
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
    const auth = useGoogleAuth();
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
