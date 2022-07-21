import { FunctionalComponent, h } from "preact";
import { Navbar, Button } from "react-bulma-components";
import { useDispatch, useSelector } from "react-redux";
import { useCallback, useEffect, useMemo, useState } from "preact/hooks";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

import { selectAccess } from "../../store/profile/selectors";
import { ClearCache } from "../../store/event/actions";
import {
    AddClubAdmin,
    AddEventMarshal,
    GetAccess,
    RemoveClubAdmin,
    RemoveEventMarshal,
    ResetAccess,
} from "../../store/profile/actions";
import { useGoogleAuth } from "../app";
import { getAccessToken } from "../../api/api";
import { useThunkDispatch } from "../../store";

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
    const baseConn = useMemo(() => {
        const accessToken = getAccessToken(auth);
        if (accessToken === undefined) {
            return undefined;
        }
        return new HubConnectionBuilder()
            .withUrl("/authorisationHub", {
                accessTokenFactory: () => accessToken,
            })
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Error);
    }, [auth]);
    const connection = useMemo(
        () =>
            typeof window !== "undefined" && baseConn !== undefined
                ? baseConn.build()
                : undefined,
        [baseConn]
    );

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
            connection.on("NewClubAdmin", (clubId: number) => {
                dispatch(AddClubAdmin(clubId));
            });
            connection.on("RemoveClubAdmin", (clubId: number) => {
                dispatch(RemoveClubAdmin(clubId));
            });
            connection.on("NewEventMarshal", (eventId: number) => {
                dispatch(AddEventMarshal(eventId));
            });
            connection.on("RemoveEventMarshal", (eventId: number) => {
                dispatch(RemoveEventMarshal(eventId));
            });
        }
    }, [connection, dispatch]);
    return <Header />;
};

export default SignalRWrapper;
