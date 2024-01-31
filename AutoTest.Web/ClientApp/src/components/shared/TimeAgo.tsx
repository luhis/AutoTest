import TimeAgo from "javascript-time-ago";
import en from "javascript-time-ago/locale/en";
import { h, FunctionalComponent } from "preact";
import { ValidDate } from "ts-date";

TimeAgo.addDefaultLocale(en);

const timeAgo = new TimeAgo("en-US");

const TimeAgoFunc = (d: ValidDate) => timeAgo.format(d);


export const TimeAgoComp: FunctionalComponent<{ readonly d: ValidDate }> = ({
  d
}) => <span title={d.toISOString()}>{timeAgo.format(d)}</span>;

export default TimeAgoFunc;
