import { FunctionalComponent, h } from "preact";
import { Navbar, Button } from "rbx";
import { useDispatch } from "react-redux";

import { useGoogleAuth, useAccess } from "../app";
import { getAccess } from "../../api/access";
import { ClearCache } from "../../store/event/actions";

const Header: FunctionalComponent = () => {
    const { signIn, signOut } = useGoogleAuth();
    const [access, setAccess] = useAccess();
    const dispatch = useDispatch();
    const clearCache = () => {
        dispatch(ClearCache());
    };
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
                <Navbar.Segment align="start">
                    <Navbar.Item href="/">Home</Navbar.Item>
                    {access.canViewClubs ? (
                        <Navbar.Item href="/clubs">Clubs</Navbar.Item>
                    ) : null}
                    <Navbar.Item href="/events">Events</Navbar.Item>
                    {access.canViewProfile ? (
                        <Navbar.Item href="/profile">Profile</Navbar.Item>
                    ) : null}
                    <Navbar.Item>
                        {!access.isLoggedIn ? (
                            <Button
                                onClick={async (): Promise<void> => {
                                    const user = await signIn();
                                    if (user) {
                                        setAccess(
                                            await getAccess(user.tokenId)
                                        );
                                    } else {
                                        console.log(user);
                                    }
                                }}
                            >
                                Sign in with Google
                            </Button>
                        ) : (
                            <Button onClick={signOut}>Sign out</Button>
                        )}
                        <Button onClick={clearCache}>Clear Cache</Button>
                    </Navbar.Item>
                </Navbar.Segment>
            </Navbar.Menu>
        </Navbar>
    );
};

export default Header;
