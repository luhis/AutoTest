﻿using AutoTest.Domain.Enums;

namespace AutoTest.Web.Models
{
    public class VehicleSaveModel
    {
        public string Make { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public int Year { get; set; }

        public string Registration { get; set; } = string.Empty;

        public int Displacement { get; set; }

        public Induction Induction { get; set; }
    }
}
