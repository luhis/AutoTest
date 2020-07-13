﻿namespace AutoTest.Web.Models
{
    public class EntrantSaveModel
    {
        public ushort DriverNumber { get; set; }

        public string GivenName { get; set; } = string.Empty;

        public string FamilyName { get; set; } = string.Empty;

        public string Class { get; set; } = string.Empty;

        public ulong EventId { get; set; }

        public bool IsPaid { get; set; }

        public VehicleSaveModel Vehicle { get; set; } = new VehicleSaveModel();
    }
}
