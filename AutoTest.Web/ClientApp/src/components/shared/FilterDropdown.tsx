import { h, FunctionComponent } from "preact";
import classNames from "classnames";
import { Dropdown } from "react-bulma-components";
import { StateUpdater } from "preact/hooks";

const FilterDropdown: FunctionComponent<{
    readonly filterName: string;
    readonly options: readonly string[];
    readonly selected: readonly string[];
    readonly setFilter: StateUpdater<readonly string[]>;
}> = ({ filterName, options, selected, setFilter }) => (
    <Dropdown
        label={`${filterName} Filter: ${
            selected.length === 0 ? "All" : selected.join(", ")
        }`}
    >
        <a
            href="#"
            class={classNames("dropdown-item", {
                "is-active": selected.length === 0,
            })}
            onClick={() => setFilter([])}
        >
            All
        </a>
        <hr class="dropdown-divider" />
        {options.map((c) => (
            <a
                key={c}
                href="#"
                class={classNames("dropdown-item", {
                    "is-active": selected.includes(c),
                })}
                onClick={() =>
                    setFilter((f) =>
                        f.includes(c) ? f.filter((a) => a != c) : f.concat(c)
                    )
                }
            >
                {c}
            </a>
        ))}
    </Dropdown>
);

export default FilterDropdown;
