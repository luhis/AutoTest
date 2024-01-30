import TimeAgo from "javascript-time-ago";
import en from "javascript-time-ago/locale/en";
import { ValidDate } from "ts-date";

TimeAgo.addDefaultLocale(en);

const timeAgo = new TimeAgo("en-US");

const TimeAgoFunc = (d: ValidDate) => timeAgo.format(d);

export default TimeAgoFunc;
