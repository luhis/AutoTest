import { FunctionalComponent, h } from "preact";
import { Navbar, Button } from "rbx";

import { useGoogleAuth, useAccess } from "../app";
import { getAccess } from "../../api/access";

const Header: FunctionalComponent = () => {
    const { signIn } = useGoogleAuth();
    const [access, setAccess] = useAccess();
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
                    <Navbar.Item>
                        <Button
                            onClick={async (): Promise<void> => {
                                const user = await signIn();
                                if (user) {
                                    setAccess(await getAccess(user.tokenId));
                                } else {
                                    console.log(user);
                                }
                            }}
                        >
                            Sign in with Google
                        </Button>
                    </Navbar.Item>
                </Navbar.Segment>
            </Navbar.Menu>
        </Navbar>
    );
};

export default Header;
