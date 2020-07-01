namespace AutoTest.Domain.StorageModels
{
    public class Vehicle
    {
        public Vehicle(string make, string model, int year, int displacement, string registration)
        {
            Make = make;
            Model = model;
            Year = year;
            Displacement = displacement;
            Registration = registration;
        }

        public string Make { get; }

        public string Model { get; }

        public int Year { get; }

        public string Registration { get; }

        public int Displacement { get; }
    }
}
