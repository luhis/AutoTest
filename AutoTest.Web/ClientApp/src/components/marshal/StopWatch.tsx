import { FunctionalComponent, h } from "preact";
import { Button } from "react-bulma-components";
import { useStopwatch } from "react-timer-hook";

const StopWatch: FunctionalComponent<{
  readonly setTime: (time: number) => void;
}> = () => {
  const { totalSeconds, start, pause } = useStopwatch();
  return (
    <span>
      {" "}
      {totalSeconds}
      <Button onClick={start}>Start</Button>
      <Button onClick={pause}>Stop</Button>
    </span>
  );
};

export default StopWatch;
