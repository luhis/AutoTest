namespace AutoTest.Domain.StorageModels
{
    public class Course(int ordinal, string mapLocation)
    {
        public int Ordinal { get; } = ordinal;

        public string MapLocation { get; } = mapLocation;
    }
}
