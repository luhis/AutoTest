using System;

namespace AutoTest.Domain.StorageModels
{
    public class Vehicle
    {
        public Vehicle()
        {
        }

        public Vehicle(string make, string model, int year, int displacement, string registration)
        {
            Make = make;
            Model = model;
            Year = year;
            Displacement = displacement;
            Registration = registration;
        }

        public string Make { get; } = string.Empty;

        public string Model { get; } = string.Empty;

        public int Year { get; }

        public string Registration { get; } = string.Empty;

        public int Displacement { get; }
    }
}
