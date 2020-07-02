using System;

namespace AutoTest.Domain.StorageModels
{
    public class Vehicle
    {
        public Vehicle()
        {
        }

        public Vehicle(ulong vehicleId, string make, string model, int year, int displacement, string registration)
        {
            VehicleId = vehicleId;
            Make = make;
            Model = model;
            Year = year;
            Displacement = displacement;
            Registration = registration;
        }

        public string Make { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public int Year { get; }

        public string Registration { get; set; } = string.Empty;

        public int Displacement { get; }

        public ulong VehicleId { get; }
    }
}
