namespace AutoTest.Domain.StorageModels
{
    public class Vehicle
    {
        public Vehicle(ulong vehicleId, string @class, string make, string model, int year, int displacement)
        {
            VehicleId = vehicleId;
            Class = @class;
            Make = make;
            Model = model;
            Year = year;
            Displacement = displacement;
        }

        public ulong VehicleId { get; }

        public string Class { get; }

        public string Make { get; }

        public string Model { get; }

        public int Year { get; }

        public int Displacement { get; }
    }
}