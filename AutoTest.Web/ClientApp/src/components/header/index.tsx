import { FunctionalComponent, h } from "preact";
import { Navbar, Button } from "react-bulma-components";
import { useDispatch, useSelector } from "react-redux";
import { useCallback, useEffect, useState } from "preact/hooks";

import { selectAccess } from "../../store/profile/selectors";
import { ClearCache } from "../../store/event/actions";
import { GetAccess, ResetAccess } from "../../store/profile/actions";
import { useGoogleAuth } from "../app";
import { getAccessToken } from "../../api/api";
import { useThunkDispatch } from "../../store";

const Header: FunctionalComponent = () => {
    const access = useSelector(selectAccess);
    const auth = useGoogleAuth();
    const thunkDispatch = useThunkDispatch();
    const dispatch = useDispatch();
    const thunkDispatch = useThunkDispatch();
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

export default Header;
