namespace AutoTest.Service.ResultCalculation;

public record TimeCalculatorConfig(int FailStop, int Barrier, int Late, int NoTest)
{
    public static readonly TimeCalculatorConfig DefaultValues = new(5_000, 5_000, 5_000, 20_000);
}
