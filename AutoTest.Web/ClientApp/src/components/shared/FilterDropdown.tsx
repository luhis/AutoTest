import { h, RenderableProps } from "preact";
import classNames from "classnames";
import { Dropdown } from "react-bulma-components";
import { StateUpdater } from "preact/hooks";

import { toggleValue } from "../../lib/form";

interface Props<T> {
  readonly filterName: string;
  readonly options: readonly T[];
  readonly selected: readonly T[];
  readonly setFilter: StateUpdater<readonly T[]>;
}

const FilterDropdown = <T extends string | number>({
  filterName,
  options,
  selected,
  setFilter,
}: RenderableProps<Props<T>>) => (
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
        onClick={() => setFilter((f) => toggleValue(f, c))}
      >
        {c}
      </a>
    ))}
  </Dropdown>
);

export default FilterDropdown;
