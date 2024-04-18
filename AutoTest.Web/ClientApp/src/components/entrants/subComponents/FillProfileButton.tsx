import { FunctionComponent, h } from "preact";
import { isEmpty } from "@s-libs/micro-dash";
import { Button, Dropdown } from "react-bulma-components";

import { ClubMembership } from "../../../types/shared";

const FillProfileButton: FunctionComponent<{
  readonly clubMemberships: readonly ClubMembership[];
  readonly fillFromProfile: (club: ClubMembership | undefined) => void;
}> = ({ clubMemberships, fillFromProfile }) => {
  if (isEmpty(clubMemberships) || clubMemberships.length === 1) {
    return (
      <Button onClick={() => fillFromProfile(clubMemberships[0])}>
        Fill from Profile
      </Button>
    );
  } else {
    return (
      <Dropdown color="secondary" label="Fill from Profile">
        {clubMemberships.map((a) => (
          <Dropdown.Item
            renderAs="a"
            key={a.clubName}
            value={a.clubName}
            onClick={() => fillFromProfile(a)}
          >
            {a.clubName} {a.membershipNumber}
          </Dropdown.Item>
        ))}
      </Dropdown>
    );
  }
};

export default FillProfileButton;
