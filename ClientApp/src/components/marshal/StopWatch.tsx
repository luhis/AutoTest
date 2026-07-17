import { FunctionalComponent, h } from "preact";
import { Button } from "react-bulma-components";
import { useStopwatch } from "react-timer-hook";

const StopWatch: FunctionalComponent<{
  readonly setTime: (time: number) => void;
}> = ({ setTime }) => {
  const { totalSeconds, start, pause } = useStopwatch();
  const end = () => {
    pause();
    setTime(totalSeconds);
  };
  return (
    <span>
      {totalSeconds}{" "}
      <Button type="button" onClick={start}>
        Start
      </Button>
      <Button type="button" onClick={end}>
        Stop
      </Button>
    </span>
  );
};

export default StopWatch;
