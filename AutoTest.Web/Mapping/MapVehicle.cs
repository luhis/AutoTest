using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models.Save;

namespace AutoTest.Web.Mapping
{
    public static class MapVehicle
    {
        public static Vehicle Map(VehicleSaveModel vehicle)
        {
            return new Vehicle(vehicle.Make, vehicle.Model, vehicle.Year,
                vehicle.Displacement, vehicle.Induction, vehicle.Registration);
        }
    }
}
