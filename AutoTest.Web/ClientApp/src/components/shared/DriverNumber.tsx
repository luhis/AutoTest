import { h, FunctionComponent } from "preact";
import { FaCar } from "react-icons/fa";

const DriverNumber: FunctionComponent<{ readonly driverNumber: number }> = ({
    driverNumber,
}) => (
    <p class="number">
        <FaCar />
        &nbsp;
        {driverNumber}
    </p>
);

export default DriverNumber;
