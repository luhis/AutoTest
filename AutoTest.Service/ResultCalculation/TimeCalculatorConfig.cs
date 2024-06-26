﻿namespace AutoTest.Service.ResultCalculation
{
    public class TimeCalculatorConfig
    {
        public TimeCalculatorConfig(int failStop, int barrier, int late, int noTest)
        {
            FailStop = failStop;
            Barrier = barrier;
            Late = late;
            NoTest = noTest;
        }

        public int FailStop { get; }
        public int Barrier { get; }
        public int Late { get; }
        public int NoTest { get; }

        public static readonly TimeCalculatorConfig DefaultValues = new(5_000, 5_000, 5_000, 20_000);
    }
}
