﻿namespace AutoTest.Domain.StorageModels
{
    public class Course
    {
        public Course(int ordinal, string mapLocation)
        {
            Ordinal = ordinal;
            this.MapLocation = mapLocation;
        }

        public int Ordinal { get; }

        public string MapLocation { get; }
    }
}
