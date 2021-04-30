import { FunctionalComponent, h } from "preact";
import { Navbar, Button } from "react-bulma-components";
import { useDispatch, useSelector } from "react-redux";
import { useCallback, useEffect } from "preact/hooks";

import { selectAccess } from "../../store/profile/selectors";
import { ClearCache } from "../../store/event/actions";
import { GetAccess, ResetAccess } from "../../store/profile/actions";
import { useGoogleAuth } from "../app";
import { getAccessToken } from "../../api/api";

const Header: FunctionalComponent = () => {
    const access = useSelector(selectAccess);
    const auth = useGoogleAuth();
    const dispatch = useDispatch();
    useEffect(() => {
        dispatch(GetAccess(getAccessToken(auth)));
    }, [dispatch, auth]);
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
    return (
        <Navbar>
            <Navbar.Brand>
                <Navbar.Item href="#">
                    <img
                        src="https://bulma.io/images/bulma-logo.png"
                        alt=""
                        role="presentation"
                        width="112"
                        height="28"
                    />
                </Navbar.Item>
                <Navbar.Burger />
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

export default Header;
